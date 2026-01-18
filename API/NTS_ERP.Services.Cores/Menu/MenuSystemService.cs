using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Redis;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.Function;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.Cores.Menu;
using NTS_ERP.Models.Entities;
using NTS_ERP.Services.Cores.GroupUsers;
using TrafficControl.Core;

namespace NTS_ERP.Services.Cores.Menu
{
    public class MenuSystemService : IMenuSystemService
    {
        private readonly RedisCacheService _redisCacheService;
        private readonly NTS_ERPContext _sqlContext;
        private readonly RedisCacheSettingModel _redisCacheSettings;
        private readonly IGroupUserService groupUser;
        public MenuSystemService(NTS_ERPContext sqlContext, RedisCacheService redisCacheService, IOptions<RedisCacheSettingModel> options, IGroupUserService groupUserService)
        {
            this._sqlContext = sqlContext;
            _redisCacheService = redisCacheService;
            _redisCacheSettings = options.Value;
            groupUser = groupUserService;
        }

        /// <summary>
        /// search menu
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<MenuViewModel>> SearchMenuAsync(MenuSearchModel searchModel)
        {
            SearchBaseResultModel<MenuViewModel> searchResult = new SearchBaseResultModel<MenuViewModel>();
            var dataQuery = (from a in _sqlContext.Nts_MenuSystem.AsNoTracking()
                             orderby a.Index
                             select new MenuViewModel
                             {
                                 Id = a.Id,
                                 TitleKeyTranslate = a.TitleKeyTranslate,
                                 TitleDefault = a.TitleDefault,
                                 Icon = a.Icon,
                                 Url = a.Url,
                                 ParentId = a.ParentId,
                                 IsDisable = a.IsDisable,
                                 Index = a.Index,
                                 IsDefaultMenu = a.IsDefaultMenu,
                                 FunctionAuto = a.FunctionAuto
                             }).OrderBy(e => e.Index).Distinct().AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.TitleDefault))
            {
                dataQuery = dataQuery.Where(u => u.TitleDefault.ToUpper().Contains(searchModel.TitleDefault.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.TitleKeyTranslate))
            {
                dataQuery = dataQuery.Where(u => u.TitleKeyTranslate.ToUpper().Contains(searchModel.TitleKeyTranslate.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Url))
            {
                dataQuery = dataQuery.Where(u => u.Url.ToUpper().Contains(searchModel.Url.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Icon))
            {
                dataQuery = dataQuery.Where(u => u.Icon.ToUpper().Contains(searchModel.Icon.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);

            }
            var nemus = dataQuery.ToList();
            var _parents = nemus.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
            foreach (var menu in _parents)
            {
                menu.Children = GetSubMenu(menu, nemus).OrderBy(e => e.Index).ToList();
                if (menu.Children.Count > 0)
                {
                    menu.Type = "collapsable";
                }
                else
                {
                    menu.Type = "item";
                }
            }
            var result = _parents.OrderBy(e => e.Index).ToList();
            searchResult.TotalItems = result.Count();
            var listResult = result.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// lấy danh sách menu cho left bar
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuViewModel>> GetMenuByUserAsync(string userId)
        {
            List<MenuViewModel> result = new List<MenuViewModel>(); // Get from Cachimng
            string key = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixMenuKey}{userId}";

            result = await _redisCacheService.GetAsync<List<MenuViewModel>>(key);

            if (result == null || result.Count == 0)
            {
                var menus = (from a in _sqlContext.Nts_MenuSystem.AsNoTracking()
                             orderby a.Index
                             select new MenuViewModel
                             {
                                 Id = a.Id,
                                 TitleDefault = a.TitleDefault,
                                 TitleKeyTranslate = string.IsNullOrEmpty(a.TitleKeyTranslate) ? a.TitleDefault : a.TitleKeyTranslate,
                                 Icon = a.Icon,
                                 Url = a.Url,
                                 ParentId = a.ParentId,
                                 IsDisable = a.IsDisable,
                                 Index = a.Index,
                                 IsDefaultMenu = a.IsDefaultMenu
                             }).OrderBy(e => e.Index).Distinct().ToList();

                //Danh sách menu được phân cho người dùng
                List<string> menuPermissions;

                //Nếu không phải tài khoản root thì chỉ lấy các menu hiện
                if (!userId.Equals(NTSConstants.IdUserRootFix))
                {
                    menus = menus.Where(s => !s.IsDisable).ToList();
                    menuPermissions = (from a in _sqlContext.Nts_MenuSystemPermission.AsNoTracking()
                                       join b in _sqlContext.Nts_UserPermission.Where(e => userId.Equals(e.UserId)).AsNoTracking() on a.FunctionId equals b.FunctionId
                                       select a.MenuSystemId).Distinct().ToList();
                }
                else
                {
                    menuPermissions = menus.Select(s => s.Id).ToList();
                }

                var stringMenuPermissions = string.Join(";", menuPermissions);

                var _parents = menus.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                foreach (var menu in _parents)
                {
                    if (stringMenuPermissions.Contains(menu.Id) || menu.IsDefaultMenu)
                    {
                        menu.Match = 1;
                    }
                    menu.Children = GetSubMenu(menu, menus, stringMenuPermissions).OrderBy(e => e.Index).ToList();
                    if (menu.Children.Count > 0)
                    {
                        menu.Type = "collapsable";
                    }
                    else
                    {
                        menu.Type = "item";
                    }
                }

                return _parents.Where(o => o.Match > 0).OrderBy(e => e.Index).ToList();

                 _redisCacheService.Remove(key);
                _redisCacheService.Add<List<MenuViewModel>>(key, result, new TimeSpan(1, 0, 0, 0));
            }

            return result;
        }

        /// <summary>
        /// lấy con cho nemu chính
        /// </summary>
        /// <returns></returns>
        private List<MenuViewModel> GetSubMenu(MenuViewModel parentMenu, List<MenuViewModel> menus, string stringMenuPermissions = null)
        {
            var children = menus.Where(e => e.ParentId == parentMenu.Id).OrderBy(o => o.Index).ToList();

            foreach (var item in children)
            {
                if ((!string.IsNullOrEmpty(stringMenuPermissions) && stringMenuPermissions.Contains(item.Id)) || item.IsDefaultMenu)
                {
                    item.Match = 1;
                }
                item.Children = GetSubMenu(item, menus, stringMenuPermissions);
                if (item.Children.Count > 0)
                {
                    item.Type = "collapsable";
                }
                else
                {
                    item.Type = "item";
                }
                parentMenu.Match = parentMenu.Match + item.Match;
            }
            if (!string.IsNullOrEmpty(stringMenuPermissions))
            {
                children = children.Where(e => e.Match > 0).ToList();
            }
            return children;
        }

        /// <summary>
        /// Xem chi tiết menu theo id
        /// </summary>
        /// <returns></returns>
        public async Task<MenuViewModel> GetMenuByIdAsync(string id)
        {
            MenuViewModel result = new MenuViewModel();
            result = (from a in _sqlContext.Nts_MenuSystem.AsNoTracking()
                      where id.Equals(a.Id)
                      select new MenuViewModel
                      {
                          Id = a.Id,
                          TitleKeyTranslate = a.TitleKeyTranslate,
                          TitleDefault = a.TitleDefault,
                          Icon = a.Icon,
                          Url = a.Url,
                          ParentId = a.ParentId,
                          IsDisable = a.IsDisable,
                          Index = a.Index,
                          IsDefaultMenu = a.IsDefaultMenu,
                          FunctionAuto = a.FunctionAuto,
                          SystemFunctionConfigId = a.SystemFunctionConfigId
                      }).FirstOrDefault();

            result.ListPermission = (from a in _sqlContext.Nts_MenuSystemPermission.AsNoTracking().Where(w => w.MenuSystemId.Equals(id))
                                     join b in _sqlContext.Nts_Function.AsNoTracking() on a.FunctionId equals b.Id
                                     select new PermissionModel()
                                     {
                                         Id = a.FunctionId,
                                         Code = b.Code,
                                         Name = b.Name,
                                         Index = b.Index,
                                         IsChecked = true
                                     }).OrderBy(o => o.Index).ToList();
            return result;
        }

        /// <summary>
        /// thêm menu
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <returns></returns>
        public async Task CreateMenuAsync(CreateMenuModel model)
        {
            if (string.IsNullOrEmpty(model.TitleDefault))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0020);
            }
            var checkurl = _sqlContext.Nts_MenuSystem.FirstOrDefault(e => !string.IsNullOrEmpty(model.Url) && e.Url.Equals(model.Url));
            if (checkurl != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0025);
            }

            //Nếu là chức năng phát triển và list quyền >0 thì kiểm tra mã quyền đã tồn tại chưa
            if (!model.FunctionAuto && model.ListPermission?.Count > 0)
            {
                string listPermissionCode = string.Join(";", model.ListPermission.Select(s => s.Code).ToList());
                var functionsExit = _sqlContext.Nts_Function.Where(s => listPermissionCode.Contains(s.Code)).ToList();
                if (functionsExit.Count > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0033);
                }
            }

            //Thêm menu
            Nts_MenuSystem menu = new Nts_MenuSystem()
            {
                Id = Guid.NewGuid().ToString(),
                TitleDefault = model.TitleDefault,
                TitleKeyTranslate = model.TitleDefault,
                Icon = model.Icon,
                Url = model.Url,
                ParentId = model.ParentId,
                IsDisable = model.IsDisable,
                IsDefaultMenu = model.IsDefaultMenu,
                FunctionAuto = model.FunctionAuto,
                SystemFunctionConfigId = model.SystemFunctionConfigId
            };
            _sqlContext.Nts_MenuSystem.Add(menu);

            #region Thêm vào bảng con
            if (model.ListPermission?.Count > 0)
            {
                //thêm vào bảng menusytempermission
                Nts_MenuSystemPermission menusystempermission;
                Nts_Function function;
                Nts_UserPermission userpermission;
                Nts_UserGroupFunction usergroupfunction;
                int index = 0;
                foreach (var item in model.ListPermission)
                {
                    //Add quyền mới
                    function = new Nts_Function()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = item.Code,
                        Name = item.Name,
                        Index = index,
                    };
                    _sqlContext.Nts_Function.Add(function);
                    index++;

                    //Add quyền mới cho menu
                    menusystempermission = new Nts_MenuSystemPermission()
                    {
                        Id = Guid.NewGuid().ToString(),
                        MenuSystemId = menu.Id,
                        FunctionId = function.Id
                    };
                    _sqlContext.Nts_MenuSystemPermission.Add(menusystempermission);

                    //phân quyền luôn cho admin
                    userpermission = new Nts_UserPermission()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FunctionId = function.Id,
                        UserId = NTSConstants.IdUserAdminFix
                    };
                    _sqlContext.Nts_UserPermission.Add(userpermission);

                    //phân quyền luôn cho root
                    userpermission = new Nts_UserPermission()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FunctionId = function.Id,
                        UserId = NTSConstants.IdUserRootFix
                    };
                    _sqlContext.Nts_UserPermission.Add(userpermission);

                    //Thêm quyền cho nhóm quyền admin
                    usergroupfunction = new Nts_UserGroupFunction()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FunctionId = function.Id,
                        UserGroupId = NTSConstants.GroupAdminId
                    };
                    _sqlContext.Nts_UserGroupFunction.Add(usergroupfunction);
                }
            }
            #endregion
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }

        }

        /// <summary>
        /// sửa menu
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        public async Task UpdateMenuAsync(string id, CreateMenuModel model)
        {
            var menu = _sqlContext.Nts_MenuSystem.FirstOrDefault(e => e.Id.Equals(id));
            if (menu == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0024);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    //Cập nhật thông tin menu
                    menu.TitleDefault = model.TitleDefault;
                    menu.TitleKeyTranslate = model.TitleKeyTranslate;
                    menu.Url = model.Url;
                    menu.ParentId = model.ParentId;
                    menu.Icon = model.Icon;
                    menu.IsDisable = model.IsDisable;
                    menu.IsDefaultMenu = model.IsDefaultMenu;
                    menu.FunctionAuto = model.FunctionAuto;
                    menu.SystemFunctionConfigId = model.SystemFunctionConfigId;
                    _sqlContext.Nts_MenuSystem.Update(menu);

                    List<Nts_MenuSystemPermission> listMenuPermissionRemove = null;
                    if (model.ListPermission?.Count > 0)
                    {
                        var listFuntionId = string.Join(";", model.ListPermission.Select(s => s.Id).ToList());
                        //Xóa các quyền nếu có
                        listMenuPermissionRemove = _sqlContext.Nts_MenuSystemPermission.Where(r => r.MenuSystemId.Equals(id) && !listFuntionId.Contains(r.FunctionId)).ToList();
                    }
                    else
                    {
                        //Xóa các quyền nếu có
                        listMenuPermissionRemove = _sqlContext.Nts_MenuSystemPermission.Where(r => r.MenuSystemId.Equals(id)).ToList();
                    }

                    if (listMenuPermissionRemove?.Count > 0)
                    {
                        _sqlContext.Nts_MenuSystemPermission.RemoveRange(listMenuPermissionRemove);

                        var listFuntionIdRemove = string.Join(";", listMenuPermissionRemove.Select(s => s.Id).ToList());
                        //Xóa trong nhóm người dùng
                        var userGroupFunctionRemove = _sqlContext.Nts_UserGroupFunction.Where(s => listFuntionIdRemove.Contains(s.FunctionId)).ToList();
                        if (userGroupFunctionRemove.Count > 0)
                            _sqlContext.Nts_UserGroupFunction.RemoveRange(userGroupFunctionRemove);

                        //Xóa quyền trong người dùng
                        var userPermissionRemove = _sqlContext.Nts_UserPermission.Where(s => listFuntionIdRemove.Contains(s.FunctionId)).ToList();
                        if (userPermissionRemove.Count > 0)
                            _sqlContext.Nts_UserPermission.RemoveRange(userPermissionRemove);

                        //Xóa quyền trong bảng quyền
                        var functionRemove = _sqlContext.Nts_Function.Where(s => listFuntionIdRemove.Contains(s.Id)).ToList();
                        if (functionRemove.Count > 0)
                            _sqlContext.Nts_Function.RemoveRange(functionRemove);
                    }

                    #region Thêm quyền cho menu và các bảng liên kết
                    if (model.ListPermission.Count > 0)
                    {
                        //thêm vào bảng menusytempermission
                        Nts_MenuSystemPermission menusystempermission;
                        Nts_Function function;
                        Nts_UserPermission userpermission;
                        Nts_UserGroupFunction usergroupfunction;
                        int index = 0;
                        foreach (var item in model.ListPermission)
                        {
                            //Thêm mới hoặc cập nhật quyền
                            function = _sqlContext.Nts_Function.FirstOrDefault(s => s.Id.Equals(item.Id)) ?? new Nts_Function();
                            function.Code = item.Code;
                            function.Name = item.Name;
                            function.Index = index;
                            if (string.IsNullOrEmpty(function.Id))
                            {
                                function.Id = Guid.NewGuid().ToString();
                                _sqlContext.Nts_Function.Add(function);
                            }
                            else
                            {
                                _sqlContext.Nts_Function.Update(function);
                            }
                            index++;

                            //Add quyền mới cho menu
                            menusystempermission = _sqlContext.Nts_MenuSystemPermission.FirstOrDefault(s => s.FunctionId.Equals(item.Id)) ?? new Nts_MenuSystemPermission();
                            menusystempermission.MenuSystemId = menu.Id;
                            menusystempermission.FunctionId = function.Id;
                            if (string.IsNullOrEmpty(menusystempermission.Id))
                            {
                                menusystempermission.Id = Guid.NewGuid().ToString();
                                _sqlContext.Nts_MenuSystemPermission.Add(menusystempermission);
                            }
                            else
                            {
                                _sqlContext.Nts_MenuSystemPermission.Update(menusystempermission);
                            }

                            //phân quyền luôn cho admin
                            userpermission = _sqlContext.Nts_UserPermission.FirstOrDefault(s => s.FunctionId.Equals(item.Id) && s.UserId.Equals(NTSConstants.IdUserAdminFix)) ?? new Nts_UserPermission();
                            userpermission.FunctionId = function.Id;
                            userpermission.UserId = NTSConstants.IdUserAdminFix;
                            if (string.IsNullOrEmpty(userpermission.Id))
                            {
                                userpermission.Id = Guid.NewGuid().ToString();
                                _sqlContext.Nts_UserPermission.Add(userpermission);
                            }
                            else
                            {
                                _sqlContext.Nts_UserPermission.Update(userpermission);
                            }

                            //phân quyền luôn cho root
                            userpermission = _sqlContext.Nts_UserPermission.FirstOrDefault(s => s.FunctionId.Equals(item.Id) && s.UserId.Equals(NTSConstants.IdUserRootFix)) ?? new Nts_UserPermission();
                            userpermission.FunctionId = function.Id;
                            userpermission.UserId = NTSConstants.IdUserRootFix;
                            if (string.IsNullOrEmpty(userpermission.Id))
                            {
                                userpermission.Id = Guid.NewGuid().ToString();
                                _sqlContext.Nts_UserPermission.Add(userpermission);
                            }
                            else
                            {
                                _sqlContext.Nts_UserPermission.Update(userpermission);
                            }

                            //Thêm quyền cho nhóm quyền admin
                            usergroupfunction = _sqlContext.Nts_UserGroupFunction.FirstOrDefault(s => s.FunctionId.Equals(item.Id) && s.UserGroupId.Equals(NTSConstants.GroupAdminId)) ?? new Nts_UserGroupFunction();
                            usergroupfunction.FunctionId = function.Id;
                            usergroupfunction.UserGroupId = NTSConstants.GroupAdminId;
                            if (string.IsNullOrEmpty(usergroupfunction.Id))
                            {
                                usergroupfunction.Id = Guid.NewGuid().ToString();
                                _sqlContext.Nts_UserGroupFunction.Add(usergroupfunction);
                            }
                            else
                            {
                                _sqlContext.Nts_UserGroupFunction.Update(usergroupfunction);
                            }
                        }
                    }
                    #endregion

                    _sqlContext.SaveChanges();
                    trans.Commit();
                    RemoveRedis(id);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Ẩn hiện menu
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        public async Task DisbaleMenuAsync(string id, string userId)
        {
            var menu = _sqlContext.Nts_MenuSystem.FirstOrDefault(e => e.Id.Equals(id));
            if (menu == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0024);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    menu.IsDisable = !menu.IsDisable;
                    _sqlContext.Nts_MenuSystem.Update(menu);
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }

        }

        /// <summary>
        /// Xóa menu
        /// </summary>
        /// <param name="id">id menu</param>
        /// <returns></returns>
        public async Task DeleteMenuByIdAsync(string id, string userId)
        {
            var menu = _sqlContext.Nts_MenuSystem.FirstOrDefault(e => e.Id.Equals(id));
            if (menu == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0024);
            }

            var listMenuPermissionRemove = _sqlContext.Nts_MenuSystemPermission.Where(r => r.MenuSystemId.Equals(id)).ToList();
            if (listMenuPermissionRemove?.Count > 0)
            {
                _sqlContext.Nts_MenuSystemPermission.RemoveRange(listMenuPermissionRemove);

                var listFuntionIdRemove = string.Join(";", listMenuPermissionRemove.Select(s => s.Id).ToList());
                //Xóa trong nhóm người dùng
                var userGroupFunctionRemove = _sqlContext.Nts_UserGroupFunction.Where(s => listFuntionIdRemove.Contains(s.FunctionId)).ToList();
                if (userGroupFunctionRemove.Count > 0)
                    _sqlContext.Nts_UserGroupFunction.RemoveRange(userGroupFunctionRemove);

                //Xóa quyền trong người dùng
                var userPermissionRemove = _sqlContext.Nts_UserPermission.Where(s => listFuntionIdRemove.Contains(s.FunctionId)).ToList();
                if (userPermissionRemove.Count > 0)
                    _sqlContext.Nts_UserPermission.RemoveRange(userPermissionRemove);

                //Xóa quyền trong bảng quyền
                var functionRemove = _sqlContext.Nts_Function.Where(s => listFuntionIdRemove.Contains(s.Id)).ToList();
                if (functionRemove.Count > 0)
                    _sqlContext.Nts_Function.RemoveRange(functionRemove);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.Nts_MenuSystem.Remove(menu);
                    await _sqlContext.SaveChangesAsync();
                    trans.Commit();

                    //Xóa cache user login liên quan đến menu này
                    RemoveRedis(id);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xóa rediscache theo khi xóa hoặc disable menu
        /// </summary>
        /// <param name="menuid"></param>
        public void RemoveRedis(string menuid)
        {
            var users = (from a in _sqlContext.Nts_UserPermission.AsNoTracking()
                         join b in _sqlContext.Nts_MenuSystemPermission.AsNoTracking() on a.FunctionId equals b.FunctionId
                         join c in _sqlContext.Nts_MenuSystem.Where(e => menuid.Equals(e.Id)).AsNoTracking() on b.MenuSystemId equals c.Id
                         select a.UserId).Distinct().ToList();
            // Key lưu cache login
            foreach (var item in users)
            {
                string keymenu = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixMenuKey}{item}";
                if (_redisCacheService.ExistsAsync(keymenu).Result)
                {
                    var IsCheck = _redisCacheService.RemoveAsync(keymenu).Result;
                }
            }

        }

        /// <summary>
        /// Cập nhật vị trí hiển thị của menu
        /// </summary>
        /// <param name="listMenu"></param>
        /// <returns></returns>
        public async Task UpdateIndexMenu(List<UpdateIndexMenuModel> listMenu)
        {
            if (listMenu != null && listMenu.Count > 0)
            {
                var listMenuUpdate = _sqlContext.Nts_MenuSystem.ToList();
                int index = 0;
                Nts_MenuSystem itemMenuUdate;
                foreach (var item in listMenu)
                {
                    itemMenuUdate = listMenuUpdate.FirstOrDefault(x => x.Id == item.Id);
                    if (itemMenuUdate != null)
                    {
                        itemMenuUdate.ParentId = "";
                        itemMenuUdate.Index = index;
                        index++;
                    }

                    if (item.Children != null && item.Children.Count > 0)
                        this.UpdateIndexSubMenu(item.Id, listMenuUpdate, item.Children);
                }
                using (var trans = _sqlContext.Database.BeginTransaction())
                {
                    try
                    {
                        _sqlContext.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        _sqlContext.ChangeTracker.Clear();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Cập nhật vị trí hiển thị của sub menu
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="listMenuUpdate"></param>
        /// <param name="subMenu"></param>
        private void UpdateIndexSubMenu(string parentId, List<Nts_MenuSystem> listMenuUpdate, List<UpdateIndexMenuModel> subMenu)
        {
            if (subMenu != null && subMenu.Count > 0)
            {
                int index = 0;
                Nts_MenuSystem itemMenuUdate;
                foreach (var item in subMenu)
                {
                    itemMenuUdate = listMenuUpdate.FirstOrDefault(x => x.Id == item.Id);
                    if (itemMenuUdate != null)
                    {
                        itemMenuUdate.ParentId = parentId;
                        itemMenuUdate.Index = index;
                        index++;
                    }

                    if (item.Children != null && item.Children.Count > 0)
                        this.UpdateIndexSubMenu(item.Id, listMenuUpdate, item.Children);
                }
            }
        }

        #region Code xử lý lấy các quyền Fix của chức năng tự động
        /// <summary>
        /// search menu
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public async Task<List<FunctionAutoModel>> GetListFuntionAuto()
        {
            List<FunctionAutoModel> listFunctionAuto = _sqlContext.Nts_FunctionAuto.Select(s => new FunctionAutoModel()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                Index = s.Index
            }).OrderBy(o => o.Index).ToList();

            return listFunctionAuto;
        }
        #endregion
    }
}
