using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace NTS.Common.Files
{
    public class ArchiveFileModel
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] FileBytes { get; set; }
    }
}
