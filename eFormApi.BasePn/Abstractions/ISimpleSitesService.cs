using System.Collections.Generic;
using eFormShared;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface ISimpleSitesService
    {
        OperationDataResult<List<Site_Dto>> Index();
        OperationResult Create(SimpleSiteModel simpleSiteModel);
        OperationDataResult<Site_Dto> Edit(int id);
        OperationResult Update(SimpleSiteModel simpleSiteModel);
        OperationResult Delete(int id);
    }
}