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
        
        var record = new DivineCourse
        {
            Chaos = (double)poeTrade.divinePrice,
            RUB = funPay.MinPrice,
            Date = DateTime.Now
        };

        db.DivineCourses.Add(record);
        await db.SaveChangesAsync();


        await Task.Delay(100);
        _logger.LogInformation(
            "Курс был записан");
    }

    public async Task ReRangeData(DateTime dateTime)
    {
        List<DivineCourse> entitiesToDelete = db.DivineCourses
            .Where(e => e.Date.Year == dateTime.Year &&
                        e.Date.Month == dateTime.Month &&
                        e.Date.Day == dateTime.Day)
            .ToList();
        entitiesToDelete.Remove(entitiesToDelete.Last());

        db.DivineCourses.RemoveRange(entitiesToDelete);

        await db.SaveChangesAsync();
    }
}