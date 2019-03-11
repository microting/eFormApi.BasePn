using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            IPluginDbContext dbContext)
        {
            var sectionObject = _options.CurrentValue;
            applyChanges(sectionObject);
            var dictionary = GetList(sectionObject, "");
            // Update values
            await UpdateConfig(dictionary, dbContext);
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

        private static async Task UpdateConfig(Dictionary<string, string> dictionary, IPluginDbContext dbContext)
        {
            var keys = dictionary.Select(x => x.Key).ToArray();
            var configs = await dbContext.PluginConfigurationValues
                .Where(x => keys.Contains(x.Id))
                .ToListAsync();

            foreach (var configElement in dictionary)
            {
                var config = configs
                    .FirstOrDefault(x => x.Id == configElement.Key);
                if (config != null && config.Value != configElement.Value)
                {
                    // Add version
                    var currentVersion = await dbContext.PluginConfigurationVersions
                        .FirstOrDefaultAsync(x => x.Id == config.Id && x.Value == config.Value);

                    var version = currentVersion == null ? 1 : currentVersion.Version++;
                    var newConfigVersion = new PluginConfigurationVersion()
                    {
                        Id = config.Id,
                        Value = config.Value,
                        Version = version,
                    };
                    await dbContext.PluginConfigurationVersions.AddAsync(newConfigVersion);
                    config.Value = configElement.Value;
                    dbContext.PluginConfigurationValues.Update(config);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private static Dictionary<string, string> GetList(object currentObject, string prefix)
        {
            var dictionary = new Dictionary<string, string>();
            var sectionName = currentObject.GetType().Name;
            prefix = prefix.IsNullOrEmpty()
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