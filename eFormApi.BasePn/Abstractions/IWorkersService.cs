using System.Collections.Generic;
using eFormShared;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IWorkersService
    {
        OperationDataResult<List<Worker_Dto>> Index();
        OperationDataResult<Worker_Dto> Edit(int id);
        OperationResult Update(WorkerModel workerModel);
        OperationResult Сreate(WorkerCreateModel model);
        OperationResult Delete(int id);
    }
}