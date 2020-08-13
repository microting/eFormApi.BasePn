namespace Microting.eFormApi.BasePn.Abstractions
{
    using System;
    using System.Threading.Tasks;
    using Infrastructure.Database.Entities;

    public interface IUserService
    {
        int UserId { get; }
        string Role { get; }
        bool IsInRole(string role);
        bool IsAdmin();
        Task<EformUser> GetByIdAsync(int id);
        Task<EformUser> GetByUsernameAsync(string username);
        Task<EformUser> GetCurrentUserAsync();
        Task<TimeZoneInfo> GetUserTimeZoneInfo();
        Task<TimeZoneInfo> GetUserTimeZoneInfo(string userId);
        Task AddPasswordAsync(EformUser user, string password);
        Task AddToRoleAsync(EformUser user, string role);
    }
}
