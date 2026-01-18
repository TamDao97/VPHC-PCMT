using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Redis;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.Function;
using NTS_ERP.Models.Cores.GroupFunction;
using NTS_ERP.Models.Cores.GroupUser;
using NTS_ERP.Models.Entities;
using NTS_ERP.Services.Cores.Combobox;
using TrafficControl.Core;

namespace NTS_ERP.Services.Cores.GroupUsers
{
    public class GroupUserService : IGroupUserService
    {

        private readonly NTS_ERPContext sqlContext;
        private readonly RedisCacheSettingModel redisCacheSetting;
        private readonly RedisCacheService _redisCacheService;
        private readonly IComboboxService _comboboxService;

        public GroupUserService(NTS_ERPContext sqlContext, RedisCacheService redisCacheService, IComboboxService comboboxService, IOptions<RedisCacheSettingModel> redisOptions)
        {
            this.sqlContext = sqlContext;
            this._redisCacheService = redisCacheService;
            this.redisCacheSetting = redisOptions.Value;
            this._comboboxService = comboboxService;
        }

        /// <summary>
        /// Thêm nhóm quyền người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateGroupUser(GroupFunctionCreateModel model, string userId)
        {
            var isExistName = sqlContext.Nts_UserGroup.FirstOrDefault(u => u.Name.ToLower().Equals(model.Name.ToLower()));
            if (isExistName != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0014);
            }

            using (var trans = sqlContext.Database.BeginTransaction())
            {
                var dateNow = DateTime.Now;

                Nts_UserGroup group = new Nts_UserGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Description = model.Description,
                    //Type = model.Type,
                    CreateBy = userId,
                    CreateDate = dateNow,
                    UpdateBy = userId,
                    UpdateDate = dateNow,
                };

                sqlContext.Nts_UserGroup.Add(group);

                #region Thêm vào bảng con
                if (model.ListPermission.Count > 0)
                {
                    List<Nts_UserGroupFunction> usergroupPermissions = new List<Nts_UserGroupFunction>();

                    List<FunctionModel> permissionsCheck = null;
                    Nts_UserGroupFunction groupFunctionAdd;
                    foreach (var item in model.ListPermission)
                    {
                        if (item.Children?.Count == 0)
                        {
                            permissionsCheck = item.Permissions?.Where(s => s.IsChecked).ToList();
                            if (permissionsCheck?.Count() > 0)
                            {
                                foreach (var permission in permissionsCheck)
                                {
                                    groupFunctionAdd = new Nts_UserGroupFunction()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        UserGroupId = group.Id,
                                        FunctionId = permission.Id,
                                    };
                                    usergroupPermissions.Add(groupFunctionAdd);
                                }
                            }
                        }
                        else if (item.Children?.Count > 0)
                        {
                            AddGroupUserPermission(usergroupPermissions, item.Children, group.Id);
                        }
                    }
                    sqlContext.Nts_UserGroupFunction.AddRange(usergroupPermissions);
                }
                #endregion

                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xóa nhóm quyền người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteGroupUserById(string id, string userId)
        {
            var data = sqlContext.Nts_UserGroup.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0015);
            }
            var user = sqlContext.Nts_User.FirstOrDefault(e => e.UserGroupId.Equals(data.Id));
            if (user != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0016);
            }

            var groupPermissionOld = sqlContext.Nts_UserGroupFunction.Where(u => u.UserGroupId.Equals(id));
            if (groupPermissionOld.Count() > 0)
            {
                sqlContext.Nts_UserGroupFunction.RemoveRange(groupPermissionOld);
            }
            sqlContext.Nts_UserGroup.Remove(data);
            await sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy nhóm người dùng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GroupUserModel> GetGroupUserById(string id)
        {
            var groupUserModel = sqlContext.Nts_UserGroup.FirstOrDefault(u => u.Id.Equals(id));
            GroupUserModel model = new GroupUserModel();

            if (groupUserModel != null)
            {
                model.Id = groupUserModel.Id;
                model.Name = groupUserModel.Name;
                model.Description = groupUserModel.Description;
                model.ListPermission = GetPermissionAsync(id);
            }
            else
            {
                model.Name = string.Empty;
                model.Description = string.Empty;
                model.ListPermission = GetPermissionAsync("");
            }

            return model;
        }

        private List<GroupFunctionModel> GetPermissionAsync(string groupUserId)
        {
            List<GroupFunctionModel> groupFunctions = new List<GroupFunctionModel>();

            var gFunctions = (from a in sqlContext.Nts_MenuSystem.AsNoTracking().OrderBy(o => o.Index)
                              where !a.IsDisable
                              select new GroupFunctionModel
                              {
                                  Id = a.Id,
                                  Name = a.TitleDefault,
                                  ParentId = a.ParentId,
                              }).ToList();
            List<FunctionModel> permissions = (from p in sqlContext.Nts_Function.AsNoTracking()
                                               join b in sqlContext.Nts_MenuSystemPermission.AsNoTracking() on p.Id equals b.FunctionId
                                               orderby p.Code
                                               select new FunctionModel
                                               {
                                                   Id = p.Id,
                                                   Name = p.Name,
                                                   Code = p.Code,
                                                   IsChecked = false,
                                                   MenuSystemId = b.MenuSystemId
                                               }).ToList();

            if (!string.IsNullOrEmpty(groupUserId))
            {
                var fucnction = (from a in sqlContext.Nts_UserGroupFunction.AsNoTracking()
                                 where groupUserId.Equals(a.UserGroupId)
                                 select a.FunctionId
                                 ).ToList();
                if (fucnction.Count > 0)
                {
                    var string_functionid = string.Join(";", fucnction);
                    foreach (var item in permissions)
                    {
                        if (string_functionid.Contains(item.Id))
                        {
                            item.IsChecked = true;
                        }
                    }
                }
            }

            var list_GFunctionid = string.Join(";", permissions.Select(e => e.MenuSystemId).Distinct().ToList());
            var parents = gFunctions.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
            foreach (var ite in parents)
            {
                ite.Permissions = permissions.Where(t => t.MenuSystemId.Equals(ite.Id)).ToList();
                ite.PermissionTotal = ite.Permissions.Count;
                ite.IsChecked = ite.Permissions.Count(r => !r.IsChecked) == 0;
                ite.CheckCount = ite.Permissions.Count(r => r.IsChecked);
                ite.Children = GetChildenPermission(ite, gFunctions, permissions);
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
                result.Add(child);
            }
            return result.Where(e => e.Permissions.Count > 0 || e.Children.Count > 0).ToList();
        }
        public List<PermissionModel> GetListGroupPermissions(List<string> lstCheck)
        {
            List<PermissionModel> lst = new List<PermissionModel>();

            var lstPermissions = sqlContext.Nts_Function.ToList();
            List<Nts_Function> lstPermissionsTemp;
            PermissionModel permissionModel;
            List<PermissionModel> permissions = new List<PermissionModel>();
            var menu = sqlContext.Nts_MenuSystem.ToList();
            menu = menu.Where(e => !e.IsDefaultMenu).ToList();
            int index = 0;
            foreach (var _item in menu)
            {
                var strg_function = string.Join(";", sqlContext.Nts_MenuSystemPermission.Where(e => _item.Id.Equals(e.MenuSystemId)).Select(r => r.FunctionId).ToList());
                lstPermissionsTemp = lstPermissions.Where(u => strg_function.Contains(u.Code)).ToList();
                permissionModel = new PermissionModel();
                permissionModel.Index = (index + 1);
                permissionModel.Code = _item.Id;
                permissionModel.Name = _item.TitleDefault;
                permissionModel.IsChecked = false;
                lst.Add(permissionModel);
                index = lst.Count;
                permissionModel = new PermissionModel();
                foreach (var item in lstPermissionsTemp)
                {
                    permissionModel = new PermissionModel();
                    permissionModel.Index = 0;
                    permissionModel.Code = item.Code;
                    permissionModel.Name = item.Name;
                    permissionModel.Id = item.Id;
                    permissionModel.IsChecked = lstCheck.Contains(item.Code);
                    lst.Add(permissionModel);
                    permissions.Add(permissionModel);
                }

                if (permissions.Count > 0)
                {
                    if (permissions.Where(i => i.IsChecked == false).Count() == 0)
                    {
                        lst[index - 1].IsChecked = true;
                    }
                }
                index++;
            }
            return lst;
        }

        /// <summary>
        /// Tìm kiếm nhóm người dùng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<GroupUserResultModel>> SearchGroupUser(GroupUserSearchModel searchModel)
        {
            SearchBaseResultModel<GroupUserResultModel> searchResult = new SearchBaseResultModel<GroupUserResultModel>();
            var dataQuery = (from a in sqlContext.Nts_UserGroup.AsNoTracking()
                             orderby a.Name
                             select new
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Description = a.Description,
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);

            }

            searchResult.TotalItems = dataQuery.Count();
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList().Select(s => new GroupUserResultModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
            }).ToList();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Update nhóm người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateGroupUser(string id, GroupFunctionCreateModel model, string updateby)
        {
            if (sqlContext.Nts_UserGroup.AsNoTracking().Where(o => !o.Id.Equals(id) && o.Name.ToLower().Equals(model.Name.ToLower())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0014);
            }

            using (var trans = sqlContext.Database.BeginTransaction())
            {
                var groupUser = sqlContext.Nts_UserGroup.FirstOrDefault(u => u.Id.Equals(id));
                if (groupUser == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0015);
                }

                //Cập nhật thông tin nhóm quyền
                groupUser.Name = model.Name;
                groupUser.Description = model.Description;
                groupUser.UpdateBy = updateby;
                groupUser.UpdateDate = DateTime.Now;

                //xóa quyền cũ
                var groupPermissionOld = sqlContext.Nts_UserGroupFunction.Where(u => u.UserGroupId.Equals(groupUser.Id));
                if (groupPermissionOld.Count() > 0)
                {
                    sqlContext.Nts_UserGroupFunction.RemoveRange(groupPermissionOld);
                }

                #region Thêm vào bảng con
                List<Nts_UserGroupFunction> usergroupPermissions = new List<Nts_UserGroupFunction>();
                if (model.ListPermission.Count > 0)
                {
                    List<FunctionModel> permissionsCheck = null;
                    Nts_UserGroupFunction groupFunctionAdd;
                    foreach (var item in model.ListPermission)
                    {
                        if (item.Children?.Count == 0)
                        {
                            permissionsCheck = item.Permissions?.Where(s => s.IsChecked).ToList();
                            if (permissionsCheck?.Count() > 0)
                            {
                                foreach (var permission in permissionsCheck)
                                {
                                    groupFunctionAdd = new Nts_UserGroupFunction()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        UserGroupId = id,
                                        FunctionId = permission.Id,
                                    };
                                    usergroupPermissions.Add(groupFunctionAdd);
                                }
                            }
                        }
                        else if (item.Children?.Count > 0)
                        {
                            AddGroupUserPermission(usergroupPermissions, item.Children, id);
                        }
                        sqlContext.Nts_UserGroupFunction.AddRange(usergroupPermissions);
                    }
                }
                #endregion

                #region update quyền vào những tài khoản có nhóm quyền đó.
                List<string> listPermissionOfGroup = usergroupPermissions?.Select(s => s.FunctionId).ToList();

                var usersId = sqlContext.Nts_User.Where(a => a.UserGroupId.Equals(id)).Select(s => s.Id).ToList();

                var userpermisionRemove = sqlContext.Nts_UserPermission.Where(s => usersId.Contains(s.UserId)).AsNoTracking().ToList();

                string key = string.Empty;

                List<Nts_UserPermission> listUserPermission;
                foreach (var userId in usersId)
                {
                    listUserPermission = new List<Nts_UserPermission>();
                    var userPermissRemove = userpermisionRemove.Where(s => s.UserId.Equals(userId)).ToList();
                    sqlContext.Nts_UserPermission.RemoveRange(userPermissRemove);
                    CreateUserPermission(userId, listUserPermission, listPermissionOfGroup);
                    if (listUserPermission.Count > 0)
                        sqlContext.Nts_UserPermission.AddRange(listUserPermission);
                }
                #endregion
                try
                {
                    await sqlContext.SaveChangesAsync();
                    await trans.CommitAsync();
                    foreach (var userId in usersId)
                    {
                        key = $"{redisCacheSetting.PrefixSystemKey}{redisCacheSetting.PrefixLoginKey}{userId}";
                        await _redisCacheService.RemoveAsync(key);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        private void AddGroupUserPermission(List<Nts_UserGroupFunction> listPermission, List<GroupFunctionModel> groupFunctions, string userGroupId)
        {
            Nts_UserGroupFunction groupFunctionAdd;
            List<FunctionModel> permissionsCheck = null;
            foreach (var item in groupFunctions)
            {
                if (item.Children?.Count == 0)
                {
                    permissionsCheck = item.Permissions?.Where(s => s.IsChecked).ToList();
                    if (permissionsCheck?.Count() > 0)
                    {
                        foreach (var permission in permissionsCheck)
                        {
                            groupFunctionAdd = new Nts_UserGroupFunction()
                            {
                                Id = Guid.NewGuid().ToString(),
                                UserGroupId = userGroupId,
                                FunctionId = permission.Id,
                            };
                            listPermission.Add(groupFunctionAdd);
                        }
                    }
                }
                else if (item.Children?.Count > 0)
                {
                    AddGroupUserPermission(listPermission, item.Children, userGroupId);
                }
            }
        }

        private void CreateUserPermission(string userId, List<Nts_UserPermission> listUserPermission, List<string> listPermissionId)
        {
            List<Nts_UserPermission> userPermissions = new List<Nts_UserPermission>();
            Nts_UserPermission userpermission;
            foreach (var permissionId in listPermissionId)
            {
                userpermission = new Nts_UserPermission
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    FunctionId = permissionId,
                };
                listUserPermission.Add(userpermission);
            }
        }
    }

}
