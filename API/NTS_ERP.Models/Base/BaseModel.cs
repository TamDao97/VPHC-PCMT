namespace NTS_ERP.Models.Base
{
    public class BaseModel
    {
        public string? Id { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
