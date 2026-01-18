using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Category
{
    public class CategorySearchResultModel
    {
        public string CategoryId { get; set; }
        public string GroupCategoryId { get; set; }
        public string GroupCategoryName { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string TableName { get; set; }
    }
}
