﻿using System.Threading.Tasks;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models.Settings.Admin;
using Microting.eFormApi.BasePn.Models.Settings.Initial;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface ISettingsService
    {
        OperationResult ConnectionStringExist();
        OperationDataResult<AdminSettingsModel> GetAdminSettings();
        OperationDataResult<string> GetDefaultLocale();
        OperationDataResult<LoginPageSettingsModel> GetLoginPageSettings();
        OperationDataResult<HeaderSettingsModel> GetPageHeaderSettings();
        OperationResult ResetLoginPageSettings();
        OperationResult ResetPageHeaderSettings();
        OperationResult UpdateAdminSettings(AdminSettingsModel adminSettingsModel);
        Task<OperationResult> UpdateConnectionString(InitialSettingsModel initialSettingsModel);
    }
}