using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Category
{
    public class GroupCategoryModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string TableName { get; set; }
    }
}
