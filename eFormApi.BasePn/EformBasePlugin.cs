using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;

namespace Microting.eFormApi.BasePn
{
    public class EformBasePlugin : IEformPlugin
    {
        public string Name => "Microting eForm Base plugin";

        public string PluginId => "EformBasePlugin";

        public string PluginPath => PluginAssembly().Location;

        public Assembly PluginAssembly()
        {
            return typeof(EformBasePlugin).GetTypeInfo().Assembly;
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEformPlugin, EformBasePlugin>();
        }

        public void ConfigureOptionsServices(IServiceCollection services, IConfiguration configuration)
        {
        }

        public void ConfigureDbContext(IServiceCollection services, string connectionString)
        {
        }

        public MenuModel HeaderMenu(IServiceProvider serviceProvider)
        {
            var result = new MenuModel();
            return result;
        }

        public void SeedDatabase(string connectionString)
        {
        }

        public void AddPluginConfig(
            IConfigurationBuilder builder,
            string connectionString)
        {
        }
    }
}