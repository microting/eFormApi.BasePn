using Microsoft.EntityFrameworkCore;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IPluginDbContext
    {
        DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
        DbSet<PluginConfigurationValueVersion> PluginConfigurationValueVersions { get; set; }
    }
}