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

    public IActionResult Index()
    {
        
        
        
        return View();
    }

    public IActionResult Privacy()
    {
        FunPay funPay = new FunPay();
        ViewBag.jopa = funPay.MinPrice;
        ViewBag.popa = funPay.MaxPrice;
        return View();
    }
}

    



