using Microsoft.EntityFrameworkCore;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IPluginSimpleDbContext
    {
        DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
        DbSet<PluginConfigurationValueVersion> PluginConfigurationValueVersions { get; set; }
    }
}