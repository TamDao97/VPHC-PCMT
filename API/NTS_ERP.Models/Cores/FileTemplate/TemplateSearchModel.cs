using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.FileUpload
{
    public class TemplateSearchModel : SearchBaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
