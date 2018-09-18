using System.Collections.Generic;
using eFormShared;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface ISitesService
    {
        OperationDataResult<List<SiteName_Dto>> Index();
        OperationDataResult<SiteName_Dto> Edit(int id);
        OperationResult Update(SiteNameModel siteNameModel);
        OperationResult Delete(int id);
    }
}