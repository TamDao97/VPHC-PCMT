using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.TreeView
{
    public class TreeItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public List<TreeItem> Child { get; set; }
        public decimal Total { get; set; }
       public bool Expanded { get; set; }
        public bool Selected { get; set; }
        public bool HideTotal { get; set; }

        public TreeItem() {
            Id = Guid.NewGuid().ToString();
            Child = new List<TreeItem>();
        }
    }
}
