using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Common
{
    public class MRUFileCache
    {
        private readonly DirectoryInfo directoryInfo;
        public int Capacity { get; set; }

        public MRUFileCache(string cacheDirectory, int capacity = 100)
        {
            Directory.CreateDirectory(cacheDirectory);
            directoryInfo = new DirectoryInfo(cacheDirectory);
            Capacity = capacity;
        }

        public string DownloadAndAdd(string url)
        {
            Uri uri = new Uri(url);
            var filePath = Path.Combine(directoryInfo.FullName, uri.Segments.Last());
            if (File.Exists(filePath))
            {
                try
                {
                    File.SetLastWriteTime(filePath, DateTime.Now);
                }
                catch (IOException)
                {
                    // File in use
                }

                return filePath;
            }

            var tempFilePath = $"{filePath}.temp";
            if (File.Exists(tempFilePath))
                DeleteIfExists(tempFilePath);

            using (WebClient ws = new WebClient())
            {
                try
                {
                    ws.DownloadFile(url, filePath);
                }
                catch (WebException)
                {
                    DeleteIfExists(tempFilePath);
                    return null;
                }

                var i = 0;
                while (i++ != 5)
                {
                    try
                    {
                        File.Move(tempFilePath, filePath);
                        break;
                    }
                    catch
                    {
                        if (i == 5)
                            throw;
                    }
                }
            }

            Clean();
            return filePath;

            void DeleteIfExists(string path)
            {
                if (File.Exists(path))
                {
                    var i = 0;
                    while (i++ != 5)
                    {
                        try
                        {
                            File.Delete(path);
                            break;
                        }
                        catch
                        {
                            if (i == 5)
                                throw;
                        }
                    }
                }
            }
        }

        public void Clean()
        {
            try
            {
                directoryInfo.Refresh();
                var files = directoryInfo.GetFiles();
                if (files.Length > Capacity)
                {
                    foreach (var file in files.OrderBy(f => f.LastWriteTimeUtc).Take(5))
                        file.Delete();
                }
            }
            catch (IOException)
            {
                // do nothing
            }
        }
    }
}
