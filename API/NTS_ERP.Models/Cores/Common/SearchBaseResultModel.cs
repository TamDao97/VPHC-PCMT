using System;
using System.Collections.Generic;
using System.Text;

namespace NTS_ERP.Models.Cores.Common
{
    public class SearchBaseResultModel<T>
    {
        public int TotalItems { get; set; }
        public List<T> DataResults { get; set; }

        public SearchBaseResultModel()
        {
            DataResults = new List<T>();
        }
    }
}
