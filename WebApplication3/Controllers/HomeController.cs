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

    public HomeController(ApplicationContext context) => db = context;

    public async Task<IActionResult> Index()
    {
        DivineCourse? lastCourse = db.DivineCourses.OrderBy(x => x.Date).LastOrDefault();
        DivineCourse? prevCourse = db.DivineCourses.FirstOrDefault(x => x.ID == lastCourse.ID - 1);

        if (lastCourse != null && prevCourse != null)
        {
            ViewBag.pisia = Math.Round(lastCourse.Chaos, 2);
            ViewBag.jopa = Math.Round(lastCourse.RUB, 2);

            double rubDifference = lastCourse.RUB - prevCourse.RUB;
            double percentageChangeRub = lastCourse.RUB != 0 ? (rubDifference / prevCourse.RUB) * 100 : 0;
            ViewBag.anal = Math.Round(percentageChangeRub, 2);

            double ChaosDifference = lastCourse.Chaos - prevCourse.Chaos;
            double percentageChangeChaos = lastCourse.RUB != 0 ? (ChaosDifference / prevCourse.RUB) * 100 : 0;
            ViewBag.ChaosCurse = Math.Round(percentageChangeChaos, 2);
        }
        else
        {
            ViewBag.pisia = 0.0;
            ViewBag.jopa = 0.0;
            ViewBag.anal = 0.0;
            ViewBag.ChaosCurse = 0.0;
        }

        var divineCourses = db.DivineCourses
            .OrderByDescending(course => course.Date)
            .Take(40)
            .ToList();;

        return View(divineCourses);
        
    }

}

    



