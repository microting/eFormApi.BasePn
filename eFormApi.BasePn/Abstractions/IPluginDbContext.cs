using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IPluginDbContext 
    {
        DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
        DbSet<PluginConfigurationVersion> PluginConfigurationVersions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        int SaveChanges();
    }
}