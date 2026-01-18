using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Document
{
    public class DocumentResultModel
    {
        public byte[] FileStream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
