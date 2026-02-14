using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

public class PluginPermission : BaseEntity
{
    public string PermissionName { get; set; }
    public string ClaimName { get; set; }
}