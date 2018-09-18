using System.Collections.Generic;
using eFormData;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models.Common;
using Microting.eFormApi.BasePn.Models.SearchableList;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface IEntitySearchService
    {
        OperationDataResult<EntityGroupList> GetEntityGroupList(
            AdvEntitySearchableGroupListRequestModel requestModel);

        OperationDataResult<List<CommonDictionaryTextModel>> GetEntityGroupDictionary(string entityGroupUid,
            string searchString);

        OperationResult CreateEntityGroup(AdvEntitySearchableGroupEditModel editModel);
        OperationResult UpdateEntityGroup(AdvEntitySearchableGroupEditModel editModel);
        OperationDataResult<EntityGroup> GetEntityGroup(string entityGroupUid);
        OperationResult DeleteEntityGroup(string entityGroupUid);
        OperationResult SendSearchableGroup(string entityGroupUid);
    }
}