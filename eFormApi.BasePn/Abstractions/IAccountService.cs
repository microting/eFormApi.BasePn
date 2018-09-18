using System.Threading.Tasks;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models.Auth;
using Microting.eFormApi.BasePn.Models.Settings.User;
using Microting.eFormApi.BasePn.Models.User;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IAccountService
    {
        Task<OperationResult> ChangePassword(ChangePasswordModel model);
        Task<OperationResult> ForgotPassword(ForgotPasswordModel model);
        Task<UserInfoViewModel> GetUserInfo();
        Task<OperationDataResult<UserSettingsModel>> GetUserSettings();
        Task<OperationResult> ResetAdminPassword(string code);
        Task<OperationResult> ResetPassword(ResetPasswordModel model);
        Task<OperationResult> UpdateUserSettings(UserSettingsModel model);
    }
}