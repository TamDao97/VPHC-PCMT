using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.GroupCategory
{
    public class GroupCategoryCreateModel
    {
        public string GroupCategoryId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}
