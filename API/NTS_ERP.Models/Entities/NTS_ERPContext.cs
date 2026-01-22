using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NTS_ERP.Models.Entities;

public partial class NTS_ERPContext : DbContext
{
    public NTS_ERPContext()
    {
    }

    public NTS_ERPContext(DbContextOptions<NTS_ERPContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BienBanVPHC> BienBanVPHC { get; set; }

    public virtual DbSet<BienPhapKPHQ> BienPhapKPHQ { get; set; }

    public virtual DbSet<BienPhapNganChanHC> BienPhapNganChanHC { get; set; }

    public virtual DbSet<CanBo> CanBo { get; set; }

    public virtual DbSet<CapBac> CapBac { get; set; }

    public virtual DbSet<ChucVu> ChucVu { get; set; }

    public virtual DbSet<ChungChiGiayPhep> ChungChiGiayPhep { get; set; }

    public virtual DbSet<CucHaiQuan> CucHaiQuan { get; set; }

    public virtual DbSet<CucTrongBoCA> CucTrongBoCA { get; set; }

    public virtual DbSet<DanToc> DanToc { get; set; }

    public virtual DbSet<DanhMucBienBan> DanhMucBienBan { get; set; }

    public virtual DbSet<DanhMucQuyetDinh> DanhMucQuyetDinh { get; set; }

    public virtual DbSet<DanhMucTaiLieu> DanhMucTaiLieu { get; set; }

    public virtual DbSet<District> District { get; set; }

    public virtual DbSet<DonVi> DonVi { get; set; }

    public virtual DbSet<DonViPhoiHop> DonViPhoiHop { get; set; }

    public virtual DbSet<DonViTinh> DonViTinh { get; set; }

    public virtual DbSet<HinhThucPhat> HinhThucPhat { get; set; }

    public virtual DbSet<LinhVucThongKeBCTH> LinhVucThongKeBCTH { get; set; }

    public virtual DbSet<LinhVucViPham> LinhVucViPham { get; set; }

    public virtual DbSet<LoaiDonVi> LoaiDonVi { get; set; }

    public virtual DbSet<LoaiPhuongTien> LoaiPhuongTien { get; set; }

    public virtual DbSet<LoaiTangVat> LoaiTangVat { get; set; }

    public virtual DbSet<LoaiTangVatDVT> LoaiTangVatDVT { get; set; }

    public virtual DbSet<LoaiVanBan> LoaiVanBan { get; set; }

    public virtual DbSet<NgheNghiep> NgheNghiep { get; set; }

    public virtual DbSet<Nguoi> Nguoi { get; set; }

    public virtual DbSet<NguoiChungKien> NguoiChungKien { get; set; }

    public virtual DbSet<NguoiGiamHoVPHC> NguoiGiamHoVPHC { get; set; }

    public virtual DbSet<NguoiVPHC> NguoiVPHC { get; set; }

    public virtual DbSet<NguonTinVPHC> NguonTinVPHC { get; set; }

    public virtual DbSet<Nts_About> Nts_About { get; set; }

    public virtual DbSet<Nts_Category> Nts_Category { get; set; }

    public virtual DbSet<Nts_FileTemplate> Nts_FileTemplate { get; set; }

    public virtual DbSet<Nts_Function> Nts_Function { get; set; }

    public virtual DbSet<Nts_FunctionAuto> Nts_FunctionAuto { get; set; }

    public virtual DbSet<Nts_GenerateCode> Nts_GenerateCode { get; set; }

    public virtual DbSet<Nts_GroupCategory> Nts_GroupCategory { get; set; }

    public virtual DbSet<Nts_MenuSystem> Nts_MenuSystem { get; set; }

    public virtual DbSet<Nts_MenuSystemPermission> Nts_MenuSystemPermission { get; set; }

    public virtual DbSet<Nts_RefreshToken> Nts_RefreshToken { get; set; }

    public virtual DbSet<Nts_SystemConfig> Nts_SystemConfig { get; set; }

    public virtual DbSet<Nts_SystemFunctionConfig> Nts_SystemFunctionConfig { get; set; }

    public virtual DbSet<Nts_SystemFunctionDesign> Nts_SystemFunctionDesign { get; set; }

    public virtual DbSet<Nts_SystemParamGroups> Nts_SystemParamGroups { get; set; }

    public virtual DbSet<Nts_SystemParams> Nts_SystemParams { get; set; }

    public virtual DbSet<Nts_User> Nts_User { get; set; }

    public virtual DbSet<Nts_UserGroup> Nts_UserGroup { get; set; }

    public virtual DbSet<Nts_UserGroupFunction> Nts_UserGroupFunction { get; set; }

    public virtual DbSet<Nts_UserHistory> Nts_UserHistory { get; set; }

    public virtual DbSet<Nts_UserPermission> Nts_UserPermission { get; set; }

    public virtual DbSet<PhatBoSungVPHC> PhatBoSungVPHC { get; set; }

    public virtual DbSet<PhienDichVienVPHC> PhienDichVienVPHC { get; set; }

    public virtual DbSet<PhongTrongCATinh> PhongTrongCATinh { get; set; }

    public virtual DbSet<PhuongTienVPHC> PhuongTienVPHC { get; set; }

    public virtual DbSet<Province> Province { get; set; }

    public virtual DbSet<QuocGia> QuocGia { get; set; }

    public virtual DbSet<QuyetDinhXuPhat> QuyetDinhXuPhat { get; set; }

    public virtual DbSet<TaiLieu> TaiLieu { get; set; }

    public virtual DbSet<TangVatVPHC> TangVatVPHC { get; set; }

    public virtual DbSet<ThamQuyenXPVPHC> ThamQuyenXPVPHC { get; set; }

    public virtual DbSet<ToChucVP> ToChucVP { get; set; }

    public virtual DbSet<TonGiao> TonGiao { get; set; }

    public virtual DbSet<VanBanQPPL> VanBanQPPL { get; set; }

    public virtual DbSet<VuViecChangeLog> VuViecChangeLog { get; set; }

    public virtual DbSet<VuViecVPHC> VuViecVPHC { get; set; }

    public virtual DbSet<Ward> Ward { get; set; }

    public virtual DbSet<XuLyVPHC> XuLyVPHC { get; set; }

    public virtual DbSet<XuPhatNguoiVPHC> XuPhatNguoiVPHC { get; set; }

    public virtual DbSet<XuPhatToChucVPHC> XuPhatToChucVPHC { get; set; }
    public virtual DbSet<KeHoachKiemTra> KeHoachKiemTra { get; set; }
    public virtual DbSet<FileKeHoachKiemTra> FileKeHoachKiemTra { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=14.248.84.128,1445;Database=VPHC;User Id=sa;Password=123@sa#;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BienBanVPHC>(entity =>
        {
            entity.HasKey(e => e.IdBienBan);

            entity.Property(e => e.IdBienBan)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.BienPhapNganChan).HasMaxLength(1500);
            entity.Property(e => e.CanCu).HasMaxLength(1500);
            entity.Property(e => e.CoQuanGiaiQuyet).HasMaxLength(1500);
            entity.Property(e => e.CoQuanGiaiTrinh).HasMaxLength(1500);
            entity.Property(e => e.DiaDiemLap).HasMaxLength(500);
            entity.Property(e => e.HanhViViPham).HasMaxLength(1500);
            entity.Property(e => e.IdCanBoLap)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdDanhMucBienBan)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdNguoiChungKien)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdNguoiViPham)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdPhienDichVien)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdToChucViPham)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdVuViec)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.LinhVuc).HasMaxLength(1500);
            entity.Property(e => e.LyDoChungKienKhongKy).HasMaxLength(1500);
            entity.Property(e => e.LyDoLapChoKhac).HasMaxLength(1500);
            entity.Property(e => e.LyDoViPhamKhongKy).HasMaxLength(1500);
            entity.Property(e => e.QuyDinhTai).HasMaxLength(1500);
            entity.Property(e => e.So).HasMaxLength(100);
            entity.Property(e => e.ThietHai).HasMaxLength(1500);
            entity.Property(e => e.ThoiGianGiaiQuyet).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianLap).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianLapXong).HasColumnType("datetime");
            entity.Property(e => e.YKienBenThietHai).HasMaxLength(1500);
            entity.Property(e => e.YKienNguoiChungKien).HasMaxLength(1500);
            entity.Property(e => e.YKienViPham).HasMaxLength(1500);
        });

        modelBuilder.Entity<BienPhapKPHQ>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BienPhap__3214EC0741DC42D6");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<BienPhapNganChanHC>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CanBo>(entity =>
        {
            entity.HasKey(e => e.IdCanBo);

            entity.Property(e => e.IdCanBo)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.DiaChiDayDu).HasMaxLength(1000);
            entity.Property(e => e.DiaChiHienNayDayDu).HasMaxLength(1000);
            entity.Property(e => e.HoVaTen).HasMaxLength(100);
            entity.Property(e => e.IdCapBac)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdChucVu)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdDanToc)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdDonVi)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdHuyen)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdHuyenHienNay)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdTinh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdTinhHienNay)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdTonGiao)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdXa)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdXaHienNay)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LyLuanChinhTri)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.NgayCapCNDN).HasColumnType("datetime");
            entity.Property(e => e.NgayNhapNgu).HasColumnType("datetime");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.NgayVaoDang).HasColumnType("datetime");
            entity.Property(e => e.SoCMQD).HasMaxLength(100);
            entity.Property(e => e.SoCNDN)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ThoiGianNghiHuu).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianRaPCMT).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianVaoPCMT).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianXuatNgu).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CapBac>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CapBac__3214EC07E6CFF8D8");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChucVu__3214EC07DF1AA39B");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ChungChiGiayPhep>(entity =>
        {
            entity.HasKey(e => e.IdChungChiGiayPhep);

            entity.Property(e => e.IdChungChiGiayPhep)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Ten).HasMaxLength(300);
            entity.Property(e => e.XuLy).HasDefaultValueSql("('0')");
        });

        modelBuilder.Entity<CucHaiQuan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CucHaiQu__3214EC07055750E1");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CucTrongBoCA>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CucTrong__3214EC074895263B");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DanToc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DanToc__3214EC07B71A9D4B");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DanhMucBienBan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Table_1");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<DanhMucQuyetDinh>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<DanhMucTaiLieu>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ProvinceId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<DonVi>(entity =>
        {
            entity.HasKey(e => e.IdDonVi).HasName("PK__DonVi__F27207FDC9558C3C");

            entity.Property(e => e.IdDonVi)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.IdDonViCha)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdHuyen)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdLoaiDonVi)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdTinh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdXa)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SoDienThoai).HasMaxLength(100);
            entity.Property(e => e.Ten).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DonViPhoiHop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonViPho__3214EC07EE5C4E44");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DonViTinh>(entity =>
        {
            entity.HasKey(e => e.IdDonViTinh).HasName("PK__DonViTin__B375F503AFFBD0DB");

            entity.Property(e => e.IdDonViTinh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IdDonViCoBan)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Ten).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HinhThucPhat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HinhThuc__3214EC078E54D49A");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LinhVucThongKeBCTH>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.ShortName).HasMaxLength(500);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LinhVucViPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LinhVucV__3214EC0797C28CFD");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LoaiDonVi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_KieuDonVi");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(300);
        });

        modelBuilder.Entity<LoaiPhuongTien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiPhuo__3214EC07BD9F3070");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LoaiTangVat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiTang__3214EC07EC8E7DAE");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LoaiTangVatDVT>(entity =>
        {
            entity.HasKey(e => e.IdLoaiTangVatDVT).HasName("PK__LoaiTang__97018A3375DAF72D");

            entity.Property(e => e.IdLoaiTangVatDVT)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdDonViTinh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdLoaiTangVat)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoaiVanBan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiVanB__3214EC07A944DE1A");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<NgheNghiep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NgheNghi__3214EC07000ABA77");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nguoi>(entity =>
        {
            entity.HasKey(e => e.IdNguoi).HasName("PK__Nguoi__57C4DE857E0803BC");

            entity.Property(e => e.IdNguoi)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CMND)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.DiaChiDayDu).HasMaxLength(1000);
            entity.Property(e => e.DiaChiHienNayDayDu).HasMaxLength(1000);
            entity.Property(e => e.HoVaTen).HasMaxLength(100);
            entity.Property(e => e.IdDanToc)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdDonVi)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdHuyen)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdHuyenHienNay)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdNgheNghiep).HasMaxLength(36);
            entity.Property(e => e.IdQuocTich)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdTinh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdTinhHienNay)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdTonGiao)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdXa)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdXaHienNay)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NgayCap).HasColumnType("datetime");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.NoiCap).HasMaxLength(150);
            entity.Property(e => e.SoDienThoai).HasMaxLength(100);
            entity.Property(e => e.TrinhDoVanHoa).HasMaxLength(500);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<NguoiChungKien>(entity =>
        {
            entity.HasKey(e => e.IdNguoiChungKien);

            entity.Property(e => e.IdNguoiChungKien)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CMND)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.HoVaTen).HasMaxLength(100);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.NgayCap).HasColumnType("datetime");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.NgheNghiep).HasMaxLength(500);
            entity.Property(e => e.NoiCap).HasMaxLength(500);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NguoiGiamHoVPHC>(entity =>
        {
            entity.HasKey(e => e.IdNguoiGiamHoVPHC).HasName("PK__NguoiGia__EB5B139D7088557A");

            entity.Property(e => e.IdNguoiGiamHoVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CMND)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.HoVaTen).HasMaxLength(100);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.NgayCap).HasColumnType("datetime");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.NoiCap).HasMaxLength(500);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NguoiVPHC>(entity =>
        {
            entity.HasKey(e => e.IdNguoiVPHC).HasName("PK__NguoiVPH__5B61F38638270E79");

            entity.Property(e => e.IdNguoiVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.DiaChiGH).HasMaxLength(500);
            entity.Property(e => e.HanhViViPham).HasMaxLength(1000);
            entity.Property(e => e.HoVaTenGH).HasMaxLength(100);
            entity.Property(e => e.IdNguoi)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.KetLuanKiemTra).HasDefaultValueSql("('0')");
            entity.Property(e => e.QuanHeGH).HasMaxLength(150);
        });

        modelBuilder.Entity<NguonTinVPHC>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nts_About>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Abou__3214EC07CFD4EB88");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(500);
        });

        modelBuilder.Entity<Nts_Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Cate__3214EC071217D738");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.GroupCategoryId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.TableName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nts_FileTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_File__3214EC07ABFD0601");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DefaultFilePath).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nts_Function>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Func__3214EC0748617AE5");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<Nts_FunctionAuto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Func__3214EC07656253E0");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<Nts_GenerateCode>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MaDuAn)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Namespace)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Sql_Database)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Sql_Password)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Sql_Server)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Sql_User)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TenDuAn).HasMaxLength(500);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nts_GroupCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Grou__3214EC0711D076E9");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<Nts_MenuSystem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Menu__3214EC07A8490C4C");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Icon).HasMaxLength(300);
            entity.Property(e => e.ParentId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.SystemFunctionConfigId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.TitleDefault).HasMaxLength(300);
            entity.Property(e => e.TitleKeyTranslate).HasMaxLength(300);
            entity.Property(e => e.Url).HasMaxLength(500);
        });

        modelBuilder.Entity<Nts_MenuSystemPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Menu__3214EC078AC22D60");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.FunctionId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.MenuSystemId)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nts_RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Refr__3214EC07CAAAF4E6");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.IssueAt).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nts_SystemConfig>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.FaviconIcon).HasMaxLength(255);
            entity.Property(e => e.Logo).HasMaxLength(255);
            entity.Property(e => e.LogoFolded).HasMaxLength(225);
            entity.Property(e => e.SoftwareName).HasMaxLength(500);
        });

        modelBuilder.Entity<Nts_SystemFunctionConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Syst__3214EC072C6DEC19");

            entity.HasIndex(e => e.TableName, "UQ__Nts_Syst__733652EEB6501AE7").IsUnique();

            entity.HasIndex(e => e.Slug, "UQ__Nts_Syst__BC7B5FB6530CA2B5").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreatePermission).HasMaxLength(100);
            entity.Property(e => e.CreateWindowType).HasDefaultValue(1);
            entity.Property(e => e.CreateWindowWidth).HasMaxLength(500);
            entity.Property(e => e.DataColumnStart).HasDefaultValue(0);
            entity.Property(e => e.DataRowStart).HasDefaultValue(0);
            entity.Property(e => e.DeleteDisplay).HasDefaultValue(false);
            entity.Property(e => e.DeletePermission).HasMaxLength(100);
            entity.Property(e => e.DetailPermission).HasMaxLength(100);
            entity.Property(e => e.DetailWindowType).HasDefaultValue(1);
            entity.Property(e => e.DetailWindowWidth).HasMaxLength(500);
            entity.Property(e => e.EditPermission).HasMaxLength(100);
            entity.Property(e => e.EditWindowType).HasDefaultValue(1);
            entity.Property(e => e.EditWindowWidth).HasMaxLength(500);
            entity.Property(e => e.ExportPermission).HasMaxLength(100);
            entity.Property(e => e.FunctionGroup).HasMaxLength(150);
            entity.Property(e => e.FunctionName).HasMaxLength(500);
            entity.Property(e => e.ImportPermission).HasMaxLength(100);
            entity.Property(e => e.LayoutType).HasDefaultValue(1);
            entity.Property(e => e.LinkTemplate).HasMaxLength(255);
            entity.Property(e => e.ModuleName).HasMaxLength(150);
            entity.Property(e => e.SearchPermission).HasMaxLength(100);
            entity.Property(e => e.Slug).HasMaxLength(150);
            entity.Property(e => e.TableName).HasMaxLength(150);
            entity.Property(e => e.TreeColumnId).HasMaxLength(150);
            entity.Property(e => e.TreeColumnParentId).HasMaxLength(150);
            entity.Property(e => e.TreeColumnsText).HasMaxLength(500);
            entity.Property(e => e.TreeFunctionConfigId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.TreeName).HasMaxLength(500);
            entity.Property(e => e.TreeTableName).HasMaxLength(150);
        });

        modelBuilder.Entity<Nts_SystemFunctionDesign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Syst__3214EC073754DB3E");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.ColumnName).HasMaxLength(150);
            entity.Property(e => e.DataType).HasMaxLength(150);
            entity.Property(e => e.DisplayName).HasMaxLength(500);
            entity.Property(e => e.DivCreateWidth).HasMaxLength(500);
            entity.Property(e => e.DivDetailWidth).HasMaxLength(500);
            entity.Property(e => e.DivEditWidth).HasMaxLength(500);
            entity.Property(e => e.LinkId).HasMaxLength(150);
            entity.Property(e => e.LinkName).HasMaxLength(150);
            entity.Property(e => e.LinkOrder).HasMaxLength(150);
            entity.Property(e => e.LinkTable).HasMaxLength(150);
            entity.Property(e => e.SystemFunctionConfigId)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nts_SystemParamGroups>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Syst__3214EC07B9119B86");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(300);
        });

        modelBuilder.Entity<Nts_SystemParams>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_Syst__3214EC07A473829A");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.DisplayName).HasMaxLength(300);
            entity.Property(e => e.ParamName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.SystemParamGroupId)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nts_User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_User__3214EC07072BC4B9");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Avatar).HasMaxLength(500);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FullName).HasMaxLength(500);
            entity.Property(e => e.IdDonVi)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserGroupId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<Nts_UserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_User__3214EC07BAF6FB15");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nts_UserGroupFunction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_User__3214EC07E7918CC8");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.FunctionId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UserGroupId)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nts_UserHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_User__3214EC0774416C09");

            entity.Property(e => e.BrowserName).HasMaxLength(150);
            entity.Property(e => e.BrowserVersion).HasMaxLength(150);
            entity.Property(e => e.ClientIP).HasMaxLength(100);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Device).HasMaxLength(150);
            entity.Property(e => e.OS).HasMaxLength(500);
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nts_UserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Nts_User__3214EC072665BA6F");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.FunctionId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhatBoSungVPHC>(entity =>
        {
            entity.HasKey(e => e.IdPhatBoSungVPHC).HasName("PK__PhatBoSu__7C9C3FE64218BF31");

            entity.Property(e => e.IdPhatBoSungVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdHinhThucPhat)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdObject)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhienDichVienVPHC>(entity =>
        {
            entity.HasKey(e => e.IdPhienDichVienVPHC).HasName("PK__PhienDic__62C03459BCE3EB1A");

            entity.Property(e => e.IdPhienDichVienVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CMND)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.HoVaTen).HasMaxLength(100);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.NgayCap).HasColumnType("datetime");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.NgheNghiep).HasMaxLength(500);
            entity.Property(e => e.NoiCap).HasMaxLength(500);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhongTrongCATinh>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhongTro__3214EC07BF04E6BC");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PhuongTienVPHC>(entity =>
        {
            entity.HasKey(e => e.IdPhuongTienVPHC).HasName("PK__PhuongTi__454CBFE4A5333596");

            entity.Property(e => e.IdPhuongTienVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.BienSo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdLoaiPhuongTien)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.NhanHieu).HasMaxLength(300);
            entity.Property(e => e.XuLy).HasDefaultValueSql("('0')");
            entity.Property(e => e.XuatXu).HasMaxLength(300);

            entity.HasOne(d => d.IdLoaiPhuongTienNavigation).WithMany(p => p.PhuongTienVPHC)
                .HasForeignKey(d => d.IdLoaiPhuongTien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhuongTie__IdLoa__50FB042B");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HCKey)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<QuocGia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuocGia__3214EC07784D3868");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<QuyetDinhXuPhat>(entity =>
        {
            entity.HasKey(e => e.IdQuyetDinh);

            entity.Property(e => e.IdQuyetDinh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CanCu).HasMaxLength(1500);
            entity.Property(e => e.ChiPhiKPHQ).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ChiPhiKPHQText).HasMaxLength(1500);
            entity.Property(e => e.CoQuanThucHienKPHC)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CuTheKPHQ).HasMaxLength(1500);
            entity.Property(e => e.CuThePBS).HasMaxLength(1500);
            entity.Property(e => e.CuThePC).HasMaxLength(1500);
            entity.Property(e => e.DiaDiemNopPhat).HasMaxLength(1500);
            entity.Property(e => e.DonViPhoiHop).HasMaxLength(1500);
            entity.Property(e => e.DonViThuTienPhat).HasMaxLength(1500);
            entity.Property(e => e.DonViThucHien).HasMaxLength(1500);
            entity.Property(e => e.HanhViViPham).HasMaxLength(1500);
            entity.Property(e => e.IdDanhMucQuyetDinh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdNguoiViPham)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdToChucViPham)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdVuViec)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.KhacPhucHauQua)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MucPhat).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MucPhatText).HasMaxLength(1500);
            entity.Property(e => e.NgayQDCoHieuLuc).HasColumnType("datetime");
            entity.Property(e => e.NgayRaQD).HasColumnType("datetime");
            entity.Property(e => e.NoiDungLienQuanKPHQ).HasMaxLength(1500);
            entity.Property(e => e.PhatBoSung)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.PhatChinh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.QuyDinhTai).HasMaxLength(1500);
            entity.Property(e => e.So).HasMaxLength(100);
            entity.Property(e => e.TinhTietGiamNhe).HasMaxLength(1500);
            entity.Property(e => e.TinhTietTangNang).HasMaxLength(1500);
        });

        modelBuilder.Entity<TaiLieu>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Extention)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FileName).HasMaxLength(300);
            entity.Property(e => e.FilePath).HasMaxLength(300);
            entity.Property(e => e.FileSize).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IdCategory)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdVuViec)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Thumbnail).HasMaxLength(300);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TangVatVPHC>(entity =>
        {
            entity.HasKey(e => e.IdTangVatVPHC).HasName("PK__TangVatV__7E214021E140DDE1");

            entity.Property(e => e.IdTangVatVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.ChungLoai).HasMaxLength(300);
            entity.Property(e => e.IdDonViTinh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdDonViTinhThuc)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdLoaiTangVat)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.SoLuongThuc).HasDefaultValueSql("('0')");
            entity.Property(e => e.XuLy).HasDefaultValueSql("('0')");
        });

        modelBuilder.Entity<ThamQuyenXPVPHC>(entity =>
        {
            entity.HasKey(e => e.IdThamQuyenXPVPHC).HasName("PK__ThamQuye__A21225D784EF84C9");

            entity.Property(e => e.IdThamQuyenXPVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.TenThamQuyen).HasMaxLength(300);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ToChucVP>(entity =>
        {
            entity.HasKey(e => e.IdToChucVP).HasName("PK__ToChucVP__98143832E93E7973");

            entity.Property(e => e.IdToChucVP)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.ChucVu).HasMaxLength(150);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DiaChiTruSo).HasMaxLength(500);
            entity.Property(e => e.HanhViViPham).HasMaxLength(1000);
            entity.Property(e => e.HoTenPhapNhan).HasMaxLength(150);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.KetLuanKiemTra).HasDefaultValueSql("('0')");
            entity.Property(e => e.MaSoDoanhNghiep).HasMaxLength(100);
            entity.Property(e => e.NgayCapDKKD).HasColumnType("datetime");
            entity.Property(e => e.NoiCapDKKD).HasMaxLength(500);
            entity.Property(e => e.SoDKKD).HasMaxLength(100);
            entity.Property(e => e.Ten).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TonGiao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TonGiao__3214EC07DF279540");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VanBanQPPL>(entity =>
        {
            entity.HasKey(e => e.IdVanBanQPPL).HasName("PK__VanBanQP__08CFE359F6EF5F19");

            entity.Property(e => e.IdVanBanQPPL)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DonViNhapLieu).HasMaxLength(100);
            entity.Property(e => e.IdCoQuanBanHanh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdLinhVuc)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdLoaiVanBan)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.So).HasMaxLength(100);
            entity.Property(e => e.ThoiGianBanHanh).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLoaiVanBanNavigation).WithMany(p => p.VanBanQPPL)
                .HasForeignKey(d => d.IdLoaiVanBan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VanBanQPP__IdLoa__07E124C1");
        });

        modelBuilder.Entity<VuViecChangeLog>(entity =>
        {
            entity.Property(e => e.HistoryLogId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.ObjectId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy).HasMaxLength(500);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<VuViecVPHC>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VuViecVP__3214EC07CF58A1F8");

            entity.HasIndex(e => e.MaHoSo, "UQ__VuViecVP__1666423D6BF8451F").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DiaChiChiTiet).HasMaxLength(500);
            entity.Property(e => e.DiaChiDayDu).HasMaxLength(1500);
            entity.Property(e => e.DonViKhacXuLy).HasMaxLength(500);
            entity.Property(e => e.DonViTiepNhanHS).HasMaxLength(500);
            entity.Property(e => e.IdDonVi)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdHuyenPhatHien)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdLinhVucBCTH)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdNguonPhatHien)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdTinhPhatHien)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdXaPhatHien)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IdXuLy)
                .HasMaxLength(36)
                .HasDefaultValueSql("((0))");
            entity.Property(e => e.MaHoSo)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.NgayBanGiaoDVKhac).HasColumnType("datetime");
            entity.Property(e => e.NgayBanGiaoHS).HasColumnType("datetime");
            entity.Property(e => e.NgayUyQuyen).HasColumnType("datetime");
            entity.Property(e => e.NguoiUyQuyen).HasMaxLength(500);
            entity.Property(e => e.SoBienBanDVKhac).HasMaxLength(150);
            entity.Property(e => e.SoBienBanHS).HasMaxLength(150);
            entity.Property(e => e.SoQDUyQuyen).HasMaxLength(100);
            entity.Property(e => e.ThoiGianTiepNhan).HasColumnType("datetime");
            entity.Property(e => e.TongTienPhat).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DistrictId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<XuLyVPHC>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<XuPhatNguoiVPHC>(entity =>
        {
            entity.HasKey(e => e.IdXuPhatNguoiVPHC).HasName("PK__XuPhatNg__B4614AD778389A97");

            entity.Property(e => e.IdXuPhatNguoiVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DieuKhoanDiem).HasMaxLength(500);
            entity.Property(e => e.IdHinhThucPhat)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdLinhVucViPham)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdNghiDinh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdNguoiVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdThamQuyenXPVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.MucPhat).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.NgayQuyetDinh).HasColumnType("datetime");
            entity.Property(e => e.SoQuyetDinh).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<XuPhatToChucVPHC>(entity =>
        {
            entity.HasKey(e => e.IdXuPhatToChucVPHC).HasName("PK__XuPhatTo__B757B3271007CAEE");

            entity.Property(e => e.IdXuPhatToChucVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DieuKhoanDiem).HasMaxLength(500);
            entity.Property(e => e.IdHinhThucPhat)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdLinhVucViPham)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdNghiDinh)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdThamQuyenXPVPHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdToChucVP)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.IdViPhamHC)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.MucPhat).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.NgayQuyetDinh).HasColumnType("datetime");
            entity.Property(e => e.SoQuyetDinh).HasMaxLength(100);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
