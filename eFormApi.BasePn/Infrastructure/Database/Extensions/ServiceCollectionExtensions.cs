using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microting.eFormApi.BasePn.Infrastructure.Helpers.PluginDbOptions;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigurePluginDbOptions<T>(
        this IServiceCollection services,
        IConfigurationSection section) where T : class, new()
    {
        services.Configure<T>(section);
        services.AddTransient<IPluginDbOptions<T>>(provider =>
        {
            var options = provider.GetService<IOptionsMonitor<T>>();
            return new PluginDbOptions<T>(options);
        });
    }

}