using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Redis;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.Permission;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.Cores.ConfigInterface
{
    public class PermissionService : IPermissionService
    {
        private readonly NTS_ERPContext _sqlContext;
        private readonly RedisCacheService _redisCacheService;
        private readonly RedisCacheSettingModel _redisCacheSettings;

        public PermissionService(NTS_ERPContext sqlContext, RedisCacheService redisCacheService, IOptions<RedisCacheSettingModel> options)
        {
            _sqlContext = sqlContext;
            _redisCacheService = redisCacheService;
            _redisCacheSettings = options.Value;
        }
        public async Task<SearchBaseResultModel<UserPermissionModel>> GetUserInfoAsync(PermissionSearchModel searchModel)
        {
            SearchBaseResultModel<UserPermissionModel> searchResult = new SearchBaseResultModel<UserPermissionModel>();

            var dataQuery = (from a in _sqlContext.Nts_UserPermission.AsNoTracking()
                             join b in _sqlContext.Nts_User.AsNoTracking() on a.UserId equals b.Id
                             where searchModel.Code.Equals(a.FunctionId)
                             select new UserPermissionModel
                             {
                                 UserId = b.Id,
                                 UserName = b.UserName,
                                 FullName = b.FullName
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(searchModel.UserName))
            {
                dataQuery = dataQuery.Where(e => searchModel.UserName.Contains(e.UserName));
            }
            searchResult.TotalItems = dataQuery.Count();
            var listResult = dataQuery.OrderBy(e => e.UserName).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.DataResults = listResult;
            return searchResult;

        }
        public async Task DeletePermissionAsync(string userid)
        {
            var userper = _sqlContext.Nts_UserPermission.FirstOrDefault(e => userid.Equals(e.UserId));
            if (userper == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0032);
            }
            _sqlContext.Nts_UserPermission.Remove(userper);
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                    //remove cache của user và menu
                    RemoveCache(userid);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }
        private async Task RemoveCache(string userId)
        {
            // Tạo key cache
            string key = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixLoginKey}{userId}";
            //key menu
            string keymenu = $"{_redisCacheSettings.PrefixSystemKey}{_redisCacheSettings.PrefixMenuKey}{userId}";

            // Kiểm tra cache tồn tại
            var keymenuExist = await _redisCacheService.ExistsAsync(keymenu);
            if (keymenuExist)
            {
                await _redisCacheService.RemoveAsync(keymenu);
            }
            var keyExist = await _redisCacheService.ExistsAsync(key);
            if (keyExist)
            {
                await _redisCacheService.RemoveAsync(key);
            }
        }
    }
}
