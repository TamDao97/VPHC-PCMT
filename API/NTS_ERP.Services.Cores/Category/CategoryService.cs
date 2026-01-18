using NTS_ERP.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.Category;

namespace NTS_ERP.Services.Cores.Categorys
{
    public class CategoryService : ICategoryService
    {
        private readonly NTS_ERPContext _sqlContext;

        public CategoryService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public async Task<List<GroupCategoryModel>> SearchGroupCategoryAsync()
        {
            var groupCategories = (from a in _sqlContext.Nts_GroupCategory.AsNoTracking()
                                   orderby a.Index
                                   select new GroupCategoryModel
                                   {
                                       Id = a.Id,
                                       Type = 1,
                                       Name = a.Name,
                                       Index = a.Index
                                   }).ToList();

            var subCategories = (from a in _sqlContext.Nts_Category.AsNoTracking()
                                 join b in _sqlContext.Nts_GroupCategory.AsNoTracking() on a.GroupCategoryId equals b.Id
                                 orderby a.Index
                                 select new GroupCategoryModel
                                 {
                                     Id = a.Id,
                                     ParentId = a.GroupCategoryId,
                                     Type = 2,
                                     Name = a.Name,
                                     Index = a.Index,
                                     TableName = a.TableName,
                                 }).ToList();

            if (subCategories.Count > 0)
            {
                groupCategories.AddRange(subCategories);
            }

            return groupCategories;
        }

        /// <summary>
        /// Thêm danh mục
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <param name="userId">Id người tạo</param>
        /// <returns></returns>
        public async Task CreateCategoryAsync(CategoryCreateModel model)
        {
            var categoryExist = _sqlContext.Nts_Category.AsNoTracking().FirstOrDefault(u => u.Name.ToLower().Equals(model.Name.NTSTrim().ToLower()) && !string.IsNullOrEmpty(model.TableName) && u.TableName.ToLower().Equals(model.TableName.NTSTrim().ToLower()));
            if (categoryExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0011);
            }

            var indexs = _sqlContext.Nts_Category.ToList();
            var maxIndex = 1;
            if (indexs.FirstOrDefault(i => i.Index == model.Index) != null)
            {
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(i => i.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listOrder = indexs.Where(i => i.Index >= modelIndex).ToList();
                    if (listOrder.Count > 0 && listOrder != null)
                    {
                        foreach (var item in listOrder)
                        {
                            item.Index++;
                        }
                    }
                }
            }

            Nts_Category category = new Nts_Category()
            {
                Id = Guid.NewGuid().ToString(),
                GroupCategoryId = model.GroupCategoryId,
                Name = model.Name.NTSTrim(),
                Index = model.Index,
                TableName = model.TableName.NTSTrim(),
            };

            _sqlContext.Nts_Category.Add(category);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <param name="model">Dữ liệu cập nhật</param>
        /// <param name="userId">Id người cập nhật</param>
        /// <returns></returns>
        public async Task UpdateCategoryAsync(string id, CategoryCreateModel model)
        {
            var category = _sqlContext.Nts_Category.FirstOrDefault(i => i.Id.Equals(id));
            if (category == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0011);
            }

            var categoryExist = _sqlContext.Nts_Category.AsNoTracking().FirstOrDefault(i => !i.Id.Equals(id) && i.Name.ToLower().Equals(model.Name.NTSTrim().ToLower()) && !string.IsNullOrEmpty(model.TableName) && i.TableName.ToLower().Equals(model.TableName.NTSTrim().ToLower()));
            if (categoryExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0012);
            }


            var indexs = _sqlContext.Nts_Category.ToList();
            if (indexs.FirstOrDefault(i => !i.Id.Equals(id) && i.Index == model.Index) != null)
            {
                var maxIndex = 1;
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(a => a.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listOrder = indexs.Where(b => b.Index >= modelIndex).ToList();
                    if (listOrder.Count > 0 && listOrder != null)
                    {
                        foreach (var item in listOrder)
                        {
                            item.Index++;
                        }
                    }
                }
            }

            category.GroupCategoryId = model.GroupCategoryId;
            category.Name = model.Name.NTSTrim();
            category.Index = model.Index;
            category.TableName = model.TableName.NTSTrim();

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
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

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        public async Task DeleteCategoryByIdAsync(string id)
        {
            var category = _sqlContext.Nts_Category.FirstOrDefault(i => i.Id.Equals(id));
            if (category == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0011);
            }

            var indexs = _sqlContext.Nts_Category.ToList();
            var maxIndex = 1;
            if (indexs.Count > 0)
            {
                maxIndex = indexs.Select(i => i.Index).Max();
            }

            if (category.Index < maxIndex)
            {
                int modelIndex = category.Index;
                var listOrder = indexs.Where(i => i.Index > modelIndex).ToList();
                if (listOrder.Count > 0 && listOrder != null)
                {
                    foreach (var item in listOrder)
                    {
                        item.Index--;
                    }
                }
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.Nts_Category.Remove(category);
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

        /// <summary>
        /// Lấy thông tin danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        public async Task<CategoryCreateModel> GetCategoryByIdAsync(string id)
        {
            var category = await (from a in _sqlContext.Nts_Category.AsNoTracking()
                                  where a.Id.Equals(id)
                                  select new CategoryCreateModel()
                                  {
                                      Id = a.Id,
                                      GroupCategoryId = a.GroupCategoryId,
                                      Name = a.Name,
                                      Index = a.Index,
                                      TableName = a.TableName,
                                }).FirstOrDefaultAsync();
            if (category == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0011);
            }

            return category;
        }

        /// <summary>
        /// Lấy list order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<int>> GetListOrderAsync(string id)
        {
            List<int> result = new List<int>();
            int max = 1;
            var data = await _sqlContext.Nts_Category.OrderBy(i => i.Index).ToListAsync();
            if (data.Count > 0)
            {
                result = data.Select(i => i.Index).ToList();
                max = result.Max(i => i);
                if (string.IsNullOrEmpty(id))
                {
                    result.Add(max + 1);
                }
            }
            else
            {
                result.Add(max);
            }
            return result;
        }

        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<CategoryTableDataModel>> SearchCategoryTableAsync(CategorySearchModel searchModel)
        {
            SearchBaseResultModel<CategoryTableDataModel> searchResult = new SearchBaseResultModel<CategoryTableDataModel>();

            if (!string.IsNullOrEmpty(searchModel.TableName))
            {
                var dataQuery = await GetListDataWidthTable(searchModel.TableName);

                if (!string.IsNullOrEmpty(searchModel.Name))
                {
                    dataQuery = dataQuery.Where(i => i.Name.ToUpper().Contains(searchModel.Name.ToUpper())).ToList();
                }

                searchResult.TotalItems = dataQuery.Count;
                searchResult.DataResults = dataQuery.OrderBy(i => i.Index).ToList();
            }

            return searchResult;
        }

        /// <summary>
        /// Thêm mới dữ liệu theo tên bảng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateCategoryTableAsync(CategoryModel model)
        {
            var categories = await GetListDataWidthTable(model.TableName);

            var categoryExist = categories.FirstOrDefault(u => u.Name.ToLower().Equals(model.Name.NTSTrim().ToLower()));
            if (categoryExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0012);
            }

            var indexs = categories.ToList();
            var maxIndex = 1;
            if (indexs.FirstOrDefault(i => i.Index == model.Index) != null)
            {
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(i => i.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listOrder = indexs.Where(i => i.Index >= modelIndex).ToList();
                    if (listOrder.Count > 0 && listOrder != null)
                    {
                        string commandTextUpdate = string.Empty;
                        foreach (var item in listOrder)
                        {
                            item.Index++;
                            commandTextUpdate = "UPDATE " + model.TableName + $" SET [Index] = " + item.Index + " WHERE Id = '" + item.Id + "'";
                            _sqlContext.Database.ExecuteSqlRaw(commandTextUpdate);
                        }
                    }
                }
            }

            model.Id = Guid.NewGuid().ToString();
            var commandText = "INSERT INTO [dbo].[" + model.TableName + $"] (Id, Name, [Index]) VALUES ('{model.Id}', N'{model.Name}', {model.Index})";
            _sqlContext.Database.ExecuteSqlRaw(commandText);
        }

        /// <summary>
        /// Cập nhật dữ liệu theo tên bảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateCategoryTableAsync(string id, CategoryModel model)
        {
            var categorys = await GetListDataWidthTable(model.TableName);
            var category = categorys.FirstOrDefault(i => i.Id.Equals(id));
            if (category == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0011);
            }

            var categoryExist = categorys.FirstOrDefault(i => !i.Id.Equals(id) && i.Name.ToLower().Equals(model.Name.NTSTrim().ToLower()));
            if (categoryExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0012);
            }

            var commandTextUpdate = string.Empty;
            if (category.Index != model.Index)
            {
                var indexs = categorys.ToList();
                if (indexs.FirstOrDefault(i => i.Index == model.Index) != null)
                {
                    if (category.Index > model.Index)
                    {
                        var listOrder = indexs.Where(i => i.Index >= model.Index && i.Index < category.Index).ToList();
                        if (listOrder.Count > 0 && listOrder != null)
                        {
                            foreach (var item in listOrder)
                            {
                                item.Index++;
                                commandTextUpdate = "UPDATE " + model.TableName + $" SET [Index] = " + item.Index + " WHERE Id = '" + item.Id + "'";
                                _sqlContext.Database.ExecuteSqlRaw(commandTextUpdate);
                            }
                        }
                    }
                    else if (category.Index < model.Index)
                    {
                        var listOrder = indexs.Where(i => i.Index > category.Index && i.Index <= model.Index).ToList();
                        if (listOrder.Count > 0 && listOrder != null)
                        {
                            foreach (var item in listOrder)
                            {
                                item.Index--;
                                commandTextUpdate = "UPDATE " + model.TableName + $" SET [Index] = " + item.Index + " WHERE Id = '" + item.Id + "'";
                                _sqlContext.Database.ExecuteSqlRaw(commandTextUpdate);
                            }
                        }
                    }
                }
            }

            var commandText = "UPDATE " + model.TableName + $" SET Name = @Name, [Index] = @Index WHERE Id = @Id";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Name", model.Name),
                new SqlParameter("@Index", model.Index),
            };
            _sqlContext.Database.ExecuteSqlRaw(commandText, parameters.ToArray());
        }

        /// <summary>
        /// Xóa dữ liệu theo tên bảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task DeleteCategoryTableAsync(string id, string tableName)
        {
            var categorys = await GetListDataWidthTable(tableName);
            var category = categorys.FirstOrDefault(i => i.Id.Equals(id));
            if (category == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0011);
            }

            var indexs = categorys.ToList();
            var maxIndex = 1;
            if (indexs.Count > 0)
            {
                maxIndex = indexs.Select(i => i.Index).Max();
            }

            if (category.Index < maxIndex)
            {
                int modelIndex = category.Index;
                var listOrder = indexs.Where(i => i.Index > modelIndex).ToList();
                if (listOrder.Count > 0 && listOrder != null)
                {
                    var commandTextDelete = string.Empty;
                    foreach (var item in listOrder)
                    {
                        item.Index--;
                        commandTextDelete = "UPDATE " + tableName + $" SET [Index] = " + item.Index + " WHERE Id = '" + item.Id + "'";
                        _sqlContext.Database.ExecuteSqlRaw(commandTextDelete);
                    }
                }
            }

            var commandText = "DELETE FROM " + tableName + $" WHERE Id = @Id";
            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", id),
                };
            _sqlContext.Database.ExecuteSqlRaw(commandText, parameters.ToArray());
        }

        /// <summary>
        /// Lấy thông tin danh mục
        /// </summary>
        /// <param name="id">Id danh mục</param>
        /// <returns></returns>
        public async Task<CategoryModel> GetCategoryTableByIdAsync(string id, string tableName)
        {
            CategoryModel category = new CategoryModel();
            using (DbCommand cmd = _sqlContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM [dbo].[" + tableName.NTSTrim() + "] WHERE Id = '" + id + "'";
                await _sqlContext.Database.OpenConnectionAsync();
                using (DbDataReader ddr = cmd.ExecuteReader())
                {
                    while (ddr.Read())
                    {
                        category = new CategoryModel()
                        {
                            Id = ddr["Id"].ToString(),
                            Name = ddr["Name"].ToString(),
                            Index = Convert.ToInt32(ddr["Index"].ToString())
                        };
                    }
                }
            }

            if (category == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0011);
            }

            return category;
        }

        /// <summary>
        /// Lấy list order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<int>> GetListOrderTableAsync(string id, string tableName)
        {
            List<int> result = new List<int>();
            var categorys = await GetListDataWidthTable(tableName);
            int max = 1;
            var data = categorys.OrderBy(i => i.Index).ToList();
            if (data.Count > 0)
            {
                result = data.Select(i => i.Index).ToList();
                max = result.Max(i => i);
                if (string.IsNullOrEmpty(id))
                {
                    result.Add(max + 1);
                }
            }
            else
            {
                result.Add(max);
            }

            return result;
        }

        /// <summary>
        /// Lấy list data theo tên bảng
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<List<CategoryTableDataModel>> GetListDataWidthTable(string tableName)
        {
            List<CategoryTableDataModel> categorys = new List<CategoryTableDataModel>();
            using (DbCommand cmd = _sqlContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM [dbo].[" + tableName.NTSTrim() + "]";
                await _sqlContext.Database.OpenConnectionAsync();
                using (DbDataReader ddr = cmd.ExecuteReader())
                {
                    while (ddr.Read())
                    {
                        categorys.Add(new CategoryTableDataModel
                        {
                            Id = ddr["Id"].ToString(),
                            Name = ddr["Name"].ToString(),
                            Index = Convert.ToInt32(ddr["Index"].ToString())
                        });
                    }
                }
            }

            return categorys;
        }

        public List<CategoryTableDataModel> GetListDataWidthTableClient(string tableName)
        {
            List<CategoryTableDataModel> categorys = new List<CategoryTableDataModel>();
            using (DbCommand cmd = _sqlContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM [dbo].[" + tableName.NTSTrim() + "]";
                _sqlContext.Database.OpenConnection();
                using (DbDataReader ddr = cmd.ExecuteReader())
                {
                    while (ddr.Read())
                    {
                        categorys.Add(new CategoryTableDataModel
                        {
                            Id = ddr["Id"].ToString(),
                            Name = ddr["Name"].ToString(),
                            Index = Convert.ToInt32(ddr["Index"].ToString())
                        });
                    }
                }
            }

            return categorys;
        }
    }
}
