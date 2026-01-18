using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace NTS.Common.Utils
{
    public static class FileUtil
    {
        public static bool SaveJson(string pathFileJson, object data)
        {
            try
            {
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(pathFileJson))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, data);


                }
                return true;

            }
            catch (IOException io) { return false; }
            catch (Exception ex) { return false; }
        }

        public static T ReadJson<T>(string pathFileJson)
        {
            if (!File.Exists(pathFileJson))
                return default(T);

            try
            {
                string json = File.ReadAllText(pathFileJson);
                T data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }
            catch (IOException io) { throw io; }
            catch (Exception ex) { throw ex; }
            return default(T);
        }

        public static bool CreatingFileZip(string zipFile, List<string> pathFiles)
        {
            try
            {
                using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                {
                    foreach (var fPath in pathFiles)
                    {
                        archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                    }
                }
                return true;
            }
            catch (IOException io) { return false; }
            catch (Exception ex) { return false; }
        }

        public static bool CreatingDirectoryZip(string startPath, string zipPath)
        {
            try
            {
                ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
                return true;
            }
            catch (IOException io) { return false; }
            catch (Exception ex) { return false; }
        }

        public static bool ExtractZipToDirectory(string zipPath, string destinationDirectoryName, bool overwrite = true)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipPath, destinationDirectoryName, overwrite);
                return true;
            }
            catch (IOException io) { return false; }
            catch (Exception ex) { return false; }
        }

        public static void ClearFolder(string FolderName, int? beforeDay = 0)
        {
            try
            {
                DateTime dateNow = DateTime.Now;
                DirectoryInfo dir = new DirectoryInfo(FolderName);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    try
                    {
                        if (beforeDay == null || beforeDay.Value == 0 || (beforeDay.HasValue && (dateNow - fi.CreationTime).TotalDays > beforeDay))
                            fi.Delete();
                    }
                    catch (IOException io) { }
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    ClearFolder(di.FullName);
                    try
                    {
                        if (beforeDay == null || beforeDay.Value == 0 || (beforeDay.HasValue && (dateNow - di.CreationTime).TotalDays > beforeDay))
                            di.Delete();
                    }
                    catch (IOException io) { }
                }
            }
            catch (Exception ex) { }
        }

        public static bool CopyFile(string sourceFileName, string destFileName, bool overwrite = true)
        {
            try
            {
                if (!File.Exists(sourceFileName))
                    return false;

                string directoryDest = Path.GetDirectoryName(destFileName);
                if (!Directory.Exists(directoryDest))
                {
                    Directory.CreateDirectory(directoryDest);
                }
                File.Copy(sourceFileName, destFileName, overwrite);
                return true;
            }
            catch (IOException iex) { return false; }
            catch (Exception ex) { return false; }
            finally
            {
            }
        }

        public static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            if (Directory.Exists(targetDirectory) == false)
            {
                Directory.CreateDirectory(targetDirectory);
            }

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyDirectoryAll(diSource, diTarget);
        }

        private static void CopyDirectoryAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                try
                {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }
                catch (IOException io) { }
                catch (Exception ex) { }
                finally
                {
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectoryAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// Check file có đang được sử dụng hay không
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string fileName)
        {
            try
            {
                FileInfo file = new FileInfo(fileName);
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        public static void ProcessKill(string filename)
        {
            try
            {
                Process proc = Process.Start(filename);
                // do stuff
                proc.Kill();
            }
            catch (IOException io) { throw io; }
            catch (Exception ex) { throw ex; }
        }

        public static void AppKill(string appName)
        {
            try
            {
                var chromeDriverProcesses1 = Process.GetProcesses();
                var chromeDriverProcesses = Process.GetProcesses().Where(pr => pr.ProcessName == appName);

                foreach (var process in chromeDriverProcesses)
                {
                    process.Kill();
                }
            }
            catch (Exception ex) { }
        }
    }
}
