namespace Microting.eFormApi.BasePn.Infrastructure.Models.Application
{
    using System.Collections.Generic;

    public class PluginGroupPermissionsListModel
    {
        public int GroupId { get; set; }

        public ICollection<PluginGroupPermissionModel> Permissions = new List<PluginGroupPermissionModel>();
    }
}