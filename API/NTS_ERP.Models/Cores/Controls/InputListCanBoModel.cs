using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.Controls
{
    public class InputListCanBoModel
    {
        public string Id { get; set; }
        public string IdCanBo { get; set; }
        public string Name { get; set; }
        public string IdCapBac { get; set; }
        public string CapBac { get; set; }
        public string IdChucVu { get; set; }
        public string ChucVu { get; set; }
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 3: Cấp phòng CoreProject tỉnh
        /// 4: Đội phòng chống ma tuý của đồn
        /// </summary>
        public int Level { get; set; }

        public InputListCanBoModel()
        {
            Id = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
    }
}
