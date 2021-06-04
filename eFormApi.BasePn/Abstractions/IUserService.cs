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
        Task<TimeZoneInfo> GetCurrentUserTimeZoneInfo();
        Task<TimeZoneInfo> GetTimeZoneInfo(int userId);
        Task<string> GetCurrentUserLocale();
        Task<string> GetUserLocale(int userId);
        Task<string> GetCurrentUserFormats();
        Task<string> GetUserFormats(int userId);
        Task AddPasswordAsync(EformUser user, string password);
        Task AddToRoleAsync(EformUser user, string role);
        Task<string> GetCurrentUserFullName();
        Task<string> GetFullNameUserByUserIdAsync(int userId);
        Task<string> GetCurrentUserLanguage();
        Task<string> GetLanguageByUserIdAsync(int userId);
    }
}
