using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Files
{
    public interface IUploadFileService
    {
        public Task<UploadResultModel> UploadFile(IFormFile file, string folderName);
        Task<List<UploadResultModel>> UploadFiles(List<IFormFile> files, string folderName);
        Task<FileResultModel> DownloadFiles(DownloadFileModel files);
        Task<FileResultModel> DownloadFile(FileModel fileModel);
        Task<byte[]> ZipFile(List<ArchiveFileModel> files);
        string GetContentType(string path);
        string GetFileName(string path, string fileName);
    }
}
