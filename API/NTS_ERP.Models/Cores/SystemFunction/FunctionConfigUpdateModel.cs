using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Models.Cores.SystemFunction
{
    public class FunctionConfigUpdateModel
    {
        public string FunctionName { get; set; }
        public string TableName { get; set; }
        public string Slug { get; set; }
        public int Index { get; set; }
        public bool SearchDisplay { get; set; }
        public bool FilterDisplay { get; set; }
        public bool CreateDisplay { get; set; }
        public int CreateWindowType { get; set; }
        public string CreateWindowWidth { get; set; }
        public bool EditDisplay { get; set; }
        public int EditWindowType { get; set; }
        public string EditWindowWidth { get; set; }
        public bool DetailDisplay { get; set; }
        public int DetailWindowType { get; set; }
        public string DetailWindowWidth { get; set; }
        public bool? DeleteDisplay { get; set; }
        public bool ImportDisplay { get; set; }
        public bool ExportDisplay { get; set; }
        public string LinkTemplate { get; set; }
        public int? DataColumnStart { get; set; }
        public int? DataRowStart { get; set; }
        public int LayoutType { get; set; }
        public string TreeName { get; set; }
        public string TreeTableName { get; set; }
        public string TreeColumnId { get; set; }
        public string TreeColumnParentId { get; set; }
        public string TreeColumnsText { get; set; }
        public string TreeFunctionConfigId { get; set; }

        /// <summary>
        /// Danh sách cấu hình chức năng tìm kiếm, thêm, sửa, xóa
        /// </summary>
        public List<SystemFunctionDesignModel> FunctionDesigns { get; set; }
    }
}
