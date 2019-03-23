using System.Linq;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Delegates;

// ReSharper disable RedundantAssignment
// ReSharper disable UnusedParameter.Local

namespace Microting.eFormApi.BasePn.Infrastructure.Settings
{
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
            if (_connectionString.IsNullOrEmpty() || _connectionString == "...")
            {
                Data = seedData.ToDictionary(
                    item => item.Name,
                    item => item.Value);
            }
            else
            {
                using (var dbContext = _dbContextFactory.CreateDbContext(new[] {_connectionString}))
                {
                    dbContext.Database.Migrate();
                    Data = dbContext.PluginConfigurationValues
                        .AsNoTracking()
                        .ToDictionary(c => c.Name, c => c.Value);

                    if (!Data.Any())
                    {
                        Data = seedData.ToDictionary(
                            item => item.Name,
                            item => item.Value);
                    }
                }
            }
        }
    }
}