namespace Microting.eFormApi.BasePn.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Abstractions;
    using Database.Entities;
    using Microsoft.EntityFrameworkCore;
    using Models.Application;

    public class PluginPermissionsHelper
    {
        private readonly IPluginDbContext _dbContext;

        public PluginPermissionsHelper(IPluginDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<PluginPermissionModel>> GetPluginPermissions()
        {
            return await _dbContext.PluginPermissions.Select(p => new PluginPermissionModel()
            {
                PermissionId = p.Id,
                Name = p.Name
            }).ToListAsync();
        }

        public async Task<ICollection<PluginGroupPermissionModel>> GetPluginGroupPermissions(int? groupId = null)
        {
            var query = _dbContext.PluginGroupPermissions.AsQueryable();

            if (groupId != null)
            {
                query = query.Where(p => p.GroupId == groupId);
            }

            return await query.Select(p => new PluginGroupPermissionModel()
            {
                PermissionId = p.PermissionId,
                GroupId = p.GroupId,
                Name = p.Permission.Name
            }).ToListAsync();
        }

        public async Task SetPluginGroupPermissions(ICollection<PluginGroupPermissionModel> groupPermissions)
        {
            var permissionsToDelete = await _dbContext.PluginGroupPermissions
                .Where(p => groupPermissions.Any(m => m.PermissionId == p.PermissionId))
                .ToListAsync();
            var permissionsToAdd = groupPermissions
                .Where(m => _dbContext.PluginGroupPermissions.All(p => p.PermissionId != m.PermissionId))
                .ToList();

            foreach (var permission in permissionsToDelete)
            {
                _dbContext.PluginGroupPermissions.Remove(permission);
            }

            foreach (var model in permissionsToAdd)
            {
                _dbContext.PluginGroupPermissions.Add(new PluginGroupPermission
                {
                    PermissionId = model.PermissionId,
                    GroupId = model.PermissionId
                });
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}