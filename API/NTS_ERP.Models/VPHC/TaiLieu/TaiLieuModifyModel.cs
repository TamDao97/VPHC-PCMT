using NTS.Common.Files;
using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.VPHC.Nguoi;
using System.Collections.Generic;

namespace NTS_ERP.Models.VPHC.TaiLieu
{
    public class TaiLieuModifyModel
    {
        public string Id { get; set; } = "";
        public string IdVuViec { get; set; } = "";
        public string IdCategory { get; set; } = "";

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public List<UploadResultModel> ListFileUpload { get; set; } = new List<UploadResultModel>();
    }
}