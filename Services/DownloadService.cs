using AisUriProviderApi;
using System.Net;

namespace Services
{
    /// <summary>
    /// Class <see cref="DownloadService"/>
    /// </summary>
    public class DownloadService : IDownloadService
    {
        private List<Uri> _files { get; set; } = new();

        /// <inheritdoc />
        public async Task DownloadFilesAsync(string localFolderPath)
        {
            try
            {
                if (!IsValidPath(localFolderPath))
                {
                    Console.WriteLine("Local folder path invalid!");
                    throw new Exception("Local folder path invalid!");
                }

                AisUriProvider aisUriProvider = new();
                _files = aisUriProvider.Get().ToList();
                for (int i = 0; i < _files.Count(); i += 3)
                {
                    int nbParallelDownload = 0;

                    if (i < _files.Count())
                    {
                        nbParallelDownload++; ;
                    }

                    if (i + 1 < _files.Count())
                    {
                        nbParallelDownload++; ;
                    }

                    if (i + 2 < _files.Count())
                    {
                        nbParallelDownload++; ;
                    }

                    if (nbParallelDownload == 0)
                    {
                        break;
                    }

                    Console.WriteLine($"\n * Start * Executing {nbParallelDownload} parallel download(s)");

                    Task[] taskArray = new Task[nbParallelDownload];

                    for (int j = 0; j < nbParallelDownload; j++)
                    {
                        int pos = i + j;
                        taskArray[j] = Task.Factory.StartNew(() => DownloadFile(_files[pos], localFolderPath));
                    }

                    Task.WaitAll(taskArray);

                    Console.WriteLine($"\n * End * Executing {nbParallelDownload} parallel downloads");
                }
                await DeleteOldFiles(localFolderPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Console.WriteLine($"\n * Operation completed *");
            }
        }

        private async Task DownloadFile(Uri uri, string localFolderPath)
        {
            string filename = Path.GetFileName(uri.LocalPath);

            Console.WriteLine(filename);

            string savePath = localFolderPath;

            using (var client = new WebClient())
            {
                Console.WriteLine($"Downloading '{filename}'...");
                client.DownloadFileAsync(uri, savePath + "\\" + filename);
                Console.WriteLine($"File '{filename}' downloaded");
            }
        }

        private async Task DeleteOldFiles(string localFolderPath)
        {
            string[] filesNames = _files.Select(x => Path.GetFileName(x.LocalPath)).ToArray();
            string savePath = localFolderPath;
            string[] filePaths = Directory.GetFiles(savePath);
            foreach (string filePath in filePaths)
            {
                string actualFileName = new FileInfo(filePath).Name;
                if (!filesNames.Contains(actualFileName))
                {
                    File.Delete(filePath);
                }
            }
        }

        private bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }

                if (!Directory.Exists(path))
                {
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}