using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microting.eFormApi.BasePn.Infrastructure.Models.Application;

namespace Microting.eFormApi.BasePn
{
    public interface IEformPlugin
    {
        string Name { get; }
        string PluginId { get; }
        string PluginPath { get; }
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
    }
}