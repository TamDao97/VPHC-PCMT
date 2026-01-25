using NTS_ERP.Models.Base;

namespace NTS_ERP.Models.VPHC.KeHoachKiemTraPhanGiao
{
    public class KeHoachKiemTraPhanGiaoModel : BaseModel
    {
        public string? IdKeHoachKiemTra { get; set; }
        public string IdDonVi { get; set; }
        public DateTime? NgayNhanPhanGiao { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int? SoDoiTuong { get; set; }
        public int? SoVu { get; set; }
        public decimal? TongTienXuPhat { get; set; }
    }

    public class KeHoachKiemTraPhanGiaoCreateRequestModel : BaseModel
    {
        public string IdKeHoachKiemTra { get; set; }
        public List<KeHoachKiemTraPhanGiaoModel> LstDonViPhanGiao { get; set; } = new List<KeHoachKiemTraPhanGiaoModel>();
    }

    public class KeHoachKiemTraPhanGiaoDetailResponseModel : BaseModel
    {
        public string IdKeHoachKiemTra { get; set; }
        public List<KeHoachKiemTraPhanGiaoModel> LstDonViPhanGiao { get; set; } = new List<KeHoachKiemTraPhanGiaoModel>();
    }
}
