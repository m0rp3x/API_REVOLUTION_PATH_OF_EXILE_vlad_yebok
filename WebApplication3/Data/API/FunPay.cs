using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace WebApplication3.API;

public class FunPay
{
    string url = "https://funpay.com/chips/173/";

    WebClient client = new WebClient();

    float minPrice = float.PositiveInfinity;

    float maxPrice = float.NegativeInfinity;
    
    public FunPay()
    {
        string pageContent = client.DownloadString(url);

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(pageContent);

        var serverElements =
            doc.DocumentNode.SelectNodes(
                "//div[contains(@class, 'tc-server hidden-xxs') and contains(text(), '(PC) Trial of the Ancestors')]");


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
        }
        else
        {
            Console.WriteLine("Серверы (PC) Trial of the Ancestors не найдены.");
        }
    }

    public float MaxPrice
    {
        get => maxPrice;
        set => maxPrice = value;
    }
    public float MinPrice
    {
        get => minPrice;
        set => minPrice = value;
    }
}