using Microsoft.AspNetCore.Razor.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Helpers
{
    // Định nghĩa các nhóm trạng thái
    public static class GroupsHelper
    {
        /// <summary>
        /// Thâm quyền xử lý
        /// </summary>
        public const string ThamQuyenXL = nameof(ThamQuyenXL);
        /// <summary>
        /// Tiền trình vụ việc
        /// </summary>
        public const string TienTrinhVuViec = nameof(TienTrinhVuViec);
    }

    public class StatusHelper
    {
        // Khởi tạo dữ liệu nhóm trạng thái
        public static readonly Dictionary<string, List<ItemStatus>> _statusGroups = new Dictionary<string, List<ItemStatus>>
        {
            {
                GroupsHelper.ThamQuyenXL, new List<ItemStatus>
                {
                    new ItemStatus { Id = 1, Name = "Thuộc thẩm quyền xử lý của BĐBP" },
                    new ItemStatus { Id = 2, Name = "Không thuộc thẩm quyền xử lý BĐBP" }
                }
            },
            {
                GroupsHelper.TienTrinhVuViec, new List<ItemStatus>
                {
                   new ItemStatus { Id = 0, Name = "Tiếp nhận, phát hiện vụ việc" },
                    new ItemStatus { Id = 1, Name = "Kiểm tra, xác minh" },
                    new ItemStatus { Id = 2, Name = "Xử lý, giải quyết" },
                    new ItemStatus { Id = 3, Name = "Kết thúc" }
                }
            },
        };


        // Lấy tên trạng thái theo nhóm và Id
        public static string GetStatusName(string group, object id, string nameDefault = "KXĐ")
        {
            if (_statusGroups.TryGetValue(group, out var statuses))
            {
                var status = statuses.FirstOrDefault(s => s.Id.Equals(id));
                return status?.Name ?? nameDefault;
            }

            return nameDefault;
        }

        // Lấy danh sách tất cả trạng thái trong một nhóm
        public static List<ItemStatus> GetListStatus(string group)
        {
            return _statusGroups.TryGetValue(group, out var statuses) ? statuses : new List<ItemStatus>();
        }
    }

    // Lớp trạng thái
    public class ItemStatus
    {
        public object Id { get; set; }
        public string Name { get; set; }
    }
}
