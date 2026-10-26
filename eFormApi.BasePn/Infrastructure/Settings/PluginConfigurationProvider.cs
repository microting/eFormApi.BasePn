using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Delegates;

// ReSharper disable RedundantAssignment
// ReSharper disable UnusedParameter.Local

namespace Microting.eFormApi.BasePn.Infrastructure.Settings;

public class PluginConfigurationProvider<TDbContext>
    : ConfigurationProvider where TDbContext
    : DbContext, IPluginDbContext
{
    private readonly string _connectionString;
    private readonly IPluginConfigurationSeedData _pluginConfigurationSeedData;
    private readonly IDesignTimeDbContextFactory<TDbContext> _dbContextFactory;

    public PluginConfigurationProvider(
        string connectionString,
        IPluginConfigurationSeedData pluginConfigurationSeedData,
        IDesignTimeDbContextFactory<TDbContext> dbContextFactory)
    {
        _connectionString = connectionString;
        _pluginConfigurationSeedData = pluginConfigurationSeedData;
        _dbContextFactory = dbContextFactory;
        ReloadDbConfigurationDelegates.ReloadDbConfigurationDelegate += ReloadPluginConfiguration;
    }

    private void ReloadPluginConfiguration()
    {
        Load();
        OnReload();
    }

    // Load config data from EF DB.
    public override void Load()
    {
        var seedData = _pluginConfigurationSeedData.Data;
        if (string.IsNullOrEmpty(_connectionString) || _connectionString == "...")
        {
            Data = seedData.ToDictionary(
                item => item.Name,
                item => item.Value);
        }
        else
        {
            using var dbContext = _dbContextFactory.CreateDbContext(new[] {_connectionString});
            dbContext.Database.Migrate();
            // Start from the seed defaults, so keys missing in the DB still get a value,
            // and let the DB rows win
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in seedData)
            {
                data[item.Name] = item.Value;
            }

            foreach (var item in dbContext.PluginConfigurationValues.AsNoTracking())
            {
                data[item.Name] = item.Value;
            }

            Data = data;
        }
    }
}