using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;

namespace Microting.eFormApi.BasePn
{
    using System.Threading.Tasks;

    public class EformBasePlugin : IEformPlugin
    {
        public string Name => "Microting eForm Base plugin";

        public string PluginId => "EformBasePlugin";

        public string PluginPath => PluginAssembly().Location;

        public string PluginBaseUrl => "EformBasePlugin";

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

        public async Task<ICollection<PluginPermissionModel>> GetPluginPermissions()
        {
            return await Task.FromResult(new List<PluginPermissionModel>());
        }

        public async Task<ICollection<PluginGroupPermissionModel>> GetPluginGroupPermissions(int? groupId = null)
        {
            return await Task.FromResult(new List<PluginGroupPermissionModel>());
        }

        public void SetPluginGroupPermissions(ICollection<PluginGroupPermissionModel> permissions)
        {

        }
    }
}