using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Services;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        LoggerProviderOptions.RegisterProviderOptions<
            EventLogSettings, EventLogLoggerProvider>(services);

        var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json");

        var config = configuration.Build();
        Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

        services.Configure<Settings>((s) => s.LocalFolderPath = settings.LocalFolderPath);

        services.AddScoped<IDownloadService, DownloadService>();
        services.AddSingleton<IHostedService>(x =>
         ActivatorUtilities.CreateInstance<DownloadBackgroundService>(x)
        );
    })
    .Build();

await host.RunAsync();