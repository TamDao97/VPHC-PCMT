using NTS.Common;
using NTS.Common.Resource;
using Syncfusion.Pdf;
using Syncfusion.Presentation;
using Syncfusion.PresentationRenderer;
using Syncfusion.PresentationToPdfConverter;

namespace NTS.Document.PowerPoint
{
    public class PowerPointService : IPowerPointService
    {
        /// <summary>
        /// Chuyển đổi file PowerPoint sang pdf
        /// </summary>
        /// <param name="pathPPT">Đường dẫn file PowerPoint đầu vào</param>
        /// <param name="pathOutPdf">Đường dẫn file pdf đầu ra</param>
        /// <returns></returns>
        public FileStream ConvertToPDF(string pathPPT, string pathOutPdf)
        {
            try
            {
                if (!File.Exists(pathPPT))
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0013);

                IPresentation pptxDoc = Presentation.Open(@$"{pathPPT}");

                //Converts the PowerPoint Presentation into PDF document
                PdfDocument pdfDocument = PresentationToPdfConverter.Convert(pptxDoc);
                FileStream outputStream = new FileStream(pathOutPdf, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                pdfDocument.Save(outputStream);
                pdfDocument.Close();
                return outputStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
