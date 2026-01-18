using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NTS.Common;
using NTS_ERP.Models.Cores.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Combobox;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Models.Cores.SystemFunction;
using NTS_ERP.Models.Entities;
using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NTS_ERP.Services.Cores.SystemFunction
{
    public class SystemFunctionService : ISystemFunctionService
    {
        private readonly ConnectionStringModel _connectionStringSql;
        public DbConnection _dbConnection = null;
        private readonly NTS_ERPContext _sqlContext;


        public SystemFunctionService(NTS_ERPContext sqlContext,IOptions<ConnectionStringModel> connectionString)
        {
            this._sqlContext = sqlContext;
            _connectionStringSql = connectionString.Value;
            _dbConnection = new SqlConnection(_connectionStringSql.DefaultConnection);
        }

        #region Cấu hình chức năng tự động
        /// <summary>
        /// Tìm kiếm cấu hình chức năng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<FuntionConfigSearchResultModel>> SearchConfig(FuntionConfigSearchModel searchModel)
        {
            SearchBaseResultModel<FuntionConfigSearchResultModel> searchResult = new SearchBaseResultModel<FuntionConfigSearchResultModel>();

            var dataQuery = (from a in _sqlContext.Nts_SystemFunctionConfig.AsEnumerable()
                             select new FuntionConfigSearchResultModel()
                             {
                                 Id = a.Id,
                                 FunctionName = a.FunctionName
                             }).AsQueryable();

            searchResult.TotalItems = dataQuery.Count();

            searchResult.DataResults = dataQuery.ToList();
            return searchResult;
        }

        /// <summary>
        /// Thêm mới cấu hình chức năng tự động
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Id chức năng sau khi thêm mới thành công</returns>
        public async Task<string> CreateFuntionConfig(SystemFunctionConfigModel model)
        {
            model.Slug = model.FunctionName.GenerateSlug();
            var functionConfigModel = JsonConvert.DeserializeObject<Nts_SystemFunctionConfig>(JsonConvert.SerializeObject(model));
            functionConfigModel.Id = Guid.NewGuid().ToString();

            _sqlContext.Nts_SystemFunctionConfig.Add(functionConfigModel);

            if (model.FunctionDesigns?.Count > 0)
            {
                Nts_SystemFunctionDesign functionDesignAdd;
                foreach (var itemFunctionDesign in model.FunctionDesigns)
                {
                    functionDesignAdd = JsonConvert.DeserializeObject<Nts_SystemFunctionDesign>(JsonConvert.SerializeObject(itemFunctionDesign));
                    functionDesignAdd.SystemFunctionConfigId = functionConfigModel.Id;
                    functionDesignAdd.Id = Guid.NewGuid().ToString();
                    _sqlContext.Nts_SystemFunctionDesign.Add(functionDesignAdd);
                }
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();

                    GlobalData.ListFunctionConfig = _sqlContext.Nts_SystemFunctionConfig.ToList();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return functionConfigModel.Id;
        }

        /// <summary>
        /// Lấy thông tin cập nhật cấu hình chức năng tự động
        /// </summary>
        /// <param name="id">Id chức năng tự động</param>
        /// <returns></returns>
        public async Task<FunctionConfigUpdateModel> GetFuntionConfigUpdateById(string id)
        {
            var item = _sqlContext.Nts_SystemFunctionConfig.AsNoTracking().FirstOrDefault(s => s.Id.Equals(id));
            if (item == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var inputModel = JsonConvert.DeserializeObject<FunctionConfigUpdateModel>(JsonConvert.SerializeObject(item));

            //Danh sách các trường trong bảng đa được cấu hình
            var functionDesigns = _sqlContext.Nts_SystemFunctionDesign.AsNoTracking().Where(s => s.SystemFunctionConfigId.Equals(id)).ToList();

            //Danh các Column trong bảng
            var columnNameConfigs = functionDesigns.Select(s => s.ColumnName).ToList();
            //Danh sách các trường trong bảng dữ liệu
            List<SystemFunctionDesignModel> listColumnTable = this.GetColumnTableAsync(item.TableName).Result;
            //Danh sách các trường chưa được cầu hình
            var listColumnTableNotConfig = listColumnTable.Where(s => !columnNameConfigs.Contains(s.ColumnName)).ToList();
            //Add các trường chưa được cấu hình vào
            if (listColumnTableNotConfig.Count > 0)
            {
                functionDesigns.AddRange(JsonConvert.DeserializeObject<List<Nts_SystemFunctionDesign>>(JsonConvert.SerializeObject(listColumnTableNotConfig)));
            }

            //Loại bỏ các trường đã được cấu hình nay không còn tồn tại trong bảng dữ liệu
            if (listColumnTable.Count() != functionDesigns.Count())
            {
                var columnTables = listColumnTable.Select(s => s.ColumnName);
                functionDesigns = functionDesigns.Where(s => columnTables.Contains(s.ColumnName)).ToList();
            }

            inputModel.FunctionDesigns = JsonConvert.DeserializeObject<List<SystemFunctionDesignModel>>(JsonConvert.SerializeObject(functionDesigns));
            return inputModel;
        }

        /// <summary>
        /// Cập nhật cấu hình chức năng tự động
        /// </summary>
        /// <param name="id">Id bản ghi cập nhật</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateFuntionConfig(string id, FunctionConfigUpdateModel model)
        {
            var itemUpdate = _sqlContext.Nts_SystemFunctionConfig.FirstOrDefault(s => s.Id.Equals(id));
            if (itemUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            model.Slug = model.FunctionName.GenerateSlug();

            _sqlContext.Entry<Nts_SystemFunctionConfig>(itemUpdate).CurrentValues.SetValues(model);

            var functionConfigDelete = _sqlContext.Nts_SystemFunctionDesign.Where(s => s.SystemFunctionConfigId.Equals(id)).ToList();
            if (functionConfigDelete.Any())
            {
                _sqlContext.Nts_SystemFunctionDesign.RemoveRange(functionConfigDelete);
            }

            if (model.FunctionDesigns?.Count > 0)
            {
                Nts_SystemFunctionDesign functionDesignAdd;
                foreach (var itemFunctionDesign in model.FunctionDesigns)
                {
                    functionDesignAdd = JsonConvert.DeserializeObject<Nts_SystemFunctionDesign>(JsonConvert.SerializeObject(itemFunctionDesign));
                    functionDesignAdd.SystemFunctionConfigId = id;
                    functionDesignAdd.Id = Guid.NewGuid().ToString();
                    _sqlContext.Nts_SystemFunctionDesign.Add(functionDesignAdd);
                }
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();

                    GlobalData.ListFunctionConfig = _sqlContext.Nts_SystemFunctionConfig.ToList();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return true;
        }

        /// <summary>
        /// Cập nhật cấu hình chức năng tự động
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteFuntionConfig(string id)
        {
            var itemDelete = _sqlContext.Nts_SystemFunctionConfig.FirstOrDefault(s => s.Id.Equals(id));
            if (itemDelete == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            var functionConfigDelete = _sqlContext.Nts_SystemFunctionDesign.Where(s => s.SystemFunctionConfigId.Equals(id)).ToList();

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    if (functionConfigDelete.Any())
                    {
                        _sqlContext.Nts_SystemFunctionDesign.RemoveRange(functionConfigDelete);
                    }
                    _sqlContext.Nts_SystemFunctionConfig.Remove(itemDelete);
                    _sqlContext.SaveChanges();
                    trans.Commit();

                    GlobalData.ListFunctionConfig = _sqlContext.Nts_SystemFunctionConfig.ToList();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return true;
        }

        /// <summary>
        /// Lấy cấu hình giao diện của một chức năng động
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public async Task<FunctionConfigViewModel> GetFuntionConfigBySlug(string slugorid)
        {
            var itemFunctionConfig = _sqlContext.Nts_SystemFunctionConfig.AsNoTracking().FirstOrDefault(s => s.Slug.Equals(slugorid) || s.Id.Equals(slugorid));
            if (itemFunctionConfig == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0004);
            }

            var resultModel = JsonConvert.DeserializeObject<FunctionConfigViewModel>(JsonConvert.SerializeObject(itemFunctionConfig));

            var functionDesigns = _sqlContext.Nts_SystemFunctionDesign.AsNoTracking().Where(s => s.SystemFunctionConfigId.Equals(itemFunctionConfig.Id)).ToList();
            resultModel.ColumnDesigns = JsonConvert.DeserializeObject<List<FunctionDesignColumnViewModel>>(JsonConvert.SerializeObject(functionDesigns)).OrderBy(o => o.ColumnIndex).ToList();
            resultModel.SearchDesigns = JsonConvert.DeserializeObject<List<FunctionDesignSearchViewModel>>(JsonConvert.SerializeObject(functionDesigns)).Where(r => r.SearchDisplay).OrderBy(o => o.ColumnIndex).ToList();
            resultModel.FilterDesigns = JsonConvert.DeserializeObject<List<FunctionDesignFilterViewModel>>(JsonConvert.SerializeObject(functionDesigns)).Where(r => r.FilterDisplay).OrderBy(o => o.FilterIndex).ToList();
            resultModel.CreateDesigns = JsonConvert.DeserializeObject<List<FunctionDesignCreateViewModel>>(JsonConvert.SerializeObject(functionDesigns)).Where(r => r.CreateDisplay).OrderBy(o => o.DivCreateIndex).ToList();
            resultModel.EditDesigns = JsonConvert.DeserializeObject<List<FunctionDesignEditViewModel>>(JsonConvert.SerializeObject(functionDesigns)).Where(r => r.EditDisplay).OrderBy(o => o.DivEditIndex).ToList();
            resultModel.DetailDesigns = JsonConvert.DeserializeObject<List<FunctionDesignDetailViewModel>>(JsonConvert.SerializeObject(functionDesigns)).Where(r => r.DetailDisplay).OrderBy(o => o.DivDetailIndex).ToList();
            if (resultModel.CreateDesigns != null)
            {
                foreach (var item in resultModel.CreateDesigns)
                {
                    if (item.IsLink)
                    {
                        item.LinkId = JsonNamingPolicy.CamelCase.ConvertName(item.LinkId);
                        item.LinkName = JsonNamingPolicy.CamelCase.ConvertName(item.LinkName);
                        //Nếu có link tới bảng thì lấy dữ liệu từ bảng ngược lại lấy từ data json fix
                        item.LinkData = !string.IsNullOrEmpty(item.LinkTable) ? this.GetDataLink(item.LinkTable, item.LinkId, item.LinkName) : JsonConvert.DeserializeObject<List<ComboboxBaseModel>>(item.LinkDataJson);
                    }
                }
            }

            if (resultModel.EditDesigns != null)
            {
                foreach (var item in resultModel.EditDesigns)
                {
                    if (item.IsLink)
                    {
                        item.LinkId = JsonNamingPolicy.CamelCase.ConvertName(item.LinkId);
                        item.LinkName = JsonNamingPolicy.CamelCase.ConvertName(item.LinkName);
                        //Nếu có link tới bảng thì lấy dữ liệu từ bảng ngược lại lấy từ data json fix
                        item.LinkData = !string.IsNullOrEmpty(item.LinkTable) ? this.GetDataLink(item.LinkTable, item.LinkId, item.LinkName) : JsonConvert.DeserializeObject<List<ComboboxBaseModel>>(item.LinkDataJson);
                    }
                }
            }

            if (resultModel.FilterDesigns != null)
            {
                foreach (var item in resultModel.FilterDesigns)
                {
                    if (item.IsLink)
                    {
                        item.LinkId = JsonNamingPolicy.CamelCase.ConvertName(item.LinkId);
                        item.LinkName = JsonNamingPolicy.CamelCase.ConvertName(item.LinkName);
                        //Nếu có link tới bảng thì lấy dữ liệu từ bảng ngược lại lấy từ data json fix
                        item.LinkData = !string.IsNullOrEmpty(item.LinkTable) ? this.GetDataLink(item.LinkTable, item.LinkId, item.LinkName) : JsonConvert.DeserializeObject<List<ComboboxBaseModel>>(item.LinkDataJson);
                    }
                }
            }
            return resultModel;
        }
        #endregion

        #region Các hàm xử lý năng tự động

        /// <summary>
        /// Tìm kiếm dữ liệu cho chức năng tự động
        /// </summary>
        /// <param name="slug">Slug chức năng</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<object>> SearchDataAsync(string slug, object model)
        {
            FunctionConfigViewModel configViewModel = this.GetFuntionConfigBySlug(slug).Result;

            SearchBaseResultModel<object> resultModel = new SearchBaseResultModel<object>();

            //Chuyển object về sring json
            var json = JsonConvert.SerializeObject(model);
            //Chuyển string json về Dictionary
            var listColumnSearch = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            //Lấy các thông tin trong bảng
            var columnProperties = await GetColumnProperties(configViewModel.TableName);

            //List các column select
            Dictionary<string, ColumnSelectModel> typeColumns = new Dictionary<string, ColumnSelectModel>();
            ColumnSelectModel columnSelect;
            FunctionDesignSearchViewModel functionDesign;
            foreach (var property in columnProperties)
            {
                columnSelect = new ColumnSelectModel();
                columnSelect.TypeColumn = property;
                if (property.IsPrimaryKey)//Nếu là khóa chính
                {
                    columnSelect.IsPrimaryKey = true;
                    typeColumns.Add(property.ColumnName, columnSelect);
                }
                else
                {
                    columnSelect.IsPrimaryKey = false;
                    functionDesign = configViewModel.SearchDesigns.FirstOrDefault(a => a.ColumnName.Equals(property.ColumnName));
                    if (functionDesign != null)
                    {
                        if (functionDesign.IsLink)
                        {
                            //Nếu có link tới bảng thì lấy dữ liệu từ bảng ngược lại lấy từ data json fix
                            columnSelect.LinkData = !string.IsNullOrEmpty(functionDesign.LinkTable) ? this.GetDataLink(functionDesign.LinkTable, functionDesign.LinkId, functionDesign.LinkName) : JsonConvert.DeserializeObject<List<ComboboxBaseModel>>(functionDesign.LinkDataJson);
                        }
                        typeColumns.Add(property.ColumnName, columnSelect);
                    }
                }
            }

            List<object> dataResult = new List<object>();
            Dictionary<string, object> dicKeyData = new Dictionary<string, object>();
            string commandText = "";
            await _dbConnection.OpenAsync();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                //Mệnh đề select
                commandText = $"SELECT [Columns] FROM [dbo].[{configViewModel.TableName}]";

                //Add mệnh đề where
                KeyValuePair<string, object> itemFilter;
                bool isWhere = false;
                foreach (var item in typeColumns)
                {
                    itemFilter = listColumnSearch.FirstOrDefault(s => s.Key.Equals(item.Key));
                    if (!string.IsNullOrEmpty(itemFilter.Key) && !string.IsNullOrEmpty(itemFilter.Value?.ToString() ?? ""))
                    {
                        if ("Int".Equals(item.Value.TypeColumn.DataType) || "Float".Equals(item.Value.TypeColumn.DataType) || "Double".Equals(item.Value.TypeColumn.DataType)
                            || "Decimal".Equals(item.Value.TypeColumn.DataType) || "Bool".Equals(item.Value.TypeColumn.DataType))
                        {
                            commandText += $" {(isWhere ? "AND" : "WHERE")} {item.Key} = '{itemFilter.Value}'";
                        }
                        else if ("DateTime".Equals(item.Value.TypeColumn.DataType))
                        {
                            KeyValuePair<string, object> itemFilterFrom = listColumnSearch.FirstOrDefault(s => s.Key.Equals(item.Key + "From"));
                            KeyValuePair<string, object> itemFilterTo = listColumnSearch.FirstOrDefault(s => s.Key.Equals(item.Key + "To"));

                            if (!string.IsNullOrEmpty(itemFilterFrom.Key) && !string.IsNullOrEmpty(itemFilterFrom.Value?.ToString() ?? ""))
                            {
                                commandText += $" {(isWhere ? "AND" : "WHERE")} {item.Key} >= '{(itemFilterFrom.Value?.ToString() ?? "").ToStartDate("yyyy-M-d")}'";
                            }
                            if (!string.IsNullOrEmpty(itemFilterTo.Key) && !string.IsNullOrEmpty(itemFilterTo.Value?.ToString() ?? ""))
                            {
                                commandText += $" {(isWhere ? "AND" : "WHERE")} {item.Key} <= '{(itemFilterTo.Value?.ToString() ?? "").ToEndDate("yyyy-M-d")}'";
                            }
                        }
                        else
                        {
                            //Trường hợp quan giao diện kiểu treeview
                            if (configViewModel.LayoutType == NTSConstants.LayoutTypeTreeview && configViewModel.ColumnDesigns.FirstOrDefault(s => s.ColumnName.Equals(item.Key))?.LinkTable?.Equals(configViewModel.TreeTableName) == true)
                            {
                                //Chuyển object về sring json
                                var jsonTreeviewId = JsonConvert.SerializeObject(itemFilter.Value);
                                //Chuyển string json về Dictionary
                                var listTreeviewId = JsonConvert.DeserializeObject<List<string>>(jsonTreeviewId);
                                if (listTreeviewId != null && listTreeviewId.Count() > 0)
                                {
                                    listTreeviewId.Contains(configViewModel.TreeTableName);
                                    commandText += $" {(isWhere ? "AND" : "WHERE")} {item.Key} IN ('" + string.Join("', '", listTreeviewId) + "')";
                                }
                            }
                            else
                            {
                                commandText += $" {(isWhere ? "AND" : "WHERE")} {item.Key} LIKE '%{itemFilter.Value}%'";
                            }
                        }
                    }
                    isWhere = commandText.Contains(" WHERE ");
                }

                //Tìm theo Keyword
                var itemSearchKeyword = listColumnSearch.FirstOrDefault(s => s.Key.Equals("searchKeyword"));
                if (!string.IsNullOrEmpty(itemSearchKeyword.Key) && !string.IsNullOrEmpty(itemSearchKeyword.Value?.ToString() ?? ""))
                {
                    string commandSearchKeyword = "";
                    foreach (var item in typeColumns)
                    {
                        if (!item.Value.IsPrimaryKey)
                            commandSearchKeyword += $" {item.Key} LIKE '%{itemSearchKeyword.Value}%' OR";
                    }
                    if (!string.IsNullOrEmpty(commandSearchKeyword))
                    {
                        commandText += $" {(isWhere ? "AND" : "WHERE")} ( {commandSearchKeyword.Remove(commandSearchKeyword.Length - 3, 3)} ) ";
                    }
                }

                //Add mệnh đề count
                command.CommandText = commandText.Replace("[Columns]", "COUNT(Id)");
                using (DbDataReader ddr = command.ExecuteReader())
                {
                    while (ddr.Read())
                    {
                        resultModel.TotalItems = ddr.GetInt32(0);
                    }
                }

                //Order dữ liệu
                commandText += " order by Id desc";

                //Phân trang
                int.TryParse(listColumnSearch.Where(s => s.Key.Equals("pageNumber")).FirstOrDefault().Value?.ToString() ?? "0", out int pageNumber);
                int.TryParse(listColumnSearch.Where(s => s.Key.Equals("pageSize")).FirstOrDefault().Value?.ToString() ?? "10", out int pageSize);

                if (pageNumber > 0)
                {
                    commandText += $" OFFSET {(pageNumber - 1) * pageSize} ROWS";
                }

                if (pageSize > 0)
                {
                    commandText += $" FETCH NEXT {pageSize} ROWS ONLY";
                }

                //Add column select
                command.CommandText = commandText.Replace("[Columns]", string.Join(", ", typeColumns.Select(s => $"[{s.Key}]")));

                using (DbDataReader ddr = command.ExecuteReader())
                {
                    while (ddr.Read())
                    {
                        dicKeyData = new Dictionary<string, object>();
                        foreach (var item in typeColumns)
                        {
                            dicKeyData.Add(item.Key, this.GetDataValue(ddr, item.Key, item.Value, true));
                        }
                        string tem = JsonConvert.SerializeObject(dicKeyData);

                        dataResult.Add(JsonConvert.DeserializeObject<object>(tem));
                    }
                }
            }

            await _dbConnection.CloseAsync();

            resultModel.DataResults = dataResult;

            return resultModel;
        }
        /// <summary>
        /// Thêm mới bản ghi cho chức năng
        /// </summary>
        /// <param name="slug">Slug chức năng</param>
        /// <returns>Id bản gi thêm mới</returns>
        public async Task<string> CreateDataAsync(string slug, object model, string userId)
        {
            FunctionConfigViewModel configViewModel = this.GetFuntionConfigBySlug(slug).Result;

            //Tạo khóa chính cho bảng
            string parentKey = "";
            DateTime dateNow = DateTime.Now;

            //Chuyển object về sring json
            var json = JsonConvert.SerializeObject(model);
            //Chuyển string json về Dictionary
            var listInput = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //Lấy các thông tin trong bảng
            var columnProperties = await GetColumnProperties(configViewModel.TableName);

            //List các column thêm mới
            List<ColumnPropertiesModel> columnsAdd = new List<ColumnPropertiesModel>();
            //List<string> columns = new List<string>();
            ////List các giá trị thêm mới
            //List<object> values = new List<object>();
            KeyValuePair<string, string> objectInput;
            foreach (var property in columnProperties)
            {
                if (property.IsPrimaryKey && !property.IsIdentity)//Nếu là khóa chính & không tự tăng thì tạo khóa mới
                {
                    parentKey = Guid.NewGuid().ToString();
                    property.Value = parentKey;
                    columnsAdd.Add(property);
                }
                else if (property.ColumnName.Equals("CreateBy") || property.ColumnName.Equals("UpdateBy")) //Nếu là trường thông tin người tạo, cập nhật thì ste dằng id login
                {
                    property.Value = userId;
                    columnsAdd.Add(property);
                }
                else if (property.ColumnName.Equals("CreateDate") || property.ColumnName.Equals("UpdateDate")) //Nếu trường là ngày tạo, cập nhật thì sét thời gian hiện tại
                {
                    property.Value = dateNow.ToStringFullDateTime();
                    columnsAdd.Add(property);
                }
                else if (configViewModel.CreateDesigns.Any(a => a.ColumnName.Equals(property.ColumnName)))//Tồn tại trường trong cấu hình thêm mới
                {
                    objectInput = listInput.Where(s => s.Key.Equals(property.ColumnName)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(objectInput.Key))
                    {
                        if ("String".Equals(property.DataType))//Nếu là kiểu string thì thêm N để lưu tiếng việt có dấu
                        {
                            property.Value = objectInput.Value;
                        }
                        else
                        {
                            property.Value = string.IsNullOrEmpty(objectInput.Value) || objectInput.Value == null ? "NULL" : objectInput.Value;
                        }
                        columnsAdd.Add(property);
                    }
                }
            }

            await _dbConnection.OpenAsync();
            // Bắt đầu giao dịch
            DbTransaction trans = _dbConnection.BeginTransaction();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                command.Transaction = trans;
                try
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"INSERT INTO [dbo].[{configViewModel.TableName}] ({string.Join(", ", columnsAdd.Select(p => $"[{p.ColumnName}]"))}) VALUES ({string.Join(", ", columnsAdd.Select(p => $"@{p.ColumnName}"))})";

                    DbParameter parameter;
                    foreach (var column in columnsAdd)
                    {
                        parameter = command.CreateParameter();
                        parameter.ParameterName = $"@{column.ColumnName}";
                        parameter.Value = column.Value;
                        command.Parameters.Add(parameter);
                    }
                    //Tạo câu lệnh và thực hiện 
                    //var commandInsert = $"INSERT INTO [dbo].[{configViewModel.TableName}] ({string.Join(", ", columns.Select(col => $"[{col}]"))}) VALUES ({string.Join(", ", values)})";
                    int rowsAffected = command.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            await _dbConnection.CloseAsync();

            return parentKey;
        }

        /// <summary>
        /// Lấy bản ghi cập nhật
        /// </summary>
        /// <param name="slug">Slug chức năng</param>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public async Task<object> GetUpdateDataAsync(string slug, string id)
        {
            FunctionConfigViewModel configViewModel = this.GetFuntionConfigBySlug(slug).Result;

            object resultModel = null;

            //Lấy các thông tin trong bảng
            var columnProperties = await GetColumnProperties(configViewModel.TableName);

            //List các column select
            string primaryKeyId = "";
            Dictionary<string, ColumnSelectModel> typeColumns = new Dictionary<string, ColumnSelectModel>();
            ColumnSelectModel columnSelect;
            FunctionDesignEditViewModel functionDesign;
            foreach (var property in columnProperties)
            {
                columnSelect = new ColumnSelectModel();
                columnSelect.TypeColumn = property;
                if (property.IsPrimaryKey)//Nếu là khóa chính
                {
                    typeColumns.Add(property.ColumnName, columnSelect);
                    primaryKeyId = property.ColumnName;
                }
                else
                {
                    functionDesign = configViewModel.EditDesigns.FirstOrDefault(a => a.ColumnName.Equals(property.ColumnName));
                    if (functionDesign != null)
                    {
                        if (functionDesign.IsLink)
                        {
                            //Nếu có link tới bảng thì lấy dữ liệu từ bảng ngược lại lấy từ data json fix
                            columnSelect.LinkData = !string.IsNullOrEmpty(functionDesign.LinkTable) ? this.GetDataLink(functionDesign.LinkTable, functionDesign.LinkId, functionDesign.LinkName) : JsonConvert.DeserializeObject<List<ComboboxBaseModel>>(functionDesign.LinkDataJson);
                        }
                        typeColumns.Add(property.ColumnName, columnSelect);
                    }
                }
            }

            await _dbConnection.OpenAsync();
            Dictionary<string, object> dicKeyData = new Dictionary<string, object>();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT {string.Join(", ", typeColumns.Select(s => $"[{s.Key}]"))} FROM [dbo].[{configViewModel.TableName}] WHERE [{primaryKeyId}] = @{primaryKeyId}";

                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = $"@{primaryKeyId}";
                parameter.Value = id;
                command.Parameters.Add(parameter);

                using (DbDataReader ddr = command.ExecuteReader())
                {
                    while (ddr.Read())
                    {
                        dicKeyData = new Dictionary<string, object>();
                        foreach (var item in typeColumns)
                        {
                            dicKeyData.Add(item.Key, GetDataValue(ddr, item.Key, item.Value, false));
                        }
                        string tem = JsonConvert.SerializeObject(dicKeyData);

                        resultModel = JsonConvert.DeserializeObject<object>(tem);
                    }
                }
            }
            await _dbConnection.CloseAsync();

            return resultModel;
        }

        /// <summary>
        /// Cập nhật bản ghi cho chức năng
        /// </summary>
        /// <param name="slug">Slug chức năng</param>
        /// <returns>Id bản gi cập nhật</returns>
        public async Task<bool> UpdateDataAsync(string slug, string id, object model, string userId)
        {
            FunctionConfigViewModel configViewModel = this.GetFuntionConfigBySlug(slug).Result;

            object itemUpdate = this.GetUpdateDataAsync(slug, id);
            if (itemUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            //Tạo khóa chính cho bảng
            string parentKeyName = "";
            DateTime dateNow = DateTime.Now;

            //Chuyển object về sring json
            var json = JsonConvert.SerializeObject(model);
            //Chuyển string json về Dictionary
            var listInput = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //Lấy các thông tin trong bảng
            var columnProperties = await GetColumnProperties(configViewModel.TableName);

            KeyValuePair<string, string> objectInput;
            Dictionary<string, object> dicKeyData = new Dictionary<string, object>();
            foreach (var property in columnProperties)
            {
                if (property.IsPrimaryKey)//Nếu là khóa chính thì tạo khóa mới
                {
                    parentKeyName = property.ColumnName;
                }
                else if ("UpdateBy".Equals(property.ColumnName)) //Nếu là trường thông tin người tạo, cập nhật thì ste dằng id login
                {
                    dicKeyData.Add(property.ColumnName, userId);
                }
                else if ("UpdateDate".Equals(property.ColumnName)) //Nếu trường là ngày tạo, cập nhật thì sét thời gian hiện tại
                {
                    dicKeyData.Add(property.ColumnName, dateNow.ToStringFullDateTime());
                }
                else if (configViewModel.EditDesigns.Any(a => a.ColumnName.Equals(property.ColumnName)))
                {
                    objectInput = listInput.Where(s => s.Key.Equals(property.ColumnName)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(objectInput.Key))
                    {
                        if ("String".Equals(property.DataType))//Nếu là kiểu string thì thêm N để lưu tiếng việt có dấu
                        {
                            dicKeyData.Add(property.ColumnName, objectInput.Value);
                        }
                        else
                        {
                            dicKeyData.Add(property.ColumnName, string.IsNullOrEmpty(objectInput.Value) || objectInput.Value == null ? "NULL" : objectInput.Value);
                        }
                    }
                }
            }

            await _dbConnection.OpenAsync();
            // Bắt đầu giao dịch
            DbTransaction trans = _dbConnection.BeginTransaction();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                command.Transaction = trans;
                try
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"UPDATE [dbo].[{configViewModel.TableName}]  SET {string.Join(", ", dicKeyData.Select(s => $"[{s.Key}] = @{s.Key}").ToList())} WHERE [{parentKeyName}] = '{id}'";

                    DbParameter parameter;
                    foreach (var column in dicKeyData)
                    {
                        parameter = command.CreateParameter();
                        parameter.ParameterName = $"@{column.Key}";
                        parameter.Value = column.Value;
                        command.Parameters.Add(parameter);
                    }

                    //Tạo câu lệnh và thực hiện 
                    //var commandUpdate = $"UPDATE [dbo].[{configViewModel.TableName}]  SET {string.Join(", ", dicKeyData.Select(s => $"[{s.Key}] = {s.Value}").ToList())} WHERE [{parentKeyName}] = '{id}'";
                    //_sqlContext.Database.ExecuteSqlRaw(commandUpdate);
                    int rowsAffected = command.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

            return true;
        }

        /// <summary>
        /// Lấy thông tin chi tiết
        /// </summary>
        /// <param name="slug">Slug chức năng</param>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public async Task<object> GetDetailDataAsync(string slug, string id)
        {
            FunctionConfigViewModel configViewModel = this.GetFuntionConfigBySlug(slug).Result;

            object resultModel = null;

            //Lấy các thông tin trong bảng
            var columnProperties = await GetColumnProperties(configViewModel.TableName);

            //List các column select
            string primaryKeyId = "";
            Dictionary<string, ColumnSelectModel> typeColumns = new Dictionary<string, ColumnSelectModel>();
            ColumnSelectModel columnSelect;
            FunctionDesignDetailViewModel functionDesign;
            foreach (var property in columnProperties)
            {
                columnSelect = new ColumnSelectModel();
                columnSelect.TypeColumn = property;
                if (property.IsPrimaryKey)//Nếu là khóa chính thì tạo khóa mới
                {
                    typeColumns.Add(property.ColumnName, columnSelect);
                    primaryKeyId = property.ColumnName;
                }
                else
                {
                    functionDesign = configViewModel.DetailDesigns.FirstOrDefault(a => a.ColumnName.Equals(property.ColumnName));
                    if (functionDesign != null)
                    {
                        //columnSelect.FunctionDesignEdit = functionDesign;
                        if (functionDesign.IsLink)
                        {
                            //Nếu có link tới bảng thì lấy dữ liệu từ bảng ngược lại lấy từ data json fix
                            columnSelect.LinkData = !string.IsNullOrEmpty(functionDesign.LinkTable) ? this.GetDataLink(functionDesign.LinkTable, functionDesign.LinkId, functionDesign.LinkName) : JsonConvert.DeserializeObject<List<ComboboxBaseModel>>(functionDesign.LinkDataJson);
                        }
                        typeColumns.Add(property.ColumnName, columnSelect);
                    }
                }
            }

            Dictionary<string, object> dicKeyData = new Dictionary<string, object>();
            await _dbConnection.OpenAsync();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT {string.Join(", ", typeColumns.Select(s => $"[{s.Key}]"))} FROM [dbo].[{configViewModel.TableName}] WHERE [{primaryKeyId}] = @{primaryKeyId}";

                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = $"@{primaryKeyId}";
                parameter.Value = id;
                command.Parameters.Add(parameter);

                using (DbDataReader ddr = command.ExecuteReader())
                {
                    while (ddr.Read())
                    {
                        dicKeyData = new Dictionary<string, object>();
                        foreach (var item in typeColumns)
                        {
                            dicKeyData.Add(item.Key, GetDataValue(ddr, item.Key, item.Value, true));
                        }
                        string tem = JsonConvert.SerializeObject(dicKeyData);

                        resultModel = JsonConvert.DeserializeObject<object>(tem);
                    }
                }
            }
            await _dbConnection.CloseAsync();

            return resultModel;
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteDataAsync(string slug, string id, string userid)
        {
            FunctionConfigViewModel configViewModel = this.GetFuntionConfigBySlug(slug).Result;

            DataTable data = await GetDataWidthTable(configViewModel.TableName, id);

            var itemExits = data.DefaultView.Table.AsEnumerable().ToList();
            //Kiểm tra tồn tại bản ghi
            if (itemExits.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.ERR0003);
            }

            //Lấy các thông tin trong bảng
            var columnProperties = await GetColumnProperties(configViewModel.TableName);

            //List các column select
            string primaryKeyId = columnProperties.Where(s => s.IsPrimaryKey).FirstOrDefault()?.ColumnName ?? "";

            await _dbConnection.OpenAsync();
            // Bắt đầu giao dịch
            DbTransaction trans = _dbConnection.BeginTransaction();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                try
                {
                    command.Transaction = trans;
                    //Tạo câu lệnh và thực hiện 
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"DELETE FROM [{configViewModel.TableName}] WHERE [{primaryKeyId}] = @{primaryKeyId};";

                    DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = $"@{primaryKeyId}";
                    parameter.Value = id;
                    command.Parameters.Add(parameter);

                    int rowsAffected = command.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            await _dbConnection.CloseAsync();
            return true;
        }
        #endregion

        #region Các hàm xử lý tree view trong danh sách quản lý
        /// <summary>
        /// Lấy dữ liệu cây thư mục
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public async Task<List<TreeViewModel>> TreeViewDataAsync(string slug)
        {
            FunctionConfigViewModel configViewModel = this.GetFuntionConfigBySlug(slug).Result;

            //Danh sách column data sử dụng cho tree view
            List<string> columnsNameUseTree = new List<string>();
            columnsNameUseTree.Add(configViewModel.TreeColumnId);
            columnsNameUseTree.Add(configViewModel.TreeColumnParentId);
            columnsNameUseTree.AddRange(configViewModel.TreeColumnsText.Split(";").ToList());

            //Lấy các thông tin trong bảng
            var columnProperties = await GetColumnProperties(configViewModel.TableName);

            //List các column select
            Dictionary<string, ColumnSelectModel> typeColumns = new Dictionary<string, ColumnSelectModel>();
            ColumnSelectModel columnSelect;
            foreach (var property in columnProperties)
            {
                columnSelect = new ColumnSelectModel();
                columnSelect.TypeColumn = property;
                if (columnsNameUseTree.Contains(property.ColumnName))
                {
                    typeColumns.Add(property.ColumnName, columnSelect);
                }
            }

            List<TreeViewModel> dataResult = new List<TreeViewModel>();
            await _dbConnection.OpenAsync();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                //Add column select
                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT {string.Join(", ", typeColumns.Select(s => $"[{s.Key}]"))} FROM [dbo].[{configViewModel.TreeTableName}]";

                using (DbDataReader ddr = command.ExecuteReader())
                {
                    List<string> textTree = configViewModel.TreeColumnsText.Split(";").ToList();
                    TreeViewModel treeViewModel;
                    while (ddr.Read())
                    {
                        treeViewModel = new TreeViewModel();
                        foreach (var item in typeColumns)
                        {
                            var dataValue = this.GetDataValue(ddr, item.Key, item.Value, true);
                            if (item.Key.Equals(configViewModel.TreeColumnId))
                            {
                                treeViewModel.Id = dataValue?.ToString() ?? "";
                            }
                            else if (item.Key.Equals(configViewModel.TreeColumnParentId))
                            {
                                treeViewModel.ParentId = string.IsNullOrEmpty(dataValue?.ToString() ?? null) ? null : dataValue.ToString();
                            }
                            else if (textTree.Contains(item.Key))
                            {
                                treeViewModel.Text = !string.IsNullOrEmpty(dataValue?.ToString() ?? "") && !string.IsNullOrEmpty(treeViewModel.Text) ? $"{treeViewModel.Text} - {dataValue?.ToString()}" : treeViewModel.Text + dataValue?.ToString() ?? "";
                            }
                        }
                        dataResult.Add(treeViewModel);
                    }
                }
            }
            await _dbConnection.CloseAsync();

            return ConverToDataTree(dataResult);
        }
        #endregion

        #region Hàm lấy theo tác với csdl
        /// <summary>
        /// Danh sách table trên CSDL
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetTableNameAsync()
        {
            List<ComboboxModel> tableName = new List<ComboboxModel>();
            try
            {
                await _dbConnection.OpenAsync();
                using (DbCommand cmd = _dbConnection.CreateCommand())
                {
                    //Lệnh sql lấy danh sách bảng
                    cmd.CommandText = $"SELECT table_name FROM information_schema.tables WHERE table_catalog = '{_dbConnection.Database}' AND table_type = 'BASE TABLE';";
                    using (DbDataReader ddr = cmd.ExecuteReader())
                    {
                        string tableNameText = "";
                        ComboboxModel comboboxModel;
                        while (ddr.Read())
                        {
                            tableNameText = ddr.GetString(0);
                            comboboxModel = new ComboboxModel();
                            comboboxModel.Id = tableNameText;
                            comboboxModel.Name = tableNameText;
                            tableName.Add(comboboxModel);
                        }
                    }
                }

                tableName = tableName.Where(r => !r.Id.Equals("__EFMigrationsHistory")).Distinct().OrderBy(o => o.Name).ToList();
                await _dbConnection.CloseAsync();
            }
            catch (Exception ex)
            {
                tableName = new List<ComboboxModel>();
            }

            return tableName;
        }

        /// <summary>
        /// Danh sách column table trên CSDL
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemFunctionDesignModel>> GetColumnTableAsync(string tableName)
        {
            List<SystemFunctionDesignModel> functionDesign = new List<SystemFunctionDesignModel>();

            try
            {
                //Lấy thuộc tính trường dữ liệu trong bảng
                List<ColumnPropertiesModel> listProperties = await GetColumnProperties(tableName);
                SystemFunctionDesignModel functionDesignModel;
                foreach (var item in listProperties)
                {
                    functionDesignModel = new SystemFunctionDesignModel()
                    {
                        ColumnName = item.ColumnName,
                        Required = item.Required,
                        IsPrimaryKey = item.IsPrimaryKey,
                        IsUnicode = item.IsUnicode,
                        MaxLength = item.MaxLength,
                        DataType = item.DataType,
                        ColumnIndex = item.ColumnIndex,
                        DivCreateIndex = item.ColumnIndex,
                        CreateRequired = item.Required,
                        DivEditIndex = item.ColumnIndex,
                        EditRequired = item.Required,
                        FilterIndex = item.ColumnIndex
                    };
                    functionDesign.Add(functionDesignModel);
                }
            }
            catch (Exception ex)
            {
                functionDesign = new List<SystemFunctionDesignModel>();
            }

            return functionDesign.OrderBy(o => o.ColumnIndex).ToList();
        }

        /// <summary>
        /// Combox column table trên CSDL
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetComboxColumnTable(string tableName)
        {
            List<ComboboxModel> comboxColumns = new List<ComboboxModel>();

            try
            {
                //Lấy thuộc tính trường dữ liệu trong bảng
                List<ColumnPropertiesModel> listProperties = await GetColumnProperties(tableName);
                ComboboxModel comboboxModel;
                foreach (var item in listProperties)
                {
                    comboboxModel = new ComboboxModel()
                    {
                        Id = item.ColumnName,
                        Name = item.ColumnName
                    };
                    comboxColumns.Add(comboboxModel);
                }
            }
            catch (Exception ex)
            {
                comboxColumns = new List<ComboboxModel>();
            }

            return comboxColumns;
        }

        /// <summary>
        /// Lấy list data theo tên bảng
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private async Task<DataTable> GetDataWidthTable(string tableName, string id)
        {
            DataTable data = new DataTable();
            await _dbConnection.OpenAsync();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT * FROM [dbo].[{tableName}] where [Id] = '{id}'";
                using (DbDataReader ddr = command.ExecuteReader())
                {
                    data.Load(ddr);
                }
            }
            await _dbConnection.CloseAsync();

            return data;
        }

        private async Task<List<ColumnPropertiesModel>> GetColumnProperties(string tableName)
        {
            List<ColumnPropertiesModel> listProperties = new List<ColumnPropertiesModel>();
            await _dbConnection.OpenAsync();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandText = $"SELECT COLUMN_NAME,ORDINAL_POSITION, DATA_TYPE," +
                    $" CASE WHEN COLUMN_NAME IN (SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS 'KEY'," +
                    $" CASE WHEN IS_NULLABLE = 'NO' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS 'NOT_NULL'," +
                    $" CHARACTER_MAXIMUM_LENGTH AS 'MAX_LENGTH'," +
                    $" CASE WHEN CHARACTER_SET_NAME = 'UNICODE' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS 'IS_UNICODE'," +
                    $" CASE WHEN COLUMNPROPERTY(OBJECT_ID(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS 'IS_IDENTITY'" +
                    $" FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}';";
                using (DbDataReader ddr = command.ExecuteReader())
                {
                    ColumnPropertiesModel columnPropertiesModel;
                    string maxLength = "";
                    while (ddr.Read())
                    {
                        maxLength = ddr["MAX_LENGTH"].ToString() ?? "";

                        columnPropertiesModel = new ColumnPropertiesModel()
                        {
                            ColumnName = ddr["COLUMN_NAME"]?.ToString() ?? "",
                            Required = (bool)ddr["NOT_NULL"],
                            IsPrimaryKey = (bool)ddr["KEY"],
                            IsIdentity = (bool)ddr["IS_IDENTITY"],
                            IsUnicode = (Nullable<bool>)ddr["IS_UNICODE"],
                            MaxLength = string.IsNullOrEmpty(maxLength) ? (int?)null : (ddr["MAX_LENGTH"].ToString() == "-1" ? 0 : (int?)ddr["MAX_LENGTH"]),
                            DataType = ConvertDataTypeSqlToCSharp(ddr["DATA_TYPE"]?.ToString() ?? ""),
                            ColumnIndex = (int)ddr["ORDINAL_POSITION"]
                        };
                        listProperties.Add(columnPropertiesModel);
                    }
                }
            }
            await _dbConnection.CloseAsync();
            return listProperties;
        }

        /// <summary>
        /// Convert kiểu dữ liệu sql sang c#
        /// </summary>
        /// <param name="nameDataType"></param>
        /// <returns></returns>
        private string ConvertDataTypeSqlToCSharp(string nameDataType)
        {
            string dataType = "String";
            if (nameDataType.Equals("bigint"))
            {
                dataType = "Long";
            }
            else if (nameDataType.Equals("tinyint"))
            {
                dataType = "Byte";
            }
            else if (nameDataType.Equals("binary") || nameDataType.Equals("image") || nameDataType.Equals("timestamp") || nameDataType.Equals("varbinary"))
            {
                dataType = "ByteArray";
            }
            else if (nameDataType.Equals("bit"))
            {
                dataType = "Bool";
            }
            else if (nameDataType.Equals("char") || nameDataType.Equals("nchar") || nameDataType.Equals("ntext") || nameDataType.Equals("nvarchar") || nameDataType.Equals("text")
                || nameDataType.Equals("varchar") || nameDataType.Equals("xml"))
            {
                dataType = "String";
            }
            else if (nameDataType.Equals("date") || nameDataType.Equals("datetime") || nameDataType.Equals("datetime2") || nameDataType.Equals("datetimeoffset") || nameDataType.Equals("smalldatetime"))
            {
                dataType = "DateTime";
            }
            else if (nameDataType.Equals("decimal") || nameDataType.Equals("money") || nameDataType.Equals("numeric") || nameDataType.Equals("smallmoney"))
            {
                dataType = "Decimal";
            }
            else if (nameDataType.Equals("float"))
            {
                dataType = "Double";
            }
            else if (nameDataType.Equals("int"))
            {
                dataType = "Int";
            }
            else if (nameDataType.Equals("real"))
            {
                dataType = "Float";
            }
            else if (nameDataType.Equals("smallint"))
            {
                dataType = "Short";
            }
            else if (nameDataType.Equals("sql_variant"))
            {
                dataType = "Object";
            }
            else if (nameDataType.Equals("time"))
            {
                dataType = "TimeSpan";
            }
            else if (nameDataType.Equals("uniqueidentifier"))
            {
                dataType = "Guid";
            }
            else
            {
                dataType = "";
            }
            return dataType;
        }

        /// <summary>
        /// Gét dữ liệu của một DataReader
        /// </summary>
        /// <param name="ddr"></param>
        /// <param name="key"></param>
        /// <param name="clrType"></param>
        /// <returns></returns>
        private object GetDataValue(DbDataReader ddr, string key, ColumnSelectModel columnSelect, bool isGetNameLink)
        {
            object dataValue = null;
            if (ddr[key] == DBNull.Value)
            {
                dataValue = null;
            }
            else
            {
                dataValue = ddr[key];
            }

            if ("Long".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (DateTime)dataValue : (DateTime?)dataValue;
            }
            else if ("Byte".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (byte)dataValue : (byte?)dataValue;
            }
            else if ("ByteArray".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (byte[])dataValue : (byte[]?)dataValue;
            }
            else if ("Bool".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (bool)dataValue : (bool?)dataValue;
            }
            else if ("String".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (string)dataValue : dataValue?.ToString();
            }
            else if ("DateTime".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (DateTime)dataValue : (DateTime?)dataValue;
            }
            else if ("Decimal".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (decimal)dataValue : (decimal?)dataValue;
            }
            else if ("Double".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (double)dataValue : (double?)dataValue;
            }
            else if ("Int".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (int)dataValue : (int?)dataValue;
            }
            else if ("Float".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (float)dataValue : (float?)dataValue;
            }
            else if ("Short".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (short)dataValue : (short?)dataValue;
            }
            else if ("Object".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (object)dataValue : (object?)dataValue;
            }
            else if ("TimeSpan".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (TimeSpan)dataValue : (TimeSpan?)dataValue;
            }
            else if ("Guid".Equals(columnSelect.TypeColumn.DataType))
            {
                dataValue = columnSelect.TypeColumn.Required ? (Guid)dataValue : (Guid?)dataValue;
            }
            else
            {
                dataValue = dataValue?.ToString();
            }

            if (isGetNameLink && columnSelect.LinkData != null && columnSelect.LinkData.Count() > 0)
            {
                dataValue = columnSelect.LinkData.FirstOrDefault(f => f.Id.Equals(dataValue?.ToString()))?.Name ?? "";
            }

            return dataValue;
        }

        /// <summary>
        /// Lấy dữ liệu liên kết
        /// </summary>
        /// <param name="linkTable"></param>
        /// <param name="linkId"></param>
        /// <param name="linkName"></param>
        /// <returns></returns>
        private List<ComboboxBaseModel> GetDataLink(string linkTable, string linkId, string linkName)
        {
            List<ComboboxBaseModel> dataLinks = new List<ComboboxBaseModel>();
            ComboboxBaseModel itemData;
             _dbConnection.Open();
            using (DbCommand command = _dbConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT [{linkId}], [{linkName}] FROM [dbo].[{linkTable}]";
                using (DbDataReader ddr = command.ExecuteReader())
                {
                    while (ddr.Read())
                    {
                        itemData = new ComboboxBaseModel();
                        itemData.Id = ddr[linkId].ToString();
                        itemData.Name = ddr[linkName].ToString();
                        dataLinks.Add(itemData);
                    }
                }
            }
            _dbConnection.Close();
            return dataLinks;
        }

        /// <summary>
        /// Convert list phẳng sang list treeview
        /// </summary>
        /// <param name="dataTreeSource"></param>
        /// <returns></returns>
        private List<TreeViewModel> ConverToDataTree(List<TreeViewModel> dataTreeSource)
        {
            List<TreeViewModel> treeResult = new List<TreeViewModel>();
            List<TreeViewModel> dataOutputSub;
            List<TreeViewModel> dataTreeSub;
            if (dataTreeSource != null && dataTreeSource.Count() > 0)
            {
                var listParent = dataTreeSource.Where(s => string.IsNullOrEmpty(s.ParentId)).ToList();
                foreach (var itemP in listParent)
                {
                    itemP.Text = itemP.Text.Trim().TrimStart('-').TrimEnd('-').Trim();
                    dataTreeSub = dataTreeSource.Where(w => itemP.Id.Equals(w.ParentId)).ToList();
                    itemP.HasChildren = dataTreeSub.Count() > 0;
                    treeResult.Add(itemP);
                    if (itemP.HasChildren)
                    {
                        dataOutputSub = DataSubTree(dataTreeSource, dataTreeSub);
                        if (dataOutputSub.Count() > 0)
                        {
                            treeResult.AddRange(dataOutputSub);
                        }
                    }
                }
            }
            return treeResult;
        }

        /// <summary>
        /// Data sub treeview
        /// </summary>
        /// <param name="dataTreeSource"></param>
        /// <param name="dataTreeSub"></param>
        /// <returns></returns>
        private List<TreeViewModel> DataSubTree(List<TreeViewModel> dataTreeSource, List<TreeViewModel> dataTreeSub)
        {
            List<TreeViewModel> treeResult = new List<TreeViewModel>();
            List<TreeViewModel> dataOutputSub;
            List<TreeViewModel> dataTreeSub1;
            if (dataTreeSub != null && dataTreeSub.Count() > 0)
            {
                foreach (var itemS in dataTreeSub)
                {
                    itemS.Text = itemS.Text.Trim().TrimStart('-').TrimEnd('-').Trim();
                    dataTreeSub1 = dataTreeSource.Where(w => itemS.Id.Equals(w.ParentId)).ToList();
                    itemS.HasChildren = dataTreeSub1.Count() > 0;
                    treeResult.Add(itemS);
                    if (itemS.HasChildren)
                    {
                        dataOutputSub = DataSubTree(dataTreeSource, dataTreeSub1);
                        if (dataOutputSub.Count() > 0)
                        {
                            treeResult.AddRange(dataOutputSub);
                        }
                    }
                }
            }
            return treeResult;
        }
        #endregion
    }
}
