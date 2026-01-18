using System.Collections.Generic;

namespace NTS_ERP.Models.Cores.Menu
{
    public class UpdateIndexMenuModel
    {
        public string Id { get; set; }
        public string TitleKeyTranslate { get; set; }
        public string TitleDefault { get; set; }
        public List<UpdateIndexMenuModel> Children { get; set; }
        public UpdateIndexMenuModel()
        {
        }
    }
}
