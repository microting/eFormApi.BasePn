using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities
{
    public class PluginGroupPermissionVersion : BaseEntity
    {
        public int GroupId { get; set; }
        public int PermissionId { get; set; }
        public bool IsEnabled { get; set; }

        public int PluginGroupPermissionId { get; set; }
    }
}