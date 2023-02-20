namespace Services
{
    /// <summary>
    /// Interface <see cref="IDownloadService"/>
    /// </summary>
    public interface IDownloadService
    {
        /// <summary>
        /// download ten Files
        /// </summary>
        /// <param name="localFolderPath">local Folder Path</param>
        Task DownloadFilesAsync(string localFolderPath);
    }
}