using System.Collections.Generic;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models.Common;
using Microting.eFormApi.BasePn.Models.Tags;

namespace Microting.eFormApi.BasePn.Abstractions
{
    public interface ITagsService
    {
        OperationDataResult<List<CommonDictionaryModel>> GetAllTags();
        OperationResult DeleteTag(int tagId);
        OperationResult CreateTag(string tagName);
        OperationResult UpdateTemplateTags(UpdateTemplateTagsModel requestModel);
    }
}