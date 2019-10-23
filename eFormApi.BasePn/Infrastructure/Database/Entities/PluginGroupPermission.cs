﻿using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities
{
    public class PluginGroupPermission : BaseEntity
    {
        public int GroupId { get; set; }
        public int PermissionId { get; set; }
        public virtual PluginPermission Permission { get; set; }
    }
}