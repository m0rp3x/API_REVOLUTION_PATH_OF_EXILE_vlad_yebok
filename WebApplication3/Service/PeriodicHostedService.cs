using WebApplication3.Controllers;
namespace WebApplication3.Service;


class PeriodicHostedService : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromMinutes(2);
    private readonly ILogger<PeriodicHostedService> _logger;
    private readonly IServiceScopeFactory _factory;
    private int _executionCount = 0;
    public bool IsEnabled { get; set; }

    public PeriodicHostedService(
        ILogger<PeriodicHostedService> logger, 
        IServiceScopeFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // ExecuteAsync is executed once and we have to take care of a mechanism ourselves that is kept during operation.
        // To do this, we can use a Periodic Timer, which, unlike other timers, does not block resources.
        // But instead, WaitForNextTickAsync provides a mechanism that blocks a task and can thus be used in a While loop.
        using PeriodicTimer timer = new PeriodicTimer(_period);

        // When ASP.NET Core is intentionally shut down, the background service receives information
        // via the stopping token that it has been canceled.
        // We check the cancellation to avoid blocking the application shutdown.
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                    
                // Get service from scope
                DatabaseUpdate sampleService = asyncScope.ServiceProvider.GetRequiredService<DatabaseUpdate>();
                await sampleService.Update();
                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    $"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
            }
        }
    }
}