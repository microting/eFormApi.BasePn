using System;
using System.Collections.Generic;
using AutoMapper;
using eFormShared;
using Microsoft.Extensions.Localization;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Helpers;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormApi.BasePn.Models;
using Microting.eFormApi.BasePn.Resources;

namespace Microting.eFormApi.BasePn.Services
{
    public class SitesService : ISitesService
    {
        private readonly IEFormCoreService _coreHelper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SitesService(IEFormCoreService coreHelper, 
            IStringLocalizer<SharedResource> localizer)
        {
            _coreHelper = coreHelper;
            _localizer = localizer;
        }

        public OperationDataResult<List<SiteName_Dto>> Index()
        {
            var core = _coreHelper.GetCore();
            var siteNamesDto = core.Advanced_SiteItemReadAll(false);

            return new OperationDataResult<List<SiteName_Dto>>(true, siteNamesDto);
        }

        public OperationDataResult<SiteName_Dto> Edit(int id)
        {
            var core = _coreHelper.GetCore();
            var siteNameDto = core.Advanced_SiteItemRead(id);

            return !siteNameDto.Equals(null)
                ? new OperationDataResult<SiteName_Dto>(true, siteNameDto)
                : new OperationDataResult<SiteName_Dto>(false);
        }

        public OperationResult Update(SiteNameModel siteNameModel)
        {
            try
            {
                var core = _coreHelper.GetCore();
                var siteNameDto = core.Advanced_SiteItemRead(siteNameModel.Id);

                if (!siteNameDto.Equals(null))
                {
                    core.Advanced_SiteItemUpdate(siteNameDto.SiteUId, siteNameModel.SiteName);
                    return new OperationResult(true);
                }

                return new OperationResult(false);
            }
            catch (Exception)
            {
                return new OperationResult(false,
                    LocaleHelper.GetString("SiteParamCouldNotBeUpdated", siteNameModel.Id));
            }
        }

        public OperationResult Delete(int id)
        {
            try
            {
                var core = _coreHelper.GetCore();
                var siteDto = core.Advanced_SiteItemRead(id);
                SiteNameModel siteNameModel;

                if (!siteDto.Equals(null))
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<SiteName_Dto, SiteNameModel>());
                    siteNameModel =
                        Mapper.Map<SiteName_Dto, SiteNameModel>(siteDto);
                }
                else
                {
                    return new OperationResult(false, LocaleHelper.GetString("SiteParamNotFound", id));
                }

                return core.Advanced_SiteItemDelete(id)
                    ? new OperationResult(true,
                        LocaleHelper.GetString("SiteParamDeletedSuccessfully", siteNameModel.SiteName))
                    : new OperationResult(false,
                        LocaleHelper.GetString("SiteParamCouldNotBeDeleted", siteNameModel.SiteName));
            }

            catch (Exception)
            {
                return new OperationDataResult<SiteNameModel>(false,
                    LocaleHelper.GetString("SiteParamCouldNotBeDeleted", id));
            }
        }
    }
}