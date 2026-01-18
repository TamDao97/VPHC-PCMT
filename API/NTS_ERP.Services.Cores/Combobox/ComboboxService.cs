using NTS_ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.Combobox;

namespace NTS_ERP.Services.Cores.Combobox
{
    public class ComboboxService : IComboboxService
    {
        private NTS_ERPContext _sqlContext;

        public ComboboxService(NTS_ERPContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public List<ComboboxIntegerModel> GetAllGioiTinh()
        {
            ComboboxIntegerModel comboboxModel;
            List<ComboboxIntegerModel> listCombobox = new List<ComboboxIntegerModel>();
            comboboxModel = new ComboboxIntegerModel();
            comboboxModel.Id = 1;
            comboboxModel.Name = "Nam";
            listCombobox.Add(comboboxModel);

            comboboxModel = new ComboboxIntegerModel();
            comboboxModel.Id = 2;
            comboboxModel.Name = "Nữ";
            listCombobox.Add(comboboxModel);

            comboboxModel = new ComboboxIntegerModel();
            comboboxModel.Id = 3;
            comboboxModel.Name = "Khác";
            listCombobox.Add(comboboxModel);
            return listCombobox;
        }

        public async Task<List<ComboboxModel>> GetAllGroupCategory()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.Nts_GroupCategory.OrderBy(r => r.Index).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public async Task<List<ComboboxModel>> GetAllGroupUser()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.Nts_UserGroup.OrderBy(r => r.Name).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public async Task<List<ComboboxModel>> GetAllUser()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.Nts_User.OrderBy(r => r.UserName).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.UserName
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        public async Task<List<ComboboxModel>> GetMenu()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                var listAllMenu = _sqlContext.Nts_MenuSystem.OrderBy(r => r.Index).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.TitleDefault,
                    IdParent = s.ParentId
                }).ToList();


                var listParent = listAllMenu.Where(s => string.IsNullOrEmpty(s.IdParent)).ToList();
                var listAllSubMenu = listAllMenu.Where(s => !string.IsNullOrEmpty(s.IdParent)).ToList();
                foreach (var item in listParent)
                {
                    listCombobox.Add(item);
                    this.GetSubMenu(item.Id, listCombobox, listAllSubMenu);
                }

                return listCombobox;
            }
            catch { return listCombobox; }
        }

        private void GetSubMenu(string parentId, List<ComboboxModel> listMenu, List<ComboboxModel> listAllSubMenu)
        {
            var listSubMenu = listAllSubMenu.Where(s => parentId.Equals(s.IdParent)).ToList();
            foreach (var item in listSubMenu)
            {
                listMenu.Add(item);
                this.GetSubMenu(item.Id, listMenu, listAllSubMenu);
            }
        }

        /// <summary>
        /// Danh sách chức năng động
        /// </summary>
        public async Task<List<ComboboxModel>> GetSystemFunctionConfig()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.Nts_SystemFunctionConfig.OrderBy(r => r.FunctionName).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.FunctionName,
                    Code = s.Slug
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Lấy danh sách combobox theo tên bảng
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// 
        public async Task<List<ComboboxModel>> GetTableInfoAsync(string tableName)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                using (DbCommand cmd = _sqlContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [dbo].[" + tableName.NTSTrim() + "]";
                    await _sqlContext.Database.OpenConnectionAsync();
                    using (DbDataReader ddr = cmd.ExecuteReader())
                    {
                        while (ddr.Read())
                        {
                            listCombobox.Add(new ComboboxModel
                            {
                                Id = ddr["Id"].ToString(),
                                Name = ddr["Name"].ToString()
                            });
                        }
                    }
                }
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        public List<ComboboxModel> GetDonVi()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                var data = _sqlContext.DonVi.OrderBy(r => r.Order).Select(s => new ComboboxModel()
                {
                    Id = s.IdDonVi,
                    Name = s.Ten,
                    IdParent = s.IdDonViCha,
                    Level = s.Level
                }).ToList();

                ComboboxModel donViModel;
                foreach (var item in data)
                {
                    if (item.Name.Contains("Phòng PCMT&TP") || item.Name.Contains("Hải Đội"))
                    {
                        donViModel = data.Where(s => s.Id.Equals(item.IdParent)).FirstOrDefault();
                        if (donViModel != null)
                        {
                            item.Name += donViModel.Name.Replace("Bộ Chỉ huy BĐBP", " ");
                        }
                    }
                }

                var listParent = data.Where(i => string.IsNullOrEmpty(i.IdParent)).ToList();
                var listChild = data.Where(i => !string.IsNullOrEmpty(i.IdParent)).ToList();
                foreach (var item in listParent)
                {
                    listCombobox.Add(item);
                    listCombobox.AddRange(GetListChild(item.Id, listChild, string.Empty));
                }
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public List<ComboboxModel> GetListChild(string parentId, List<ComboboxModel> listGroupDocument, string convert)
        {
            List<ComboboxModel> listChild = new List<ComboboxModel>();
            var listChilds = listGroupDocument.Where(r => r.IdParent.Equals(parentId)).ToList();
            string convertchild = $"{convert} --";
            foreach (var item in listChilds)
            {
                item.IdParent = parentId;
                item.Name = convertchild + " " + item.Name;
                listChild.Add(item);
                listChild.AddRange(GetListChild(item.Id, listGroupDocument, convertchild));
            }

            return listChild;
        }

        public List<ComboboxModel> GetAllTinh(bool? isBienGioi = null)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.Province.Where(r => isBienGioi == null || r.Border == isBienGioi).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).OrderBy(o => o.Id).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public List<ComboboxModel> GetHuyenByTinh(string idTinh, bool? isBienGioi = null)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.District.Where(r => r.ProvinceId.Equals(idTinh)).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).OrderBy(o => o.Name).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public List<ComboboxModel> GetXaByHuyen(string idHuyen)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.Ward.Where(r => r.DistrictId.Equals(idHuyen)).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).OrderBy(o => o.Name).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public List<ComboboxModel> GetAllDanToc()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.DanToc.OrderBy(r => r.Order).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public List<ComboboxModel> GetAllQuocTich()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.QuocGia.OrderBy(r => r.Order).ThenBy(t => t.Name).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public List<ComboboxModel> GetAllNgheNghiep()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.NgheNghiep.OrderBy(r => r.Order).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        public List<ComboboxModel> GetAllTonGiao()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.TonGiao.OrderBy(r => r.Order).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        public List<ComboboxModel> GetAllNguonTinVPHC()
        {
            var result = (from a in _sqlContext.NguonTinVPHC.AsNoTracking()
                          orderby a.Order
                          select new ComboboxModel
                          {
                              Id = a.Id,
                              Name = a.Name
                          }).ToList();

            return result;
        }

        public List<ComboboxModel> GetAllXuLyVPHC(string thamquyen = "1")
        {
            var result = (from a in _sqlContext.XuLyVPHC.AsNoTracking()
                          orderby a.Order
                          select new ComboboxModel
                          {
                              Id = a.Id,
                              Name = a.Name
                          }).ToList();
            if (thamquyen.Equals("2"))
            {
                result = result.Where(r => r.Id.Equals("4")).ToList();
            }
            return result;
        }

        public List<ComboboxIntegerModel> GetAllPhanLoaiTin()
        {
            ComboboxIntegerModel comboboxModel;
            List<ComboboxIntegerModel> listCombobox = new List<ComboboxIntegerModel>();
            comboboxModel = new ComboboxIntegerModel();
            comboboxModel.Id = 1;
            comboboxModel.Name = "Thuộc thẩm quyền xử lý của BĐBP";
            listCombobox.Add(comboboxModel);

            comboboxModel = new ComboboxIntegerModel();
            comboboxModel.Id = 2;
            comboboxModel.Name = "Không thuộc thẩm quyền xử lý BĐBP";
            listCombobox.Add(comboboxModel);

            return listCombobox;
        }

        public List<ComboboxIntegerModel> GetKetLuanVPHC()
        {
            List<ComboboxIntegerModel> listCombobox = new List<ComboboxIntegerModel>
            {
                new ComboboxIntegerModel(){ Id = 1, Name = "Có hành vi vi phạm" },
                new ComboboxIntegerModel(){ Id = 2, Name = "Không có hành vi vi phạm" }
            };

            return listCombobox;
        }

        public List<ComboboxModel> GetLinhVucBCTH()
        {
            var result = (from a in _sqlContext.LinhVucThongKeBCTH.AsNoTracking()
                          orderby a.Order
                          select new ComboboxModel
                          {
                              Id = a.Id,
                              Name = a.Name,
                          }).ToList();

            return result;
        }

        public List<ComboboxModel> GetLoaiTangVat()
        {
            var result = (from a in _sqlContext.LoaiTangVat.AsNoTracking()
                          orderby a.Order
                          select new ComboboxModel
                          {
                              Id = a.Id,
                              Name = a.Name
                          }).ToList();

            return result;
        }

        public List<ComboboxModel> GetDonViTinhByTangVat(string id)
        {
            var result = (from a in _sqlContext.LoaiTangVatDVT.AsNoTracking()
                          where a.IdLoaiTangVat.Equals(id) || "FixOther".Equals(id)
                          join b in _sqlContext.DonViTinh.AsNoTracking() on a.IdDonViTinh equals b.IdDonViTinh
                          orderby b.Order
                          select new ComboboxModel
                          {
                              Id = b.IdDonViTinh,
                              Name = b.Ten
                          }).ToList();

            return result;
        }

        public List<ComboboxModel> GetLoaiPhuongTien()
        {
            var result = (from a in _sqlContext.LoaiPhuongTien.AsNoTracking()
                          orderby a.Order
                          select new ComboboxModel
                          {
                              Id = a.Id,
                              Name = a.Name
                          }).ToList();

            return result;
        }

        public List<ComboboxModel> GetListHinhThucPhat(string filterIds = "")
        {
            var result = (from a in _sqlContext.HinhThucPhat.AsNoTracking()
                          orderby a.Order
                          select new ComboboxModel
                          {
                              Id = a.Id,
                              Name = a.Name
                          }).ToList();
            if (!string.IsNullOrEmpty(filterIds))
            {
                result = result.Where(r => filterIds.Contains(r.Id.ToString())).ToList();
            }
            return result;
        }

        public List<ComboboxIntegerModel> GetXuLyTangVatVPHC()
        {
            List<ComboboxIntegerModel> listCombobox = new List<ComboboxIntegerModel>
            {
                new ComboboxIntegerModel(){ Id = 1, Name = "Tịch thu" },
                new ComboboxIntegerModel(){ Id = 2, Name = "Trả lại" },
                new ComboboxIntegerModel(){ Id = 3, Name = "Tiêu huỷ" },
                 new ComboboxIntegerModel(){ Id = 3, Name = "Chuyển giao" },
                new ComboboxIntegerModel(){ Id = 4, Name = "Hình thức khác" }
            };

            return listCombobox;
        }

        public List<ComboboxIntegerModel> GetXuLyPhuongTienVPHC()
        {
            List<ComboboxIntegerModel> listCombobox = new List<ComboboxIntegerModel>
            {
                new ComboboxIntegerModel(){ Id = 1, Name = "Tịch thu" },
                new ComboboxIntegerModel(){ Id = 2, Name = "Trả lại" },
                new ComboboxIntegerModel(){ Id = 3, Name = "Chuyển giao" },
                new ComboboxIntegerModel(){ Id = 3, Name = "Hình thức khác" }
            };

            return listCombobox;
        }

        public List<ComboboxIntegerModel> GetXuLyGiayToVPHC()
        {
            List<ComboboxIntegerModel> listCombobox = new List<ComboboxIntegerModel>
            {
                new ComboboxIntegerModel(){ Id = 1, Name = "Tạm giữ" },
                new ComboboxIntegerModel(){ Id = 2, Name = "Trả lại" },
                new ComboboxIntegerModel(){ Id = 3, Name = "Chuyển giao" },
                new ComboboxIntegerModel(){ Id = 3, Name = "Hình thức khác" }
            };

            return listCombobox;
        }

        public async Task<List<ComboboxModel>> GetDonViByIdDonVi(string idDonVi)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                if (GlobalData.ListAllDonVi == null || GlobalData.ListAllDonVi.Count == 0)
                    GlobalData.ListAllDonVi = _sqlContext.DonVi.ToList();

                var itemDonVi = GlobalData.ListAllDonVi.Where(a => a.IdDonVi.Equals(idDonVi)).FirstOrDefault();
                if (itemDonVi == null)
                    return listCombobox;
                //Nếu là Phòng của Cục thì lấy toàn đơn vị
                if (itemDonVi.Level == 2 && itemDonVi.IdLoaiDonVi == "DV01")
                {
                    itemDonVi = GlobalData.ListAllDonVi.Where(a => a.IdDonVi.Equals(itemDonVi.IdDonViCha)).FirstOrDefault();
                }
                //Nếu là Phòng ma túy của tỉnh thì lấy đơn vị của cả tỉnh
                else if (itemDonVi.Level == 3 && itemDonVi.IdLoaiDonVi == "DV03")
                {
                    itemDonVi = GlobalData.ListAllDonVi.Where(a => a.IdDonVi.Equals(itemDonVi.IdDonViCha)).FirstOrDefault();
                }


                List<ComboboxModel> listChild = GlobalData.ListAllDonVi
                .Select(s => new ComboboxModel()
                {
                    Id = s.IdDonVi,
                    Name = s.Ten,
                    IdParent = s.IdDonViCha,
                    Level = s.Level
                }).ToList();

                listCombobox.Add(new ComboboxModel()
                {
                    Id = itemDonVi.IdDonVi,
                    Name = itemDonVi.Ten,
                    IdParent = itemDonVi.IdDonViCha,
                    Level = itemDonVi.Level
                });
                listCombobox.AddRange(GetListChild(itemDonVi.IdDonVi, listChild, string.Empty));
                return listCombobox;
            }
            catch { return listCombobox; }

        }
    }
}
