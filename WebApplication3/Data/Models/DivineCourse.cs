namespace WebApplication3.Models;
using API;

public class DivineCourse
{
    public int ID { get; set; }
    public double Chaos { get; set; }
    public double RUB { get; set; }
    public DateTime Date { get; set; }


    public DivineCourse()
    {
        FunPay funPay = new();

        PoeTrade poeTrade = new();

        var zalupa = poeTrade.Generate();
        
    }
    
    


}


