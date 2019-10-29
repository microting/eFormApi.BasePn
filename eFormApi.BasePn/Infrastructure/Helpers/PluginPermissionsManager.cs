namespace Microting.eFormApi.BasePn.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Abstractions;
    using Database.Entities;
    using Microsoft.EntityFrameworkCore;
    using Models.Application;
    using Microting.eForm.Infrastructure.Constants;

    public class PluginPermissionsManager
    {
        private readonly IPluginDbContext _dbContext;

        public PluginPermissionsManager(IPluginDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<PluginPermissionModel>> GetPluginPermissions()
        {
            return await _dbContext.PluginPermissions.Select(p => new PluginPermissionModel()
            {
                PermissionId = p.Id,
                PermissionName = p.PermissionName,
                ClaimName = p.ClaimName
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
                    PermissionName = p.PermissionName,
                    ClaimName = p.ClaimName,
                    IsEnabled = g.Any(gp => gp.PermissionId == p.Id && gp.IsEnabled)
                }).ToList()
            }).ToListAsync();
        }

        public async Task SetPluginGroupPermissions(ICollection<PluginGroupPermissionsListModel> groupPermissions)
        {
            foreach (var groupPermissionModel in groupPermissions)
            {
                foreach (var permissionModel in groupPermissionModel.Permissions)
                {
                    var pluginGroupPermission = await _dbContext.PluginGroupPermissions
                        .FirstOrDefaultAsync(pgp => pgp.GroupId == groupPermissionModel.GroupId 
                            && pgp.PermissionId == permissionModel.PermissionId
                            && pgp.WorkflowState != Constants.WorkflowStates.Removed);

                    if (pluginGroupPermission != null)
                    {
                        pluginGroupPermission.Update(_dbContext);
                    } 
                    else
                    {
                        pluginGroupPermission = new PluginGroupPermission
                        {
                            GroupId = groupPermissionModel.GroupId,
                            PermissionId = permissionModel.PermissionId,
                            IsEnabled = permissionModel.IsEnabled
                        };
                        pluginGroupPermission.Create(_dbContext);
                    }
                }
            }
        }
    }
}