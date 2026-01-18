using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Menu
{
    public class MenuSearchModel : SearchBaseModel
    {
        public string TitleKeyTranslate { get; set; }
        public string TitleDefault { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
    }
}
