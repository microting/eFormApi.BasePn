using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

public class PluginConfigurationValue : BaseEntity
{
    public string Name { get; set; }
        
    public string Value { get; set; }
}