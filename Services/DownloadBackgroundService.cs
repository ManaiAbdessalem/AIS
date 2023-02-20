using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Services
{
    public sealed class DownloadBackgroundService : BackgroundService
    {
        private readonly IDownloadService _downloadService;
        private readonly IOptionsMonitor<Settings> _optionsMonitor;

        public DownloadBackgroundService(
            IDownloadService downloadService, IOptionsMonitor<Settings> optionsMonitor) =>
            (_downloadService, _optionsMonitor) = (downloadService, optionsMonitor);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    string localFolderPath = _optionsMonitor.CurrentValue.LocalFolderPath;
                    await _downloadService.DownloadFilesAsync(localFolderPath);
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Environment.Exit(1);
            }
        }
    }
}