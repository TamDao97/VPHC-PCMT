using Microsoft.Extensions.DependencyInjection;
using NTS_ERP.Services.VPHC.BienBan;
using NTS_ERP.Services.VPHC.CanBo;
using NTS_ERP.Services.VPHC.ChungChiGiayPhep;
using NTS_ERP.Services.VPHC.KeHoachKiemTra;
using NTS_ERP.Services.VPHC.Nguoi;
using NTS_ERP.Services.VPHC.NguoiChungKien;
using NTS_ERP.Services.VPHC.NguoiVP;
using NTS_ERP.Services.VPHC.PhienDich;
using NTS_ERP.Services.VPHC.PhuongTien;
using NTS_ERP.Services.VPHC.QuyetDinh;
using NTS_ERP.Services.VPHC.TaiLieu;
using NTS_ERP.Services.VPHC.TangVat;
using NTS_ERP.Services.VPHC.ThongKe;
using NTS_ERP.Services.VPHC.ToChucVP;
using NTS_ERP.Services.VPHC.TraCuu;
using NTS_ERP.Services.VPHC.VuViecVPHC;
using NTS_ERP.Services.VPHC.XuHuong;

namespace NTS_ERP.Services.VPHC
{
    public static class VPHCServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesVPHC(this IServiceCollection services)
        {
            services.AddScoped<IVuViecService, VuViecService>();
            services.AddScoped<INguoiService, NguoiService>();
            services.AddScoped<INguoiVPService, NguoiVPService>();
            services.AddScoped<IToChucVPService, ToChucVPService>();

            services.AddScoped<ITangVatService, TangVatService>();
            services.AddScoped<IPhienDichService, PhienDichService>();
            services.AddScoped<INguoiChungKienService, NguoiChungKienService>();
            services.AddScoped<IChungChiGiayPhepService, ChungChiGiayPhepService>();
            services.AddScoped<IPhuongTienService, PhuongTienService>();
            services.AddScoped<ICanBoService, CanBoService>();
            services.AddScoped<IVuViecXacMinhService, VuViecXacMinhService>();
            services.AddScoped<IBienBanService, BienBanService>();
            services.AddScoped<IQuyetDinhService, QuyetDinhService>();
            services.AddScoped<IVuViecXuLyService, VuViecXuLyService>();
            services.AddScoped<ITaiLieuService, TaiLieuService>();
            services.AddScoped<ITraCuuNguoiVPService, TraCuuNguoiVPService>();
            services.AddScoped<ITrangChuService, TrangChuService>();
            services.AddScoped<IXuHuongService, XuHuongService>();
            services.AddScoped<IThongKeService, ThongKeService>();
            services.AddScoped<IKeHoachKiemTraService, KeHoachKiemTraService>();
            return services;
        }
    }
}
