using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class PoeTrade
{
    string league = "Ancestor";
    string itemType = "Currency";
    string detailsId = "divine-orb";

    public decimal divinePrice;

    public PoeTrade()
    {
        Generate();
    }

    public async Task Generate()
    {
        string apiUrl =
            $"https://poe.ninja/api/data/currencyoverview?league={league}&type={itemType}&detailsId={detailsId}";
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    JObject jsonObject = JObject.Parse(responseBody);

                    JToken divineOrbToken = jsonObject.SelectToken($"$.lines[?(@.detailsId == '{detailsId}')]");

                    if (divineOrbToken != null)
                    {
                        decimal divineOrbValue = divineOrbToken["receive"]["value"].Value<decimal>();
                        divinePrice = divineOrbValue;
                    }
                    else
                    {
                        Console.WriteLine("Divine Orb не найден в JSON-ответе.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка при выполнении запроса к API poe.ninja.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}