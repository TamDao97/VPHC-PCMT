using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Controls
{
    public class InputListToaDoModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 3: Cấp phòng CoreProject tỉnh
        /// 4: Đội phòng chống ma tuý của đồn
        /// </summary>
        public int Level { get; set; }

        public InputListToaDoModel()
        {
            Id = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
    }
}
