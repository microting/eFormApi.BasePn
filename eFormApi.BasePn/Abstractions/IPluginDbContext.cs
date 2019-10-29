using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IPluginDbContext
    {
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken));

        DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
        DbSet<PluginConfigurationValueVersion> PluginConfigurationValueVersions { get; set; }
        DbSet<PluginPermission> PluginPermissions { get; set; }
        DbSet<PluginGroupPermission> PluginGroupPermissions { get; set; }
        DbSet<PluginGroupPermissionVersion> PluginGroupPermissionVersions { get; set; }
    }
}