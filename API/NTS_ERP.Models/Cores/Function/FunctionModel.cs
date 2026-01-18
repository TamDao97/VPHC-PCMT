namespace NTS_ERP.Models.Cores.Function
{
    public class FunctionModel
    {
        /// <summary>
        /// Id quyền
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Id nhóm quyền
        /// </summary>
        public string? MenuSystemId { get; set; }

        /// <summary>
        /// Mã quyền
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Tên quyền
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// check
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
