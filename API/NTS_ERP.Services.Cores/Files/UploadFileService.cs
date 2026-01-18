using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NTS.Common.Resource;
using System.IO.Compression;

namespace NTS.Common.Files
{
    public class UploadFileService : IUploadFileService
    {
        private readonly UploadSettingModel uploadSettingModel;
        public UploadFileService(IOptions<UploadSettingModel> option)
        {
            this.uploadSettingModel = option.Value;
        }

        /// <summary>
        /// Upload 1 file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName">Thư mục lưu trữ</param>
        /// <returns></returns>
        public async Task<UploadResultModel> UploadFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0017);
            }

            // Tạo tên file lưu trức
            var extension = Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + extension;

            if (!string.IsNullOrEmpty( uploadSettingModel.Extensions) && !uploadSettingModel.Extensions.Contains(extension.ToLower()))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0018);
            }

            string pathFolder = Path.Combine(uploadSettingModel.FolderUpload, folderName);
            string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);

            // Kiểm tra folder upload
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }

            string pathFile = Path.Combine(pathFolderServer, fileName);

            // Lưu file
            using (var stream = new FileStream(pathFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            UploadResultModel fileResult = new UploadResultModel();
            fileResult.Id = Guid.NewGuid().ToString();
            fileResult.UpdateDate = DateTime.Now;
            fileResult.FileSize = file.Length;
            fileResult.Extension = extension;
            fileResult.FileUrl = Path.Combine(pathFolder, fileName).Replace("\\", "/");
            fileResult.FileName = file.FileName;

            return fileResult;
        }

        /// <summary>
        /// Upload nhiều file
        /// </summary>
        /// <param name="files"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public async Task<List<UploadResultModel>> UploadFiles(List<IFormFile> files, string folderName)
        {
            if (files == null || files.Count <0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0017);
            }
            List<UploadResultModel> results = new List<UploadResultModel>();

            UploadResultModel result;
            foreach (var file in files)
            {
                result = await UploadFile(file, folderName);

                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Download nhiều file
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<FileResultModel> DownloadFiles(DownloadFileModel files)
        {
            List<ArchiveFileModel> archiveFiles = new List<ArchiveFileModel>();
            if (files.Files.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0019);
            }

            foreach (var item in files.Files)
            {
                ArchiveFileModel file = new ArchiveFileModel();
                file.Name = Path.GetFileNameWithoutExtension(item.PathFile);
                file.Extension = Path.GetExtension(item.PathFile);

                string pathFolder = Path.Combine(item.PathFile);
                string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);

                if (File.Exists(pathFolderServer))
                {
                    file.FileBytes = File.ReadAllBytes(pathFolderServer);

                    archiveFiles.Add(file);
                }
            }

            FileResultModel fileResultModel = new FileResultModel();
            fileResultModel.FileStream = await ZipFile(archiveFiles);

            fileResultModel.ContentType = GetContentType(".zip");
            fileResultModel.FileName = GetFileName(".zip", files.FolderName);

            return fileResultModel;
        }

        /// <summary>
        /// Download 1 file
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        public async Task<FileResultModel> DownloadFile(FileModel fileModel)
        {
            if (string.IsNullOrEmpty(fileModel.PathFile))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0013);
            }

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                fileModel.PathFile);

            if (!File.Exists(path))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0013);
            }

            FileResultModel fileResultModel = new FileResultModel();
            using (var memory = new MemoryStream())
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                fileResultModel.FileStream = memory.ToArray();
            }

            fileResultModel.ContentType = GetContentType(fileModel.PathFile);
            fileResultModel.FileName = GetFileName(fileModel.PathFile, fileModel.NameFile);

            return fileResultModel;
        }

        /// <summary>
        /// Zip file
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<byte[]> ZipFile(List<ArchiveFileModel> files)
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
        public string GetContentType(string path)
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
        public string GetFileName(string path, string fileName)
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

        private Dictionary<string, string> GetMimeTypes()
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
                {".zip","application/zip" },
                {".json","application/json" }
            };
        }
    }
}
