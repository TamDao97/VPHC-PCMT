using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.Files
{
    public class DownloadFileModel
    {
        public List<FileModel> Files { get; set; }
        public string FolderName { get; set; }
        public DownloadFileModel()
        {
            Files = new List<FileModel>();
        }
    }


    public class FileModel
    {
        public string PathFile { get; set; }
        public string NameFile { get; set; }
    }
}
