using WebApplication3.Controllers;

namespace WebApplication3.Service;

class PeriodicHostedService : BackgroundService
{
    static DateTime now = DateTime.Now;
    static DateTime nextMidnight = now.Date.AddDays(1); // Получаем полночь следующего дня
    TimeSpan timeUntilMidnight = nextMidnight - now;
    private readonly TimeSpan _period1 = TimeSpan.FromMinutes(2); // Таймер для выполнения каждые х минут
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
        using PeriodicTimer timer1 = new PeriodicTimer(_period1);
        using PeriodicTimer timer2 = new PeriodicTimer(timeUntilMidnight);


        while (!stoppingToken.IsCancellationRequested)
        {
            // Первая задача, выполняющаяся каждые 5 минут
            if (await timer1.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                    DatabaseUpdate sampleService = asyncScope.ServiceProvider.GetRequiredService<DatabaseUpdate>();
                    await sampleService.Update();

                    _executionCount++;
                    _logger.LogInformation($"Выполнено (каждые 5 минут): {_executionCount} раза");
                    
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(
                        $"Ошибка при выполнении задачи каждые 5 минут: {ex.Message}. Попробуйте в следующий раз!");
                }
            }

            // Вторая задача, выполняющаяся раз в день
            if (await timer2.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                    DatabaseUpdate sampleService = asyncScope.ServiceProvider.GetRequiredService<DatabaseUpdate>();
                    await sampleService.ReRangeData(now);
                    
                    _logger.LogInformation($"Выполнено (РеРенж данных)в {now.ToString("MM/dd/yy")} ");

                    now = DateTime.Now;
                    timeUntilMidnight = nextMidnight - now;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(
                        $"Ошибка при выполнении задачи раз в день: {ex.Message}. Попробуйте в следующий раз!");
                }
            }
        }
    }
}