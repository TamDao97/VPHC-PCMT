using NTS_ERP.Models.Cores.Combobox;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.Combobox
{
    public interface IComboboxService
    { 
        /// <summary>
       /// Lấy danh sách combobox theo tên bảng
       /// </summary>
       /// <param name="tableName"></param>
       /// <returns></returns>
        Task<List<ComboboxModel>> GetTableInfoAsync(string tableName);

        /// <summary>
        /// Danh sách giới tính
        /// </summary>
        /// <returns></returns>
        List<ComboboxIntegerModel> GetAllGioiTinh();

        /// <summary>
        /// Danh sách nhóm danh mục
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetAllGroupCategory();

        /// <summary>
        /// Lấy danh sách nhóm người dùng
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetAllGroupUser();

        /// <summary>
        /// Lấy danh sách người dùng
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetAllUser();

        /// <summary>
        /// Lấy danh sách menu
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetMenu();

        /// <summary>
        /// Danh sách chức năng động
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetSystemFunctionConfig();

        List<ComboboxModel> GetDonVi();
        List<ComboboxModel> GetAllTinh(bool? isBienGioi = null);

        List<ComboboxModel> GetHuyenByTinh(string idTinh, bool? isBienGioi = null);
        List<ComboboxModel> GetXaByHuyen(string idHuyen);
        List<ComboboxModel> GetAllDanToc();
        List<ComboboxModel> GetAllQuocTich();
        List<ComboboxModel> GetAllNgheNghiep();
        List<ComboboxModel> GetAllTonGiao();
        List<ComboboxModel> GetAllNguonTinVPHC();
        List<ComboboxModel> GetAllXuLyVPHC(string thamquyen = "1");
        List<ComboboxIntegerModel> GetAllPhanLoaiTin();
        List<ComboboxIntegerModel> GetKetLuanVPHC();
        List<ComboboxModel> GetLinhVucBCTH();

        List<ComboboxModel> GetLoaiTangVat();
        List<ComboboxModel> GetDonViTinhByTangVat(string id);
        List<ComboboxModel> GetLoaiPhuongTien();
        List<ComboboxModel> GetListHinhThucPhat(string filterIds = "");

        List<ComboboxIntegerModel> GetXuLyTangVatVPHC();
        List<ComboboxIntegerModel> GetXuLyPhuongTienVPHC();
        List<ComboboxIntegerModel> GetXuLyGiayToVPHC();

        Task<List<ComboboxModel>> GetDonViByIdDonVi(string idDonVi);
    }
}
