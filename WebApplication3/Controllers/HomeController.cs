using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using HtmlAgilityPack;
using WebApplication3.API;

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
        return View();
    }
}

    



