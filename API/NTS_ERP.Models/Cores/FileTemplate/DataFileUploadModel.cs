using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.FileUpload
{
    public class DataFileUploadModel
    {
        public int Index { get; set; }
        [DisplayName("Id")]
        public string Id { get; set; }
        public string ObjectId { get; set; }
        /// <summary>
        /// Tên file người dùng đặt
        /// </summary>
        [DisplayName("Tên tài liệu")]
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        [DisplayName("Loại tệp")]
        public string Extension { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
