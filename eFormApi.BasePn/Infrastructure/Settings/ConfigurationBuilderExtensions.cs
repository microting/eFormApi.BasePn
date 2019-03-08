using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microting.eFormApi.BasePn.Abstractions;

namespace Microting.eFormApi.BasePn.Infrastructure.Settings
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddPluginConfiguration<TDbContext>(
            this IConfigurationBuilder builder,
            string connectionString,
            IPluginConfigurationSeedData pluginConfigurationSeedData,
            IDesignTimeDbContextFactory<TDbContext> dbContextFactory) where TDbContext : DbContext, IPluginDbContext
        {
            return builder.Add(new PluginConfigurationSource<TDbContext>(
                connectionString,
                pluginConfigurationSeedData,
                dbContextFactory));
        }
    }
}