using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microting.eFormApi.BasePn.Abstractions;

namespace Microting.eFormApi.BasePn.Infrastructure.Helpers.PluginDbOptions
{
    public interface IPluginDbOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        Task UpdateDb(
            Action<T> applyChanges,
            IPluginDbContext dbContext,
            int userId);
    }
}