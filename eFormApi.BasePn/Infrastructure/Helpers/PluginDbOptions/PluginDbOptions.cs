using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microting.eForm.Infrastructure.Constants;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Delegates;

namespace Microting.eFormApi.BasePn.Infrastructure.Helpers.PluginDbOptions
{
    public class PluginDbOptions<T> : IPluginDbOptions<T> where T : class, new()
    {
        private readonly IOptionsMonitor<T> _options;

        public PluginDbOptions(
            IOptionsMonitor<T> options)
        {
            _options = options;
        }

        public T Value => _options.CurrentValue;

        public T Get(string name) => _options.Get(name);

        public async Task UpdateDb(
            Action<T> applyChanges,
            IPluginDbContext dbContext,
            int userId)
        {
            var sectionObject = _options.CurrentValue;
            applyChanges(sectionObject);
            var dictionary = GetList(sectionObject, "");
            // Update values
            await UpdateConfig(dictionary, dbContext, userId);
            // Reload configuration from database
            if (ReloadDbConfigurationDelegates.ReloadDbConfigurationDelegate != null)
            {
                var invocationList = ReloadDbConfigurationDelegates.ReloadDbConfigurationDelegate
                    .GetInvocationList();
                foreach (var func in invocationList)
                {
                    func.DynamicInvoke();
                }
            }
        }

        private static async Task UpdateConfig(Dictionary<string, string> dictionary, IPluginDbContext dbContext, int userId)
        {
            var keys = dictionary.Select(x => x.Key).ToArray();
            var configs = await dbContext.PluginConfigurationValues
                .Where(x => keys.Contains(x.Name))
                .ToListAsync();

            foreach (var configElement in dictionary)
            {
                var config = configs
                    .FirstOrDefault(x => x.Name == configElement.Key);
                if (config != null && config.Value != configElement.Value)
                {
                    // Add version
                    var currentVersion = config.Version;
                    var version = currentVersion + 1;
                    var newConfigVersion = new PluginConfigurationValueVersion()
                    {
                        Name = config.Name,
                        Value = config.Value,
                        Version = currentVersion,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        WorkflowState = Constants.WorkflowStates.Created,
                        CreatedByUserId = userId,
                        UpdatedByUserId = userId,
                    };
                    await dbContext.PluginConfigurationValueVersions.AddAsync(newConfigVersion);
                    config.Value = configElement.Value;
                    config.Version = version;
                    config.UpdatedAt = DateTime.UtcNow;
                    config.UpdatedByUserId = userId;
                    dbContext.PluginConfigurationValues.Update(config);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private static Dictionary<string, string> GetList(object currentObject, string prefix)
        {
            var dictionary = new Dictionary<string, string>();
            var sectionName = currentObject.GetType().Name;
            prefix = string.IsNullOrEmpty(prefix)
                ? $"{sectionName}"
                : $"{prefix}:{sectionName}";

            var types = new[]
            {
                typeof(string),
                typeof(int),
                typeof(bool),
                typeof(TimeSpan)
            };
            var properties = currentObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.MemberType == MemberTypes.Property)
                {
                    var value = property.GetValue(currentObject, null);
                    var type = value.GetType();
                    if (types.Contains(type))
                    {
                        dictionary.Add($"{prefix}:{property.Name}", value.ToString());
                    }
                }
            }

            return dictionary;
        }
    }
}