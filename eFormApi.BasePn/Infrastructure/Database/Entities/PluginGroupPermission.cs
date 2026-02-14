using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.eForm.Infrastructure.Constants;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Entities;

public class PluginGroupPermission : BaseEntity
{
    public int GroupId { get; set; }
    public int PermissionId { get; set; }
    public bool IsEnabled { get; set; }
    public virtual PluginPermission Permission { get; set; }

    public void Create(IPluginDbContext dbContext)
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Version = 1;
        WorkflowState = Constants.WorkflowStates.Created;

        dbContext.PluginGroupPermissions.Add(this);
        dbContext.SaveChanges();

        dbContext.PluginGroupPermissionVersions.Add(MapVersions(this));
        dbContext.SaveChanges();
    }

    public void Update(IPluginDbContext dbContext)
    {
        PluginGroupPermission pluginGroupPermission = dbContext.PluginGroupPermissions.FirstOrDefault(x => x.Id == Id);

        if (pluginGroupPermission == null)
        {
            throw new NullReferenceException($"Could not find pluginGroupPermission with id {Id}");
        }

        pluginGroupPermission.WorkflowState = WorkflowState;
        pluginGroupPermission.GroupId = pluginGroupPermission.GroupId;
        pluginGroupPermission.PermissionId = pluginGroupPermission.PermissionId;
        pluginGroupPermission.IsEnabled = pluginGroupPermission.IsEnabled;
        pluginGroupPermission.CreatedByUserId = CreatedByUserId;
        pluginGroupPermission.UpdatedByUserId = UpdatedByUserId;

        if ((dbContext as DbContext).ChangeTracker.HasChanges())
        {
            pluginGroupPermission.UpdatedAt = DateTime.UtcNow;
            pluginGroupPermission.Version += 1;

            dbContext.PluginGroupPermissionVersions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }
    }

    public void Delete(IPluginDbContext dbContext)
    {
        PluginGroupPermission pluginGroupPermission = dbContext.PluginGroupPermissions.FirstOrDefault(x => x.Id == Id);

        if (pluginGroupPermission == null)
        {
            throw new NullReferenceException($"Could not find pluginGroupPermission with id {Id}");
        }

        pluginGroupPermission.WorkflowState = Constants.WorkflowStates.Removed;

        if ((dbContext as DbContext).ChangeTracker.HasChanges())
        {
            pluginGroupPermission.UpdatedAt = DateTime.UtcNow;
            pluginGroupPermission.Version += 1;

            dbContext.PluginGroupPermissionVersions.Add(MapVersions(this));
            dbContext.SaveChanges();
        }
    }

    private PluginGroupPermissionVersion MapVersions(PluginGroupPermission pluginGroupPermission)
    {
        PluginGroupPermissionVersion pluginGroupPermissionVersion = new PluginGroupPermissionVersion
        {
            Version = pluginGroupPermission.Version,
            GroupId = pluginGroupPermission.GroupId,
            PermissionId = pluginGroupPermission.PermissionId,
            IsEnabled = pluginGroupPermission.IsEnabled,
            CreatedAt = pluginGroupPermission.CreatedAt,
            UpdatedAt = pluginGroupPermission.UpdatedAt,
            CreatedByUserId = pluginGroupPermission.CreatedByUserId,
            UpdatedByUserId = pluginGroupPermission.UpdatedByUserId,
            WorkflowState = pluginGroupPermission.WorkflowState,
            PluginGroupPermissionId = pluginGroupPermission.Id
        };

        return pluginGroupPermissionVersion;
    }
}