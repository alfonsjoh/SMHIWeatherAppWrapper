namespace WeatherApp.Models;

public class Forecast
{
    public List<Weather> Prognosis { get; set; }
    
    public Forecast(List<Weather> prognosis)
    {
        Prognosis = prognosis;
    }
}