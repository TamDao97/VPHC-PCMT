using NTS_ERP.Services.Cores.Combobox;
using Microsoft.AspNetCore.Mvc;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using System;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.Combobox;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/combobox")]
    [ApiController]
    //[Authorize]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    public class ComboboxController : BaseApiController
    {
        private readonly IComboboxService comboboxService;

        public ComboboxController(IComboboxService comboboxService)
        {
            this.comboboxService = comboboxService;
        }

        /// <summary>
        /// Lấy danh sách combobox theo tên bảng
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-data/{table}")]
        public async Task<ActionResult<ApiResultModel>> GetAllTableInfo([FromRoute] string table)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await comboboxService.GetTableInfoAsync(table);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách nhóm danh mục
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-group-category")]
        public async Task<ActionResult<ApiResultModel>> GetAllGroupCategory()
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await comboboxService.GetAllGroupCategory();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách nhóm danh mục
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-group-user")]
        public async Task<ActionResult<ApiResultModel>> GetAllGroupUser()
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await comboboxService.GetAllGroupUser();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-user")]
        public async Task<ActionResult<ApiResultModel>> GetAllUser()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetAllUser();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }


        /// <summary>
        /// Danh sách menu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list-menu")]
        public async Task<ActionResult<ApiResultModel>> GetMenu()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetMenu();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách chức năng động
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-system-function-auto")]
        public async Task<ActionResult<ApiResultModel>> GetSystemFunctionConfig()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await comboboxService.GetSystemFunctionConfig();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-date-now-server")]
        public async Task<ActionResult> GetDateNowServer()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = DateTime.Now;
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-don-vi")]
        public ActionResult<ApiResultModel> GetDonVi()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetDonVi();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-don-vi-by-don-vi")]
        public ActionResult<ApiResultModel> GetDonViByIdDonVi()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  comboboxService.GetDonViByIdDonVi(CurrentUser.DonViId).Result;
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-tinh")]
        public ActionResult<ApiResultModel> GetAllTinh()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data =  comboboxService.GetAllTinh();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-huyen-by-tinh/{id}")]
        public ActionResult<ApiResultModel> GetHuyenByTinh([FromRoute]string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetHuyenByTinh(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("get-xa-by-huyen/{id}")]
        public ActionResult<ApiResultModel> GetXaByHuyen([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetXaByHuyen(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-gioi-tinh")]
        public ActionResult<ApiResultModel> GetAllGioiTinh()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetAllGioiTinh();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-dan-toc")]
        public ActionResult<ApiResultModel> GetAllDanToc()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetAllDanToc();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("get-quoc-tich")]
        public ActionResult<ApiResultModel> GetAllQuocTich()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetAllQuocTich();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("get-nghe-nghiep")]
        public ActionResult<ApiResultModel> GetAllNgheNghiep()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetAllNgheNghiep();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("get-ton-giao")]
        public ActionResult<ApiResultModel> GetAllTonGiao()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetAllTonGiao();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("get-nguon-tin")]
        public ActionResult<ApiResultModel> GetAllNguonTinVPHC()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetAllNguonTinVPHC();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-xu-ly-vphc/{thamquyen}")]
        public ActionResult<ApiResultModel> GetAllXuLyVPHC([FromRoute] string thamquyen)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetAllXuLyVPHC(thamquyen);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-phan-loai-tin")]
        public ActionResult<ApiResultModel> GetAllPhanLoaiTin()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetAllPhanLoaiTin();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("get-ket-luan-vphc")]
        public ActionResult<ApiResultModel> GetKetLuanVPHC()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetKetLuanVPHC();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-linh-vuc-bcth")]
        public ActionResult<ApiResultModel> GetLinhVucBCTH()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetLinhVucBCTH();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-loai-tang-vat")]
        public ActionResult<ApiResultModel> GetLoaiTangVat()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetLoaiTangVat();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-don-vi-tinh/{id}")]
        public ActionResult<ApiResultModel> GetLinhVucBCTH([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetDonViTinhByTangVat(id);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-loai-phuong-tien")]
        public ActionResult<ApiResultModel> GetLoaiPhuongTien()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetLoaiPhuongTien();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-hinh-thuc-phat-chinh")]
        public ActionResult<ApiResultModel> GetListHinhThucPhatChinh()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetListHinhThucPhat();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-hinh-thuc-phat-bo-sung")]
        public ActionResult<ApiResultModel> GetListHinhThucPhatBoSung()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetListHinhThucPhat("HTP003,HTP004,HTP005");
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-xu-ly-tang-vat")]
        public ActionResult<ApiResultModel> GetXuLyTangVatVPHC()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetXuLyTangVatVPHC();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-xu-ly-phuong-tien")]
        public ActionResult<ApiResultModel> GetXuLyPhuongTienVPHC()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetXuLyPhuongTienVPHC();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-xu-ly-giay-to")]
        public ActionResult<ApiResultModel> GetXuLyGiayToVPHC()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = comboboxService.GetXuLyGiayToVPHC();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
