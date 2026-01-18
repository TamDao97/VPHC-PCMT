using NTS.Common;
using NTS.Common.Files;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS.Document.Excel;
using NTS.Document.PowerPoint;
using NTS.Document.Word;

namespace NTS_ERP.Services.Cores.ViewFileWeb
{
    public class ViewFileWebService : IViewFileWebService
    {
        private readonly IWordService _wordService;
        private readonly IPowerPointService _powerPointService;
        private readonly IExcelService _excelService;

        public ViewFileWebService(IWordService wordService, IPowerPointService powerPointService, IExcelService excelService)
        {
            this._wordService = wordService;
            this._powerPointService = powerPointService;
            this._excelService = excelService;
        }

        /// <summary>
        /// Lấy thông tin view file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<FileResultModel> GetFileViewAsync(string path)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), path)))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0013);
            }

            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), path);
            string extension = Path.GetExtension(path);

            FileResultModel fileResult = new FileResultModel();
            string pathOutPdf = Path.Combine(Directory.GetCurrentDirectory(), NTSConstants.FolderExportData, "fileview_temp" + NTSConstants.ExtensionPDF);
            using (var memory = new MemoryStream())
            {
                if (Path.GetExtension(path).ToLower().Equals(".pdf") || Path.GetExtension(path).ToLower().Equals(".jpg") || Path.GetExtension(path).ToLower().Equals(".png"))
                {
                    using (var stream = new FileStream(pathFile, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                        stream.Dispose();
                    }
                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(extension);
                }
                else if (Path.GetExtension(path).ToLower().Equals(".doc") || Path.GetExtension(path).ToLower().Equals(".docx"))
                {
                    using (var stream = this._wordService.ConvertToPDF(pathFile, pathOutPdf))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(memory);
                        stream.Dispose();
                    }
                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(".pdf");
                }
                else if (Path.GetExtension(path).ToLower().Equals(".xls") || Path.GetExtension(path).ToLower().Equals(".xlsx"))
                {
                    using (var stream = this._excelService.ConvertToPDF(pathFile, pathOutPdf))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(memory);
                        stream.Dispose();
                    }

                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(".pdf");
                }
                else if (Path.GetExtension(path).ToLower().Equals(".ppt") || Path.GetExtension(path).ToLower().Equals(".pptx"))
                {
                    using (var stream = this._powerPointService.ConvertToPDF(pathFile, pathOutPdf))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(memory);
                        stream.Dispose();
                    }
                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(".pdf");
                }
                else
                {
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;

                    fileResult.FileStream = memory.ToArray();
                    fileResult.ContentType = FileHelper.GetContentType(extension);
                }
                memory.Dispose();
            }

            fileResult.FileName = Path.GetFileName(path);

            return fileResult;
        }
    }
}
