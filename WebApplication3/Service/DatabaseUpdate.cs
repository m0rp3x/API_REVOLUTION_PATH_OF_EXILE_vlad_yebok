using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using WebApplication3.API;
namespace WebApplication3.Service;
using WebApplication3.Data;

public class DatabaseUpdate
{
    ApplicationContext db;
    private readonly ILogger<DatabaseUpdate> _logger;

    public DatabaseUpdate(ILogger<DatabaseUpdate> logger, ApplicationContext context)
    {
        db = context;
        _logger = logger;
    }

    public async Task Update()
    {
        FunPay funPay = new();
        PoeTrade poeTrade = new();

        await poeTrade.Generate();

       
            var record = db.DivineCourses.FirstOrDefault();
            if (record == null)
            {
                record = new DivineCourse();
            }

            record.Chaos = (int)Math.Round((double)poeTrade.divinePrice);
            record.RUB = (int)Math.Round(funPay.MinPrice);
            record.Date = DateTime.Now;

            db.Entry(record).State = EntityState.Modified;
            await db.SaveChangesAsync();

        

        await Task.Delay(100);
        _logger.LogInformation(
            "Sample Service did something.");
    }
}