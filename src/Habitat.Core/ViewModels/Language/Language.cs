using BoC.CmsHelpers.Sitecore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Habitat.Core.ViewModels.Language
{
    public class Language: SitecoreItem
    {
        private CultureInfo _cultureInfo;
        public string FallbackLanguage { get; set; }
        public string Encoding  { get; set; }
        public string Iso { get; set; }
        public string WorldLingoLanguageIdentifier { get; set; }
        public string Charset { get; set; }
        public string CodePage { get; set; }
        public string RegionalIsoCode { get; set; }
        public CultureInfo CultureInfo => _cultureInfo ?? (_cultureInfo = new System.Globalization.CultureInfo(this.Iso));
    }
}
