using NTS_ERP.Models.Base;

namespace NTS_ERP.Models.Entities
{
    public class KeHoachKiemTra : BaseEntity
    {
        /// <summary>
        /// 1:Cục, 2:Phòng, 3:Đồn
        /// </summary>
        public int? CapKeHoach { get; set; }

        /// <summary>
        /// Id kế hoạch cha (tự tham chiếu)
        /// </summary>
        public string? IdKeHoachCha { get; set; }

        /// <summary>
        /// Đơn vị lập kế hoạch
        /// </summary>
        public string IdDonVi { get; set; }

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

}
