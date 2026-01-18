using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.ConfigInterface;
using NTS_ERP.Models.Entities;

namespace NTS_ERP.Services.Cores.ConfigInterface
{
    public class ConfigInterfaceService : IConfigInterfaceService
    {
        private readonly NTS_ERPContext _sqlContext;

        public ConfigInterfaceService(NTS_ERPContext sqlContext)
        {
            _sqlContext = sqlContext;
        }
        public async Task<ConfigInterfaceModel> GetConfigAsync()
        {
            ConfigInterfaceModel result = new ConfigInterfaceModel();
            try
            {
                result = (from a in _sqlContext.Nts_SystemConfig.AsNoTracking()
                          select new ConfigInterfaceModel
                          {
                              Id = a.Id,
                              SoftwareName = a.SoftwareName,
                              IsShowLogoTopBar = a.ShowLogoTopBar,
                              IsUseCaptcha = a.IsLoginCaptcha,
                              IsUseMultiLanguage = a.IsMultiLanguage,
                              Logo = a.Logo,
                              FilePathLogo = a.LogoFolded,
                              FilePathIcon = a.FaviconIcon,
                              MenuType = a.MenuType,

                          }).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task CreateOrUpdateAsync(ConfigInterfaceModel model)
        {
            //thêm
            if (string.IsNullOrEmpty(model.Id))
            {
                Nts_SystemConfig nts_SystemConfig = new Nts_SystemConfig()
                {
                    Id = Guid.NewGuid().ToString(),
                    SoftwareName = model.SoftwareName,
                    IsLoginCaptcha = model.IsUseCaptcha,
                    IsMultiLanguage = model.IsUseMultiLanguage,
                    LogoFolded = model.FilePathLogo,
                    FaviconIcon = model.FilePathIcon,
                    ShowLogoTopBar = model.IsShowLogoTopBar,
                    Logo = model.Logo,
                    MenuType = model.MenuType,
                };
                _sqlContext.Nts_SystemConfig.Add(nts_SystemConfig);
            }
            else if (!string.IsNullOrEmpty(model.Id))
            {
                var config = _sqlContext.Nts_SystemConfig.FirstOrDefault(e => model.Id.Equals(e.Id));
                if (config == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0032);
                }
                config.SoftwareName = model.SoftwareName;
                config.IsLoginCaptcha = model.IsUseCaptcha;
                config.IsMultiLanguage = model.IsUseMultiLanguage;
                config.LogoFolded = model.CroppedImage;
                config.FaviconIcon = model.FilePathIcon;
                config.ShowLogoTopBar = model.IsShowLogoTopBar;
                config.Logo = model.Logo;
                config.MenuType = model.MenuType;
                _sqlContext.Nts_SystemConfig.Update(config);

            }

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();

                    RemoveFile(model.FilePathIcon);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }
        //private string CloneFileImage(string data, string folder)
        //{
        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        var data_split = data.Split(";")[1];
        //        var base64 = data_split.Split(",")[1];
        //        if (base64 == null || base64.Length == 0)
        //        {
        //            throw NTSException.CreateInstance(MessageResourceKey.MSG0017);
        //        }
        //        string fileName = "Logo.png";
        //        //string pathFolder = Path.Combine(uploadSettingModel.FolderUpload, "ConfigInterface", "Logo");
        //        string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);
        //        try
        //        {
        //            if (!Directory.Exists(pathFolderServer))
        //            {
        //                Directory.CreateDirectory(pathFolderServer);
        //            }
        //            string pathFile = Path.Combine(pathFolderServer, fileName);

        //            byte[] bytes = Convert.FromBase64String(base64);
        //            File.WriteAllBytes(pathFile, bytes);
        //            return Path.Combine(pathFolder, fileName);
        //        }
        //        catch (Exception ex)
        //        {
        //            return "";
        //        }
        //    }
        //    else
        //    {
        //        return folder;
        //    }

        //}
        private void RemoveFile(string icon)
        {
            try
            {
                string folderPathIcon = Path.Combine(Directory.GetCurrentDirectory(), "FileUpload", "ConfigInterface", "Icon");
                string[] iconFiles = Directory.GetFiles(folderPathIcon);
                foreach (string iconFile in iconFiles)
                {
                    if (iconFile.Contains(Path.Combine(Directory.GetCurrentDirectory(), icon).Replace("/", @"\"))) continue;
                    if (File.Exists(iconFile))
                        File.Delete(iconFile);
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}
