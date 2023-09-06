using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using HtmlAgilityPack;
using WebApplication3.API;
using WebApplication3.Data;
using WebApplication3.Models;
namespace WebApplication3.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class HomeController : Controller
{
    static ApplicationContext db;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApplicationContext context)
    {
        db = context;
    }

    public async Task<IActionResult> Index()
    {
        FunPay funPay = new();
        PoeTrade poeTrade = new();

        await poeTrade.Generate();

        ViewBag.pisia = poeTrade.divinePrice;

        ViewBag.jopa = funPay.MinPrice;


        DivineCourse? jopa = db.DivineCourses.FirstOrDefault(m => m != null && m.ID == 1);

        if (jopa != null)
        {
            double rubDifference = funPay.MinPrice - jopa.RUB;
            double percentageChange;

            if (jopa.RUB != 0)
            {
                percentageChange = (rubDifference / jopa.RUB) * 100;
            }
            else
            {
                // Обработка случая, когда RUB равно нулю.
                percentageChange = 0; // Или любое другое значение по вашему усмотрению.
            }

            ViewBag.anal = (int)Math.Round(percentageChange);
        }

        return View();
    }

    public IActionResult UpdateData()
    {
        try
        {
            // Получите запись из базы данных
            var dataRecord = db.DivineCourses.FirstOrDefault(); // Замените YourTable на имя вашей таблицы
            
            FunPay funPay = new();
            PoeTrade poeTrade = new();

             poeTrade.Generate();

            var popa  = poeTrade.divinePrice;

            var jopa =  funPay.MinPrice;
            
            if (dataRecord != null)
            {
                // Обновите данные
                dataRecord.Chaos = (double)popa; // Замените на вашу логику получения нового значения Chaos
                dataRecord.RUB = jopa; // Замените на вашу логику получения нового значения RUB
                dataRecord.Date = DateTime.Now;

                // Сохраните изменения в базе данных
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Обработка ошибок, если необходимо
            ViewBag.ErrorMessage = ex.Message;
            return View("Error");
        }
    }
}

    



