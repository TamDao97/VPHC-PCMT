using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Document.PowerPoint
{
    public interface IPowerPointService
    {
        /// <summary>
        /// Chuyển đổi file PowerPoint sang pdf
        /// </summary>
        /// <param name="pathPPT"></param>
        /// <returns>file pdf</returns>
        FileStream ConvertToPDF(string pathPPT, string pathOutPdf);
    }
}
