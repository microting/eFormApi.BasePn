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
            }).OrderBy(x => x.ClaimName).ToListAsync();
        }

        public async Task<ICollection<PluginGroupPermissionsListModel>> GetPluginGroupPermissions(int? groupId = null)
        {
            var query = _dbContext.PluginGroupPermissions.AsQueryable();

            if (groupId != null)
            {
                query = query.Where(p => p.GroupId == groupId);
            }

            if (query.Any())
            {
                var pluginGroupPermissionsListModels = new List<PluginGroupPermissionsListModel>();
                foreach (var pluginGroupPermission in query.Select(x => x.GroupId).Distinct().ToList())
                {
                    PluginGroupPermissionsListModel pluginGroupPermissionsListModel = new PluginGroupPermissionsListModel()
                    {
                        GroupId = pluginGroupPermission,
                        Permissions = _dbContext.PluginPermissions.Select(p => new PluginGroupPermissionModel
                        {
                            PermissionId = p.Id,
                            PermissionName = p.PermissionName,
                            ClaimName = p.ClaimName,
                            IsEnabled = query.SingleOrDefault(x =>
                                x.PermissionId == p.Id && x.GroupId == pluginGroupPermission).IsEnabled
                        }).OrderBy(x => x.ClaimName).ToList()
                    };
                    pluginGroupPermissionsListModels.Add(pluginGroupPermissionsListModel);
                }
                return pluginGroupPermissionsListModels;
            }

            return new List<PluginGroupPermissionsListModel>();
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
                        pluginGroupPermission.IsEnabled = permissionModel.IsEnabled;
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