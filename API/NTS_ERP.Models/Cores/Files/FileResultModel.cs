using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NTS.Common.Files
{
    public class FileResultModel
    {
        public byte[] FileStream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
