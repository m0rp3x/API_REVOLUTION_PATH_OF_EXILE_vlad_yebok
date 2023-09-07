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
        DivineCourse? lastCourse = db.DivineCourses.OrderBy(x=>x.Date).Last();
        DivineCourse? prevCourse = db.DivineCourses.FirstOrDefault(x=>x.ID == lastCourse.ID-1);
        
        ViewBag.pisia = lastCourse.Chaos;
        ViewBag.jopa = lastCourse.RUB;
        
        if (lastCourse != null)
        {
            double rubDifference = lastCourse.RUB - prevCourse.RUB;
            double percentageChange;

            if (lastCourse.RUB != 0)
            {
                percentageChange = (rubDifference / prevCourse.RUB) * 100;
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
}

    



