using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Utils;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.Function;
using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.Cores.User;
using NTS_ERP.Models.Entities;
using NTS_ERP.Services.Cores.Auth;
using NTS_ERP.Services.Cores.Menu;
using TrafficControl.Core;

namespace NTS_ERP.Services.Cores.Users
{
    public class UserService : IUserService
    {
        private readonly NTS_ERPContext _sqlContext;
        private readonly IAuthService _authService;
        private readonly IMenuSystemService _menuSystemService;
        public UserService(NTS_ERPContext sqlContext, IAuthService authService, IMenuSystemService menuSystemService)
        {
            this._sqlContext = sqlContext;
            this._authService = authService;
            this._menuSystemService = menuSystemService;
        }

        /// <summary>
        /// Tìm kiếm user quản trị
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<UserSearchResultModel>> SearchUserAsync(UserSearchModel searchModel)
        {
            SearchBaseResultModel<UserSearchResultModel> searchResult = new SearchBaseResultModel<UserSearchResultModel>();

            var dataQuery = (from a in _sqlContext.Nts_User.AsNoTracking()
                             orderby a.UserName
                             where !a.Id.Equals(NTSConstants.IdUserRootFix)
                             select new
                             {
                                 a.Id,
                                 a.UserName,
                                 a.FullName,
                                 a.LockoutEnabled,
                                 a.Description
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.UserName))
            {
                dataQuery = dataQuery.Where(a => a.UserName.ToUpper().Contains(searchModel.UserName.ToUpper()));
            }

            //if (searchModel.LockoutEnabled.HasValue)
            //{
            //    dataQuery = dataQuery.Where(a => a.LockoutEnabled == searchModel.LockoutEnabled);
            //}

            if (!string.IsNullOrEmpty(searchModel.FullName))
            {
                dataQuery = dataQuery.Where(a => a.FullName.ToUpper().Contains(searchModel.FullName.ToUpper()));
            }
            //if (!string.IsNullOrEmpty(searchModel.Description))
            //{
            //    dataQuery = dataQuery.Where(a => a.Description.ToUpper().Contains(searchModel.Description.ToUpper()));
            //}
            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);

            }
            else
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, "UserName");
            }

            searchResult.TotalItems = dataQuery.Count();

            searchResult.DataResults = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new UserSearchResultModel()
            {
                Id = s.Id,
                UserName = s.UserName,
                FullName = s.FullName,
                LockoutEnabled = s.LockoutEnabled,
                Description = s.Description,
            }).ToList();
            return searchResult;
        }

        /// <summary>
        /// Thêm mới tài khoản đăng nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> CreateUserAsync(UserCreateModel model, string userId)
        {
            //Xử lý khoảng trống
            model.UserName = model.UserName.NTSTrim();
            model.Email = model.Email.NTSTrim();
            model.PhoneNumber = model.PhoneNumber.NTSTrim();

            //Kiểm tra tồn tại tài khoản
            var userName = _sqlContext.Nts_User.AsNoTracking().FirstOrDefault(a => a.UserName.Equals(model.UserName));
            if (userName != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0006);
            }

            //Kiểm tra tồn tại mail
            var email = _sqlContext.Nts_User.AsNoTracking().FirstOrDefault(a => a.Email.Equals(model.Email) && !string.IsNullOrEmpty(model.Email));
            if (email != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0007);
            }

            //Kiểm tra tồn tại số điện thoại
            var phoneNumber = _sqlContext.Nts_User.AsNoTracking().FirstOrDefault(a => a.PhoneNumber.Equals(model.PhoneNumber) && !string.IsNullOrEmpty(model.PhoneNumber));
            if (phoneNumber != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0008);
            }

            Nts_User user = new Nts_User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Avatar = model.Avatar,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = PasswordUtils.CreateSecurityStamp(),
                LockoutEnabled = model.LockoutEnabled,
                UserGroupId = model.UserGroupId,
                CreateBy = userId,
                CreateDate = DateTime.Now,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
                FullName = model.FullName,
                Description = model.Description,
                IdDonVi = model.IdDonVi,
            };
            user.PasswordHash = PasswordUtils.ComputeHash($"{model.Password}{user.SecurityStamp}");
            _sqlContext.Nts_User.Add(user);

            // Thêm mới bảng quyền
            List<Nts_UserPermission> userPermissions = new List<Nts_UserPermission>();
            Nts_UserPermission userPermission;

            foreach (var item in model.ListGroupFunction)
            {
                if (item.Permissions.Count > 0)
                {
                    var userpermission = AddPermission(item.Permissions, user.Id);
                    userPermissions.AddRange(userpermission);

                }
                if (item.Children.Count > 0)
                {
                    var listper = FindPermission(item);
                    var userpermission = AddPermission(listper, user.Id);
                    userPermissions.AddRange(userpermission);
                }
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.Nts_UserPermission.AddRange(userPermissions);
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

            return user.Id;
        }
        private List<FunctionModel> FindPermission(GroupFunctionModel group)
        {
            List<FunctionModel> userPermissions = new List<FunctionModel>();
            var children = group.Children;
            foreach (var item in children)
            {
                userPermissions.AddRange(item.Permissions);
                if (item.Children.Count > 0)
                {
                    var listpermissionchild = FindPermission(item);
                    userPermissions.AddRange(listpermissionchild);
                }
            }

            return userPermissions;
        }
        private List<Nts_UserPermission> AddPermission(List<FunctionModel> permissions, string userId)
        {
            List<Nts_UserPermission> userPermissions = new List<Nts_UserPermission>();
            Nts_UserPermission userPermission;
            foreach (var per in permissions)
            {
                if (per.IsChecked)
                {
                    userPermission = new Nts_UserPermission
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,
                        FunctionId = per.Id,
                    };

                    userPermissions.Add(userPermission);
                }
            }
            return userPermissions;
        }
        /// <summary>
        /// Cập nhật thông tin tài khoản đăng nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(string id, UserCreateModel model, string userId = null)
        {
            //Xử lý khoảng trống
            model.Email = model.Email.NTSTrim();
            model.PhoneNumber = model.PhoneNumber.NTSTrim();

            var user = _sqlContext.Nts_User.FirstOrDefault(i => i.Id.Equals(id));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }
            //Kiểm tra tồn tại mail
            var email = _sqlContext.Nts_User.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(id) && a.Email.Equals(model.Email) && !string.IsNullOrEmpty(model.Email));
            if (email != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0007);
            }

            //Kiểm tra tồn tại số điện thoại
            var phoneNumber = _sqlContext.Nts_User.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(id) && a.PhoneNumber.Equals(model.PhoneNumber) && !string.IsNullOrEmpty(model.PhoneNumber));
            if (phoneNumber != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0008);
            }
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                user.Avatar = model.Avatar;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.LockoutEnabled = model.LockoutEnabled;
                user.UserGroupId = model.UserGroupId;
                user.UpdateBy = userId;
                user.UpdateDate = DateTime.Now;
                user.Description = model.Description;
                user.FullName = model.FullName;
                user.IdDonVi = model.IdDonVi;

                var userPer = _sqlContext.Nts_UserPermission.Where(r => r.UserId.Equals(id)).ToList();
                string perOld = string.Join(";", userPer.Select(s => s.FunctionId).OrderBy(o => o));
                if (userPer != null)
                {
                    _sqlContext.Nts_UserPermission.RemoveRange(userPer);
                }
                List<Nts_UserPermission> userPermissions = new List<Nts_UserPermission>();
                Nts_UserPermission userPermission;

                foreach (var item in model.ListGroupFunction)
                {
                    if (item.Permissions.Count > 0)
                    {
                        var userpermission = AddPermission(item.Permissions, user.Id);
                        userPermissions.AddRange(userpermission);

                    }
                    if (item.Children.Count > 0)
                    {
                        var listper = FindPermission(item);
                        var userpermission = AddPermission(listper, user.Id);
                        userPermissions.AddRange(userpermission);
                    }
                }
                var _pernew = userPermissions.Select(e => e.FunctionId).ToList();
                _sqlContext.Nts_UserPermission.AddRange(userPermissions);
                string perNew = string.Join(";", _pernew);
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                    //nếu thay đổi nhóm người dùng hoặc thay đổi quyền trong nhóm người dùng thì xóa cache của người dùng
                    if (!perOld.Equals(perNew))
                    {
                        // Key lưu cache login
                        _authService?.RemoveRedis(id);
                    }
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
        /// Khóa tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UserLockAsync(string id, bool isunlock)
        {
            var user = _sqlContext.Nts_User.FirstOrDefault(r => r.Id.Equals(id));

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }
            if (user != null)
            {
                user.LockoutEnabled = isunlock;
                _authService.RemoveRedis(id);
                _sqlContext.SaveChanges();
            }
        }

        /// <summary>
        /// Lấy thông tin tài khoản
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<UserCreateModel> GetUserByIdAsnyc(string UserId)
        {
            // Lấy thông tin user
            var result = (from a in _sqlContext.Nts_User.AsNoTracking()
                          where a.Id.Equals(UserId)
                          join b in _sqlContext.Nts_UserGroup.AsNoTracking()
                          on a.UserGroupId equals b.Id
                          select new UserCreateModel()
                          {
                              Id = a.Id,
                              UserName = a.UserName,
                              Avatar = a.Avatar,
                              Email = a.Email,
                              FullName = a.FullName,
                              PhoneNumber = a.PhoneNumber,
                              LockoutEnabled = a.LockoutEnabled,
                              UserGroupId = a.UserGroupId,
                              NameGroupUser = b.Name,
                              Description = a.Description,
                              IdDonVi = a.IdDonVi
                          }).FirstOrDefault();

            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }
            result.ListGroupFunction = GetPermissionAsync(result.UserGroupId, UserId).Result;

            return result;
        }

        /// <summary>
        /// Xóa user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(string id, string userid)
        {
            var user = _sqlContext.Nts_User.FirstOrDefault(i => i.Id.Equals(id));
            var userPermission = _sqlContext.Nts_UserPermission.Where(a => a.UserId.Equals(id)).ToList();
            var userHistory = _sqlContext.Nts_UserHistory.Where(a => a.UserId.Equals(id)).ToList();
            var refreshToken = _sqlContext.Nts_RefreshToken.Where(a => a.UserId.Equals(id)).ToList();

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                if (userPermission.Count > 0)
                {
                    _sqlContext.Nts_UserPermission.RemoveRange(userPermission);
                }
                if (userHistory.Count > 0)
                {
                    _sqlContext.Nts_UserHistory.RemoveRange(userHistory);
                }
                if (refreshToken.Count > 0)
                {
                    _sqlContext.Nts_RefreshToken.RemoveRange(refreshToken);
                }

                if (user != null)
                {
                    _sqlContext.Nts_User.Remove(user);
                    _authService?.RemoveRedis(id);
                }
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
        /// Lấy quyền danh sách quyền
        /// </summary>
        /// <returns></returns>
        public async Task<List<GroupFunctionModel>> GetPermissionAsync(string groupUserId, string userId = null)
        {
            List<GroupFunctionModel> groupFunctions = new List<GroupFunctionModel>();
            //Danh sách menu
            var menuSystems = (from a in _sqlContext.Nts_MenuSystem.AsNoTracking()
                               where !a.IsDefaultMenu
                               select new GroupFunctionModel
                               {
                                   Id = a.Id,
                                   Name = a.TitleDefault,
                                   ParentId = a.ParentId,
                                   Index = a.Index
                               }).OrderBy(o => o.Index).ToList();
            List<FunctionModel> permissions = new List<FunctionModel>();
            var _permissions = (from p in _sqlContext.Nts_Function.AsNoTracking()
                                join b in _sqlContext.Nts_MenuSystemPermission.AsNoTracking() on p.Id equals b.FunctionId
                                orderby p.Code
                                select new FunctionModel
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Code = p.Code,
                                    IsChecked = true,
                                    MenuSystemId = b.MenuSystemId,
                                }).ToList();
            List<string> fucnction = new List<string>();
            if (!string.IsNullOrEmpty(groupUserId))
            {
                fucnction = (from a in _sqlContext.Nts_UserGroupFunction.AsNoTracking()
                             where groupUserId.Equals(a.UserGroupId)
                             select a.FunctionId
                          ).ToList();
            }
            else
            {
                fucnction = (from a in _sqlContext.Nts_UserGroupFunction.AsNoTracking()
                             select a.FunctionId
                          ).ToList();
            }


            var string_functionid = string.Join(";", fucnction);
            _permissions = _permissions.Where(e => string_functionid.Contains(e.Id)).ToList();

            if (!string.IsNullOrEmpty(userId))
            {
                var permissionsold = (from a in _sqlContext.Nts_UserPermission.AsNoTracking()
                                      where userId.Equals(a.UserId)
                                      select a.FunctionId).ToList();
                var permissionid_string = string.Join(";", permissionsold);
                foreach (var permission in _permissions)
                {
                    if (permissionid_string.Contains(permission.Id))
                    {
                        permission.IsChecked = true;
                    }
                    else
                    {
                        permission.IsChecked = false;
                    }
                    permissions.Add(permission);
                }
            }
            else
            {
                permissions = _permissions;
            }
            var list_GFunctionid = string.Join(";", permissions.Select(e => e.MenuSystemId).Distinct().ToList());

            var parents = menuSystems.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
            foreach (var ite in parents)
            {
                ite.Permissions = permissions.Where(t => t.MenuSystemId.Equals(ite.Id)).ToList();
                ite.PermissionTotal = ite.Permissions.Count;
                ite.IsChecked = ite.Permissions.Count(r => !r.IsChecked) == 0;
                ite.CheckCount = ite.Permissions.Count(r => r.IsChecked);
                ite.Children = GetChildenPermission(ite, menuSystems, permissions);
                if (ite.Permissions.Count == 0)
                {
                    ite.CountPermission = "";
                }
                else
                {
                    ite.CountPermission = ite.CheckCount.ToString() + "/" + ite.PermissionTotal.ToString();
                }

                groupFunctions.Add(ite);
            }
            groupFunctions = groupFunctions.Where(e => e.Permissions.Count > 0 || e.Children.Count > 0).ToList();
            return groupFunctions;
        }

        private List<GroupFunctionModel> GetChildenPermission(GroupFunctionModel group, List<GroupFunctionModel> listgroup, List<FunctionModel> permissions)
        {
            List<GroupFunctionModel> result = new List<GroupFunctionModel>();
            var children = listgroup.Where(e => group.Id.Equals(e.ParentId)).ToList();
            foreach (var child in children)
            {
                child.Permissions = permissions.Where(t => t.MenuSystemId.Equals(child.Id)).ToList();
                child.PermissionTotal = child.Permissions.Count;
                child.IsChecked = child.Permissions.Count(r => !r.IsChecked) == 0;
                child.CheckCount = child.Permissions.Count(r => r.IsChecked);
                child.Children = GetChildenPermission(child, listgroup, permissions);
                if (child.Permissions.Count == 0)
                {
                    child.CountPermission = "";
                }
                else
                {
                    child.CountPermission = child.CheckCount.ToString() + "/" + child.PermissionTotal.ToString();
                }
                result.Add(child);
            }
            return result.Where(e => e.Permissions.Count > 0 || e.Children.Count > 0).ToList();
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task ChangePasswordAsync(ChangePasswordModel model)
        {
            if (model.MatKhauMoi != model.XacNhanMatKhauMoi)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0010);
            }
            if (string.IsNullOrEmpty(model.Id))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }
            var user = _sqlContext.Nts_User.FirstOrDefault(r => r.Id.Equals(model.Id));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }
            if (!model.IsChange)
            {
                var passwordHash = PasswordUtils.ComputeHash(model.MatKhauCu + user.SecurityStamp);
                if (!user.PasswordHash.Equals(passwordHash))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0005);
                }
            }

            user.PasswordHash = PasswordUtils.ComputeHash(model.MatKhauMoi + user.SecurityStamp);

            _sqlContext.SaveChangesAsync();
            //xóa cache đăng nhâp của user bị thay đổi pass
            _authService?.RemoveRedis(user.Id);

        }

        /// <summary>
        /// Lấy thông tin tài khoản
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserInfoModel> GetUserInfoAsnyc(string userId)
        {
            // Lấy thông tin user
            var result = await (from a in _sqlContext.Nts_User.AsNoTracking()
                                where a.Id.Equals(userId)
                                select new UserInfoModel()
                                {
                                    Id = a.Id,
                                    UserName = a.UserName,
                                    Avatar = a.Avatar,
                                    Email = a.Email,
                                    PhoneNumber = a.PhoneNumber,
                                    FullName = a.FullName,
                                    IdDonVi = a.IdDonVi
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }

            result.Permission = _authService.GetListPermission(userId);
            result.MenuView = this._menuSystemService.GetMenuByUserAsync(userId).Result;

            //Lấy thông tin đơn vị
            if (!string.IsNullOrEmpty(result.IdDonVi))
            {
                var listDonVi = _sqlContext.DonVi.AsNoTracking().ToList();

                var donVi = (from a in listDonVi
                             where a.IdDonVi.Equals(result.IdDonVi)
                             select new
                             {
                                 a.IdDonVi,
                                 Ten = a.Ten.NTSTrim(),
                                 IdTinh = a.IdTinh ?? "",
                                 IdHuyen = a.IdHuyen ?? "",
                                 IdXa = a.IdXa ?? "",
                                 a.IdDonViCha,
                                 a.Level,
                                 a.IdLoaiDonVi
                             }).FirstOrDefault();
                if (donVi != null)
                {
                    result.ProvinceId = donVi.IdTinh;
                    result.DistrictId = donVi.IdHuyen;
                    result.WardId = donVi.IdXa;
                    result.UnitName = listDonVi.Where(r => r.IdDonVi.Equals(donVi.IdDonViCha)).FirstOrDefault()?.Ten ?? "Bộ Đội Biên Phòng";
                    result.DepartmentName = donVi.Ten;
                }
            }

            return result;
        }

        /// <summary>
        /// Cập nhật người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateUserInfoAsync(string id, UserInfoModel model, string userId)
        {
            var user = _sqlContext.Nts_User.FirstOrDefault(i => i.Id.Equals(id));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                user.UserName = model.UserName.NTSTrim();
                user.Avatar = model.Avatar;
                user.Email = model.Email.NTSTrim();
                user.PhoneNumber = model.PhoneNumber.NTSTrim();
                user.FullName = model.FullName;
                user.UpdateBy = userId;
                user.UpdateDate = DateTime.Now;

                try
                {
                    await _sqlContext.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
    }
}
