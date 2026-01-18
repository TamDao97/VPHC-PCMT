using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.FileUpload
{
    public class FileUploadCreateModel
    {
        public int Index { get; set; }
        public string Id { get; set; }
        public string ObjectId { get; set; }
        /// <summary>
        /// Tên file người dùng đặt
        /// </summary>
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime? CreateDate { get; set; }
        public FileUploadCreateModel()
        {
            Id = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
    }
}
