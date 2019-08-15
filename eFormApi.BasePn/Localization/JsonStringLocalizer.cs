using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Microting.eFormApi.BasePn.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly List<JsonLocalization> _localization;

        public JsonStringLocalizer()
        {
            _localization =
                JsonConvert.DeserializeObject<List<JsonLocalization>>(File.ReadAllText(@"localization.json"));
        }

        public JsonStringLocalizer(Type resourceSource)
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException(nameof(resourceSource));
            }

            var assembly = resourceSource.Assembly;
            var assemblyName = assembly.GetName().Name;
            var resourceStream = assembly.GetManifestResourceStream($"{assemblyName}.Resources.localization.json");
            if (resourceStream == null)
            {
                throw new NullReferenceException($"Localization not found in {assemblyName}");
            }

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                var json = reader.ReadToEndAsync().Result;
                if (!string.IsNullOrEmpty(json))
                {
                    _localization = JsonConvert.DeserializeObject<List<JsonLocalization>>(json);
                }
            }
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localization.Where(l => l.LocalizedValue.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name))
                .Select(l => new LocalizedString(l.Key, l.LocalizedValue[CultureInfo.CurrentCulture.Name], true));
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetString(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var query = _localization.Where(l =>
                l.LocalizedValue.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name));
            var value = query.FirstOrDefault(l => l.Key == name);
            if (value == null)
            {
                return name;
            }

            var result = value.LocalizedValue[CultureInfo.CurrentCulture.Name];
            if (string.IsNullOrEmpty(result))
            {
                return name;
            }

            return result;
        }
    }
}