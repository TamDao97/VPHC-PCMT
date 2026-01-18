using NTS_ERP.Models.Entities;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Common;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrafficControl.Core;
using NTS_ERP.Models.Cores.FileUpload;

namespace NTS_ERP.Services.Cores.FileUpload
{
    public class FileTemplateService : IFileTemplateService
    {
        private readonly NTS_ERPContext _sqlContext;
        public FileTemplateService(NTS_ERPContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }
        /// <summary>
        /// Search biểu mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<TemplateSearchResultModel>> SearchFileTemplate(TemplateSearchModel searchModel)
        {
            SearchBaseResultModel<TemplateSearchResultModel> searchResult = new SearchBaseResultModel<TemplateSearchResultModel>();

            var dataQuery = (from a in _sqlContext.Nts_FileTemplate.AsEnumerable()
                             orderby a.CreateDate
                             select new TemplateSearchResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Description = a.Description,
                                 Code = a.Code,
                                 UpdatedDate = a.UpdateDate,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(a => a.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(a => a.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.OrderBy))
            {
                dataQuery = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType);

            }
            searchResult.TotalItems = dataQuery.Count();

            searchResult.DataResults = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
           
            return searchResult;

        }
      
        /// <summary>
        /// xem biểu mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TemplateSearchResultModel> GetFileTemplateById(string id)
        {
            TemplateSearchResultModel result = new TemplateSearchResultModel();

            result = (from a in _sqlContext.Nts_FileTemplate.AsEnumerable()
                      where a.Id == id
                      select new TemplateSearchResultModel()
                      {
                          Id = a.Id,
                          Name = a.Name,
                          Description = a.Description,
                          Code = a.Code,
                          UpdatedDate = a.UpdateDate,
                          PathFile = a.FilePath,
                      }).FirstOrDefault();

            return result;

        }

        /// <summary>
        /// Thêm mới biểu mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateFileTemplate(string userId, FileUploadCreateModel model)
        {
            if(string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Code) )
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0020);
            }
            if (string.IsNullOrEmpty(model.Path))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0017);
            }
            Nts_FileTemplate fileTemplate = new Nts_FileTemplate()
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                FilePath = model.Path,
                Description = model.Description,
                CreateBy = userId,
                CreateDate = DateTime.Now,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
            };
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.Nts_FileTemplate.Add(fileTemplate);
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
        /// Update biểu mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateFileTemplate(string userId,string id, FileUploadCreateModel model)
        {

            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Code))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0020);
            }
            if (string.IsNullOrEmpty(model.Path))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0017);
            }
            var file = _sqlContext.Nts_FileTemplate.FirstOrDefault(e => e.Id.Equals(id));
            if (file == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0024);
            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), file.FilePath);

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    file.Description = model.Description;
                    file.FilePath = model.Path;
                    file.UpdateDate = DateTime.Now;
                    file.UpdateBy = userId;
                    _sqlContext.Nts_FileTemplate.Update(file);
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
}
