using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Document.Word
{
    public interface IWordService
    {
        /// <summary>
        /// Export word Out MemoryStream
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        /// <returns></returns>
        MemoryStream ExportWord(string templatePath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null);

        /// <summary>
        ///  Export word Out Path
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="outPath">Đường dẫn ghi file ra</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        void ExportWord(string templatePath, string outPath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null);

        /// <summary>
        /// Export word to pdf Out MemoryStream
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="replateTexts"></param>
        /// <param name="dataTables"></param>
        /// <param name="replateImages"></param>
        /// <returns></returns>
        MemoryStream ExportWordConvertToPdf(string templatePath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null);

        /// <summary>
        ///  Export word to pdf Out Path
        /// </summary>
        /// <param name="templatePath">Đường dẫn file template</param>
        /// <param name="outPath">Đường dẫn ghi file ra</param>
        /// <param name="replateTexts">List các đối tượng text, html cần replate</param>
        /// <param name="dataTables">List danh sách cá bảng cần fill dữ liệu</param>
        /// <param name="replateImages">List các ảnh cần fill lên template</param>
        void ExportWordConvertToPdf(string templatePath, string outPath, List<ItemTextReplate> replateTexts = null, Dictionary<string, List<object>> dataTables = null, List<ItemImageReplate> replateImages = null);

        MemoryStream ExportWord<T>(string templatePath, T model);

        MemoryStream ExportWordConvertToPdf<T>(string templatePath, T model);

        /// <summary>
        /// Import Word Content
        /// </summary>
        /// <param name="streamSource"></param>
        /// <param name="streamImport"></param>
        /// <returns></returns>
        MemoryStream ImportWordContent(MemoryStream streamSource, MemoryStream streamImport);

        FileStream ConvertToPDF(string pathDoc, string pathOutPdf);
    }
}
