namespace WebApplication3.Service;

public class DatabaseUpdate
{
    private readonly ILogger<DatabaseUpdate> _logger;

    public DatabaseUpdate(ILogger<DatabaseUpdate> logger)
    {
        _logger = logger;
    }

    public async Task Update()
    {
        await Task.Delay(100);
        _logger.LogInformation(
            "Sample Service did something.");
    }
}