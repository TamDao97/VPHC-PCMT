using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    public class FuntionConfigSearchResultModel
    {
        public string Id { get; set; }
        public string FunctionName { get; set; }
        public string TableName { get; set; }
        public string Slug { get; set; }
        public int Index { get; set; }
    }
}
