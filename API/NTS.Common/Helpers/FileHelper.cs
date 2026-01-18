using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// Zip file
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static async Task<byte[]> ZipFile(List<ArchiveFileModel> files)
        {
            using (var packageStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(packageStream, ZipArchiveMode.Create, true))
                {
                    foreach (var virtualFile in files)
                    {
                        //Create a zip entry for each attachment
                        var zipFile = archive.CreateEntry(virtualFile.Name + virtualFile.Extension);

                        using (MemoryStream originalFileMemoryStream = new MemoryStream(virtualFile.FileBytes))
                        {
                            using (var zipEntryStream = zipFile.Open())
                            {
                                await originalFileMemoryStream.CopyToAsync(zipEntryStream);
                            }
                        }
                    }
                }

                return packageStream.ToArray();
            }

        }

        /// <summary>
        /// Lấy loại file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        /// <summary>
        /// Lấy tên file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileName(string path, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string extensionInPath = Path.GetExtension(path);
            string fileNameNew = string.Empty;
            if (string.IsNullOrEmpty(extension))
            {
                fileNameNew = fileName + extensionInPath;
            }
            else if (extension.Equals(extensionInPath))
            {
                fileNameNew = fileName.Remove(fileName.LastIndexOf(".")) + extensionInPath;
            }

            return fileNameNew;
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }
    }

    public class ArchiveFileModel
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] FileBytes { get; set; }
    }
}
