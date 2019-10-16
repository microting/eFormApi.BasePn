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
    using Infrastructure.Helpers;

    public interface IEformPlugin
    {
        string Name { get; }
        string PluginId { get; }
        string PluginPath { get; }
        string PluginBaseUrl { get; }
        Assembly PluginAssembly();
        void Configure(IApplicationBuilder appBuilder);
        void ConfigureServices(IServiceCollection services);
        void ConfigureOptionsServices(
            IServiceCollection services,
            IConfiguration configuration);

        void ConfigureDbContext(IServiceCollection services, string connectionString);
        MenuModel HeaderMenu(IServiceProvider serviceProvider);
        void SeedDatabase(string connectionString);
        void AddPluginConfig(
            IConfigurationBuilder builder,
            string connectionString);

        PluginPermissionsManager GetPermissionsManager(string connectionString);
    }
}