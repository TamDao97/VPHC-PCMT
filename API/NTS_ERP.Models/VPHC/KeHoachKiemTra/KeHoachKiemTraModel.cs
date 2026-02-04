using NTS_ERP.Models.Base;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.VPHC.TraCuu;
using System.ComponentModel;

namespace NTS_ERP.Models.VPHC.KeHoachKiemTra
{
    public class KeHoachKiemTraModel : BaseModel
    {
        /// <summary>
        /// 1:Cục, 2:Phòng, 3:Đồn
        /// </summary>
        public int? CapKeHoach { get; set; }

        public string? IdKeHoachCha { get; set; }
        public string? IdDonVi { get; set; }

        public string? SoQuyetDinhBanHanh { get; set; }
        public string? CanCu { get; set; }
        public string? MucDich { get; set; }
        public string? YeuCau { get; set; }
        public string? NoiDungKiemTra { get; set; }

        public DateTime? TuNgayThucHienKeHoach { get; set; }
        public DateTime? DenNgayThucHienKeHoach { get; set; }

        /// <summary>
        /// 1:Soạn thảo
        /// 2:Trình duyệt
        /// 3:Đã ban hành
        /// 4:Tiếp nhận
        /// 5:Đang triển khai
        /// 9:Ngưng
        /// </summary>
        public int? TrangThaiKeHoachKiemTra { get; set; }
    }


    public class KeHoachKiemTraCreateRequestModel : KeHoachKiemTraModel
    {
        /// <summary>
        /// File soạn thảo / trình ký
        /// </summary>
        public List<FileKeHoachKiemTraModel> DataFileChoDuyet { get; set; } = new();

        /// <summary>
        /// File đã ban hành (nếu có)
        /// </summary>
        public List<FileKeHoachKiemTraModel> DataFileDaBanHanh { get; set; } = new();
    }


    public class KeHoachKiemTraUpdateRequestModel : KeHoachKiemTraModel
    {
        public List<FileKeHoachKiemTraModel> DataFileSoanThao { get; set; } = new();
        public List<FileKeHoachKiemTraModel> DataFileBanHanh { get; set; } = new();
    }

    public class KeHoachKiemTraDetailResponseModel : KeHoachKiemTraModel
    {
        public List<FileKeHoachKiemTraModel> DataFileChoDuyet { get; set; } = new List<FileKeHoachKiemTraModel>();
        public List<FileKeHoachKiemTraModel> DataFileDaDuyet { get; set; } = new List<FileKeHoachKiemTraModel>();
    }

    public class KeHoachKiemTraSearchModel : SearchBaseModel
    {
        public string? SoQuyetDinhBanHanh { get; set; }
        public string? IdDonVi { get; set; }
        public DateTime? NgayBanHanhKeHoachFrom { get; set; }
        public DateTime? NgayBanHanhKeHoachTo { get; set; }
        public string? CanCu { get; set; }
        public string? MucDich { get; set; }
        public string? YeuCau { get; set; }
        public DateTime? TuNgayThucHienKeHoach { get; set; }
        public DateTime? DenNgayThucHienKeHoach { get; set; }
    }

    public class KeHoachKiemTraSearchResultModel : KeHoachKiemTraModel
    {
        public int Index { get; set; } = 0;
        public string? DonVi { get; set; }
        public int? SoDoiTuong { get; set; }
        public int? SoVu { get; set; }
        public decimal? TongTienXuPhat { get; set; }
        public int? TrangThaiPhanGiao { get; set; }

    }

    public class KeHoachKiemTraBaseResultModel : SearchBaseResultModel<KeHoachKiemTraSearchResultModel>
    {

    }

    public class KeHoachKiemTraUpdateStatusModel : KeHoachKiemTraModel
    {
        public int? TrangThaiKeHoachKiemTra { get; set; }
    }

    public enum CapKeHoachEnum
    {
        Cuc = 1,
        Phong = 2,
        Don = 3
    }

    public enum TrangThaiKeHoachEnum
    {
        SoanThao = 1,
        TrinhDuyet = 2,
        DaBanHanh = 3,
        TiepNhan = 4,
        DangTrienKhai = 5,
        DangThucHien = 6,
        HoanThanh = 7,
        Ngung = 9
    }

    public enum LoaiFileKeHoachEnum
    {
        SoanThao = 1,
        TrinhKy = 2,
        BanHanh = 3,
        DieuChinh = 4
    }

}
