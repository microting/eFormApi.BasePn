using System.Collections.Generic;
using eFormShared;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IUnitsService
    {
        OperationDataResult<List<Unit_Dto>> Index();
        OperationDataResult<Unit_Dto> RequestOtp(int id);
    }
}