namespace CollectionManager.Common;

using System;
using System.IO;
using System.Linq;
using System.Net;

public class MRUFileCache
{
    private readonly DirectoryInfo directoryInfo;
    public int Capacity { get; set; }

    public MRUFileCache(string cacheDirectory, int capacity = 100)
    {
        _ = Directory.CreateDirectory(cacheDirectory);
        directoryInfo = new DirectoryInfo(cacheDirectory);
        Capacity = capacity;
    }

    public string DownloadAndAdd(string url)
    {
        Uri uri = new(url);
        string filePath = Path.Combine(directoryInfo.FullName, uri.Segments.Last());
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

        string tempFilePath = $"{filePath}.temp";
        if (File.Exists(tempFilePath))
        {
            DeleteIfExists(tempFilePath);
        }

        using (WebClient ws = new())
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

            int i = 0;
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
                    {
                        throw;
                    }
                }
            }
        }

        Clean();
        return filePath;

        static void DeleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                int i = 0;
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
                        {
                            throw;
                        }
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
            FileInfo[] files = directoryInfo.GetFiles();
            if (files.Length > Capacity)
            {
                foreach (FileInfo file in files.OrderBy(f => f.LastWriteTimeUtc).Take(5))
                {
                    file.Delete();
                }
            }
        }
        catch (IOException)
        {
            // do nothing
        }
    }
}
