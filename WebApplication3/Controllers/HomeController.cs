using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using HtmlAgilityPack;
using WebApplication3.API;
using WebApplication3.Models;
namespace WebApplication3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        FunPay funPay = new();
        PoeTrade poeTrade = new();
        
        await poeTrade.Generate();
        
        ViewBag.pisia = poeTrade.divinePrice;
        
        ViewBag.jopa = funPay.MinPrice;
        
        DivineCourse divineCourse = new DivineCourse();
        
        ViewBag.anal = ((int.Parse(poeTrade.divinePrice.ToString()) - divineCourse.RUB) / divineCourse.RUB) * 100;
        return View();

        

    }
}

    



