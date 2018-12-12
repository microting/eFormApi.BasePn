using System.Collections.Generic;

namespace Microting.eFormApi.BasePn.Localization
{
    public class JsonLocalization
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedValue = new Dictionary<string, string>();
    }
}