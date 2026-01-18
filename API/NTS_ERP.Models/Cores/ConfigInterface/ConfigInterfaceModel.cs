using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.ConfigInterface
{
    public class ConfigInterfaceModel
    {
        public string Id { get; set; }
        public string SoftwareName { get; set; }
        public bool IsUseMultiLanguage { get; set; }
        public bool IsUseCaptcha { get; set; }
        public string FilePathLogo { get; set; }
        public string FilePathIcon { get; set; }
        public bool IsShowLogoTopBar { get; set; }
        public string Logo { get; set; }
        public int MenuType { get; set; }
        public string CroppedImage { get; set; }
    }
}
