using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using HtmlAgilityPack;

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





        
        string url = "https://funpay.com/chips/173/";

        WebClient client = new WebClient();

        string pageContent = client.DownloadString(url);

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(pageContent);

        var serverElements = doc.DocumentNode.SelectNodes("//div[contains(@class, 'tc-server hidden-xxs') and contains(text(), '(PC) Trial of the Ancestors')]");

        // Инициализируем минимальную и максимальную цены
        float minPrice = float.PositiveInfinity;
        float maxPrice = float.NegativeInfinity;

        if (serverElements != null)
        {
            var priceContainers = serverElements.Select(serverElement => serverElement.ParentNode);

            foreach (var container in priceContainers)
            {
                var priceElement = container.SelectSingleNode(".//div[@class='tc-price']");

                if (priceElement != null)
                {
                    string priceText = priceElement.InnerText.Trim();

                    string pattern = @"(\d+\.\d+)";
                    Match match = Regex.Match(priceText, pattern);

                    if (match.Success)
                    {
                        string extractedPrice = match.Groups[1].Value;
                        var pricecomplete = float.Parse(extractedPrice.Replace(".", ","));

                        // Сравниваем найденную цену с текущей минимальной и максимальной ценами
                        if (pricecomplete < minPrice)
                        {
                            minPrice = pricecomplete;
                        }

                        if (pricecomplete > maxPrice)
                        {
                            maxPrice = pricecomplete;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Цена не найдена для (PC) Trial of the Ancestors.");
                }
            }

            if (minPrice != float.PositiveInfinity)
            {
                ViewBag.jopa = minPrice.ToString();
            }
            else
            {
                Console.WriteLine("Цены не найдены для (PC) Trial of the Ancestors.");
            }

            if (maxPrice != float.NegativeInfinity)
            {
                ViewBag.popa = maxPrice.ToString();
            }
            else
            {
                Console.WriteLine("Цены не найдены для (PC) Trial of the Ancestors.");
            }
        }
        else
        {
            Console.WriteLine("Серверы (PC) Trial of the Ancestors не найдены.");
        }

        return View();
    }
}

    



