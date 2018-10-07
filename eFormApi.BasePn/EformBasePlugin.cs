using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microting.eFormApi.BasePn.Infrastructure.Helpers;

namespace Microting.eFormApi.BasePn
{
    public class EformBasePlugin : IEformPlugin
    {
        public string GetName() => "Microting eForm Base plugin";

        public string ConnectionStringName() => "EformBasePlugin";

        public string PluginPath()
        {
            return PluginAssembly().Location;
        }

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

        public void ConfigureDbContext(IServiceCollection services, string connectionString)
        {
        }

        public void SeedDatabase(string connectionString)
        {
        }
    }
}