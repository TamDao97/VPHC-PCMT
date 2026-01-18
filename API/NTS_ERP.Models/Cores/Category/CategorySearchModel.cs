using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Category
{
    public class CategorySearchModel : SearchBaseModel
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public string GroupCategoryId { get; set; }
        public string IdTinh { get; set; }
        public string IdHuyen { get; set; }
    }
}
