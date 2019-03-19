using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IPluginDbContext : 
        IDisposable,
        IInfrastructure<IServiceProvider>,
        IDbContextDependencies,
        IDbSetCache,
        IDbQueryCache,
        IDbContextPoolable
    {
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken));

        DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
        DbSet<PluginConfigurationValueVersion> PluginConfigurationValueVersions { get; set; }
    }
}