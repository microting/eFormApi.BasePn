using System;
using System.Collections.Generic;
using eFormShared;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Helpers;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace Microting.eFormApi.BasePn.Services
{
    public class UnitsService : IUnitsService
    {
        private readonly IEFormCoreService _coreHelper;

        public UnitsService(IEFormCoreService coreHelper)
        {
            _coreHelper = coreHelper;
        }

        public OperationDataResult<List<Unit_Dto>> Index()
        {
            var core = _coreHelper.GetCore();
            var unitsDto = core.Advanced_UnitReadAll();
            return new OperationDataResult<List<Unit_Dto>>(true, unitsDto);
        }

        public OperationDataResult<Unit_Dto> RequestOtp(int id)
        {
            try
            {
                var core = _coreHelper.GetCore();
                var unitDto = core.Advanced_UnitRequestOtp(id);
                return new OperationDataResult<Unit_Dto>(true, LocaleHelper.GetString("NewOTPCreatedSuccessfully"),
                    unitDto);
            }
            catch (Exception)
            {
                return new OperationDataResult<Unit_Dto>(false,
                    LocaleHelper.GetString("UnitParamOTPCouldNotCompleted", id));
            }
        }
    }
}