using eFormData;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models.Cases.Request;
using Microting.eFormApi.BasePn.Models.Cases.Response;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface ICasesService
    {
        OperationDataResult<CaseListModel> Index(CaseRequestModel requestModel);
        OperationDataResult<ReplyElement> Edit(int id);
        OperationResult Delete(int id);
        OperationResult Update(ReplyRequest model);
    }
}