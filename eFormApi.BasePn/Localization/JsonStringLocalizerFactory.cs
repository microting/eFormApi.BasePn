using System;
using Microsoft.Extensions.Localization;
using Microting.eFormApi.BasePn.Localization.Abstractions;

namespace Microting.eFormApi.BasePn.Localization
{
    public class JsonStringLocalizerFactory : IEformLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(resourceSource);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer();
        }
    }
}