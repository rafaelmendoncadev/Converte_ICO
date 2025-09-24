using Converte_ICO.Services;

namespace Converte_ICO.BackgroundServices
{
    public class TempFileCleanupService : BackgroundService
    {
        private readonly ILogger<TempFileCleanupService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(1); // Run every hour
        private readonly TimeSpan _fileAge = TimeSpan.FromHours(2); // Delete files older than 2 hours

        public TempFileCleanupService(ILogger<TempFileCleanupService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var imageConverter = scope.ServiceProvider.GetRequiredService<IImageConverterService>();
                        
                        if (imageConverter is ImageConverterService service)
                        {
                            service.CleanupTempFiles(_fileAge);
                            _logger.LogInformation("Limpeza de arquivos temporários executada com sucesso.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro durante a limpeza de arquivos temporários.");
                }

                await Task.Delay(_cleanupInterval, stoppingToken);
            }
        }
    }
}