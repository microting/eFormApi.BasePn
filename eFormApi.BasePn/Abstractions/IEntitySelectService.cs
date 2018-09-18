using System.Collections.Generic;
using eFormData;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models.Common;
using Microting.eFormApi.BasePn.Models.SelectableList;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IEntitySelectService
    {
        OperationDataResult<EntityGroupList> GetEntityGroupList(
            AdvEntitySelectableGroupListRequestModel requestModel);

        OperationResult CreateEntityGroup(AdvEntitySelectableGroupEditModel editModel);
        OperationResult UpdateEntityGroup(AdvEntitySelectableGroupEditModel editModel);
        OperationDataResult<EntityGroup> GetEntityGroup(string entityGroupUid);
        OperationDataResult<List<CommonDictionaryTextModel>> GetEntityGroupDictionary(string entityGroupUid);
        OperationResult SendSearchableGroup(string entityGroupUid);
        OperationResult DeleteEntityGroup(string entityGroupUid);
    }
}