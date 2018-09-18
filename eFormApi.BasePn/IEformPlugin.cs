using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Microting.eFormApi.BasePn
{
    public interface IEformPlugin
    {
        string GetName();
        string ConnectionStringName();
        string PluginPath();
        Assembly PluginAssembly();
        void Configure(IApplicationBuilder appBuilder);
        void ConfigureServices(IServiceCollection services);
        void ConfigureDbContext(IServiceCollection services, string connectionString);
        void SeedDatabase(string connectionString);
    }
}