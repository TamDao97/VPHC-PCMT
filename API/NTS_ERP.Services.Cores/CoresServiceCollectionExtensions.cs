using Microsoft.Extensions.DependencyInjection;
using NTS.Common.Files;
using NTS.Document.Excel;
using NTS.Document.PowerPoint;
using NTS.Document.Word;
using NTS_ERP.Services.Cores.Abouts;
using NTS_ERP.Services.Cores.Auth;
using NTS_ERP.Services.Cores.Categorys;
using NTS_ERP.Services.Cores.Combobox;
using NTS_ERP.Services.Cores.ConfigInterface;
using NTS_ERP.Services.Cores.FileUpload;
using NTS_ERP.Services.Cores.GroupUsers;
using NTS_ERP.Services.Cores.Log;
using NTS_ERP.Services.Cores.Menu;
using NTS_ERP.Services.Cores.SystemFunction;
using NTS_ERP.Services.Cores.UserHistorys;
using NTS_ERP.Services.Cores.Users;
using NTS_ERP.Services.Cores.ViewFileWeb;

namespace NTS_ERP.Services.Cores
{
    public static class CoresServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesCores(this IServiceCollection services)
        {
            services.AddScoped<IWordService, WordService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IPowerPointService, PowerPointService>();

            services.AddScoped<IComboboxService, ComboboxService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupUserService, GroupUserService>();
            services.AddScoped<ILogEventService, LogEventService>();
            services.AddScoped<IUserHistoryService, UserHistoryService>();
            services.AddScoped<IViewFileWebService, ViewFileWebService>();
            services.AddScoped<IAboutService, AboutService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFileTemplateService, FileTemplateService>();
            services.AddScoped<IMenuSystemService, MenuSystemService>();
            services.AddScoped<ISystemFunctionService, SystemFunctionService>();
            services.AddScoped<IConfigInterfaceService, ConfigInterfaceService>();
            services.AddScoped<IPermissionService, PermissionService>();

            return services;
        }
    }
}
