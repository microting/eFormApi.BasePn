using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

public class PluginConfigurationValueVersion : BaseEntity
{
    public string Name { get; set; }
        
    public string Value { get; set; }
}