using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microting.eFormApi.BasePn.Abstractions;

namespace Microting.eFormApi.BasePn.Infrastructure.Settings;

public class PluginConfigurationSource<TDbContext>
    : IConfigurationSource where TDbContext : DbContext, IPluginDbContext
{
    private readonly string _connectionString;
    private readonly IPluginConfigurationSeedData _pluginConfigurationSeedData;
    private readonly IDesignTimeDbContextFactory<TDbContext> _dbContextFactory;

    public PluginConfigurationSource(
        string connectionString,
        IPluginConfigurationSeedData pluginConfigurationSeedData,
        IDesignTimeDbContextFactory<TDbContext> dbContextFactory)
    {
        _connectionString = connectionString;
        _pluginConfigurationSeedData = pluginConfigurationSeedData;
        _dbContextFactory = dbContextFactory;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new PluginConfigurationProvider<TDbContext>(
            _connectionString,
            _pluginConfigurationSeedData,
            _dbContextFactory);
    }
}