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

        public async Task<ICollection<PluginGroupPermissionsListModel>> GetPluginGroupPermissions(int? groupId = null)
        {
            var query = _dbContext.PluginGroupPermissions.AsQueryable();

            if (groupId != null)
            {
                query = query.Where(p => p.GroupId == groupId);
            }

            return await query.GroupBy(p => p.GroupId).Select(g => new PluginGroupPermissionsListModel()
            {
                GroupId = g.Key,
                Permissions = _dbContext.PluginPermissions.Select(p => new PluginGroupPermissionModel
                {
                    PermissionId = p.Id,
                    Name = p.Name,
                    IsEnabled = g.Any(gp => gp.PermissionId == p.Id)
                }).ToList()
            }).ToListAsync();
        }

        public async Task SetPluginGroupPermissions(ICollection<PluginGroupPermissionsListModel> groupPermissions)
        {
            var permissionsToDelete = await _dbContext.PluginGroupPermissions
                .Where(p => groupPermissions
                    .Where(m => m.GroupId == p.GroupId)
                    .Any(m => m.Permissions
                        .Any(mp => !mp.IsEnabled && p.PermissionId == mp.PermissionId)))
                .ToListAsync();

            foreach (var permission in permissionsToDelete)
            {
                _dbContext.PluginGroupPermissions.Remove(permission);
            }

            foreach (var groupPermissionModel in groupPermissions)
            {
                var permissionsToAdd = groupPermissionModel.Permissions
                    .Where(mp => mp.IsEnabled && _dbContext.PluginGroupPermissions.All(p => p.PermissionId != mp.PermissionId));

                foreach (var model in permissionsToAdd)
                {
                    _dbContext.PluginGroupPermissions.Add(new PluginGroupPermission
                    {
                        PermissionId = model.PermissionId,
                        GroupId = groupPermissionModel.GroupId
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}