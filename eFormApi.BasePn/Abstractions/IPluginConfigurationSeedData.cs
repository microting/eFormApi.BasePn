using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace Microting.eFormApi.BasePn.Abstractions;

public interface IPluginConfigurationSeedData
{
    PluginConfigurationValue[] Data { get; }
}