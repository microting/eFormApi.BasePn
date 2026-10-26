using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Database.Extensions;
using Microting.eFormApi.BasePn.Infrastructure.Delegates;
using Microting.eFormApi.BasePn.Infrastructure.Helpers.PluginDbOptions;
using Microting.eFormApi.BasePn.Infrastructure.Settings;
using NUnit.Framework;

namespace Microting.eFormApi.BasePn.Test;

public class TestPluginSettings
{
    public string Name { get; set; }

    public bool? SelfHealEnabled { get; set; }
}

public class TestPluginDbContext(DbContextOptions<TestPluginDbContext> options)
    : DbContext(options), IPluginDbContext
{
    public DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
    public DbSet<PluginConfigurationValueVersion> PluginConfigurationValueVersions { get; set; }
    public DbSet<PluginPermission> PluginPermissions { get; set; }
    public DbSet<PluginGroupPermission> PluginGroupPermissions { get; set; }
    public DbSet<PluginGroupPermissionVersion> PluginGroupPermissionVersions { get; set; }
}

public class TestPluginDbContextFactory(SqliteConnection connection)
    : IDesignTimeDbContextFactory<TestPluginDbContext>
{
    public TestPluginDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TestPluginDbContext>()
            .UseSqlite(connection)
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
            .Options;
        return new TestPluginDbContext(options);
    }
}

public class TestSeedData(params PluginConfigurationValue[] data) : IPluginConfigurationSeedData
{
    public PluginConfigurationValue[] Data { get; } = data;
}

[TestFixture]
public class PluginConfigurationTests
{
    private const string NameKey = "TestPluginSettings:Name";
    private const string SelfHealKey = "TestPluginSettings:SelfHealEnabled";

    private SqliteConnection _connection;
    private TestPluginDbContextFactory _factory;

    [SetUp]
    public void SetUp()
    {
        ReloadDbConfigurationDelegates.ReloadDbConfigurationDelegate = null;
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _factory = new TestPluginDbContextFactory(_connection);
        using var dbContext = _factory.CreateDbContext([]);
        dbContext.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        ReloadDbConfigurationDelegates.ReloadDbConfigurationDelegate = null;
        _connection.Dispose();
    }

    private void AddDbRows(params (string Name, string Value)[] rows)
    {
        using var dbContext = _factory.CreateDbContext([]);
        foreach (var (name, value) in rows)
        {
            dbContext.PluginConfigurationValues.Add(new PluginConfigurationValue
            {
                Name = name,
                Value = value,
                Version = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        dbContext.SaveChanges();
    }

    private string GetDbValue(string name)
    {
        using var dbContext = _factory.CreateDbContext([]);
        return dbContext.PluginConfigurationValues.AsNoTracking().Single(x => x.Name == name).Value;
    }

    private (IConfigurationRoot Configuration, ServiceProvider Services) Build(TestSeedData seedData)
    {
        var configuration = new ConfigurationBuilder()
            .AddPluginConfiguration("DataSource=:memory:", seedData, _factory)
            .Build();
        var services = new ServiceCollection();
        services.ConfigurePluginDbOptions<TestPluginSettings>(configuration.GetSection("TestPluginSettings"));
        return (configuration, services.BuildServiceProvider());
    }

    [Test]
    public void Load_SeedKeyMissingFromNonEmptyTable_UsesSeedDefault()
    {
        AddDbRows((NameKey, "db-name"));
        var seedData = new TestSeedData(
            new PluginConfigurationValue { Name = NameKey, Value = "seed-name" },
            new PluginConfigurationValue { Name = SelfHealKey, Value = "true" });

        var (configuration, _) = Build(seedData);

        Assert.That(configuration[NameKey], Is.EqualTo("db-name"));
        Assert.That(configuration[SelfHealKey], Is.EqualTo("true"));
    }

    [Test]
    public async Task UpdateDb_NullableSettingIsNull_DoesNotThrowAndSavesOtherValues()
    {
        AddDbRows((NameKey, "db-name"));
        var seedData = new TestSeedData(
            new PluginConfigurationValue { Name = NameKey, Value = "seed-name" });
        var (_, services) = Build(seedData);
        var options = services.GetRequiredService<IPluginDbOptions<TestPluginSettings>>();
        Assert.That(options.Value.SelfHealEnabled, Is.Null);

        await using var dbContext = _factory.CreateDbContext([]);
        await options.UpdateDb(settings => settings.Name = "new-name", dbContext, 1);

        Assert.That(GetDbValue(NameKey), Is.EqualTo("new-name"));
    }

    [Test]
    public async Task UpdateDb_NullableSettingHasValue_IsSaved()
    {
        AddDbRows((NameKey, "db-name"), (SelfHealKey, "False"));
        var seedData = new TestSeedData(
            new PluginConfigurationValue { Name = NameKey, Value = "seed-name" },
            new PluginConfigurationValue { Name = SelfHealKey, Value = "False" });
        var (_, services) = Build(seedData);
        var options = services.GetRequiredService<IPluginDbOptions<TestPluginSettings>>();

        await using var dbContext = _factory.CreateDbContext([]);
        await options.UpdateDb(settings => settings.SelfHealEnabled = true, dbContext, 1);

        Assert.That(GetDbValue(SelfHealKey), Is.EqualTo("True"));
        Assert.That(options.Value.SelfHealEnabled, Is.True);
    }
}
