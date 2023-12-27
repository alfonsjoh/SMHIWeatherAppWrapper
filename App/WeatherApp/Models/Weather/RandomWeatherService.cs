using WeatherApp.Models.Weather.Models;
using WeatherApp.Models.WeatherTipGeneration;

namespace WeatherApp.Models.Weather;

public class RandomWeatherService : IWeatherService
{
    private readonly Random _rng = new();
    private static readonly string[] Directions = { "S", "N", "W", "E", "SE", "SW", "NE", "NW" };
    private static readonly IconModel[] Icons = {
        new ("/icons/01d.png", "Clear sky"),        new ("/icons/01n.png", "Clear sky"),
        new ("/icons/02d.png", "Few clouds"),       new ("/icons/02n.png", "Few clouds"),
        new ("/icons/03d.png", "Scattered clouds"), new ("/icons/03n.png", "Scattered clouds"),
        new ("/icons/04d.png", "Broken clouds"),    new ("/icons/04n.png", "Broken clouds"),
        new ("/icons/09d.png", "Shower rain"),      new ("/icons/09n.png", "Shower rain"),
        new ("/icons/10d.png", "Rain"),             new ("/icons/10n.png", "Rain"),
        new ("/icons/11d.png", "Thunderstorm"),     new ("/icons/11n.png", "Thunderstorm"),
        new ("/icons/13d.png", "Snow"),             new ("/icons/13n.png", "Snow"),
        new ("/icons/50d.png", "Mist"),             new ("/icons/50n.png", "Mist")
    };

    public async Task<ForecastModel> GetForecastAsync(WorldPositionModel positionModel)
    {
        
        var prognosis = new List<WeatherModel>();
        await Task.Run(() =>
        {
            var now = DateTime.UtcNow;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            
            for (int i = 0; i < 20; i++)
            {
                var temp = _rng.NextSingle() * 40 + 273.15f - 10;
                
                prognosis.Add(new WeatherModel(now + TimeSpan.FromHours(1) * i, 
                    temp,
                    _rng.NextSingle()*12,
                    temp - _rng.NextSingle() * 5,  
                    temp + _rng.NextSingle() * 5,  
                    _rng.NextSingle() * 10 - 5,
                    Directions[_rng.Next(Directions.Length)],
                    Icons[_rng.Next(Icons.Length)]));
            }
        });

        var forecast = new ForecastModel(prognosis);

        return forecast;
    }

    public async Task<ForecastModel> Get10DayForecastAsync(WorldPositionModel positionModel)
    {
        var prognosis = new List<WeatherModel>();
        await Task.Run(() =>
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            
            for (int i = 0; i < 10; i++)
            {
                var temp = _rng.NextSingle() * 40 + 273.15f - 10;
                
                prognosis.Add(new WeatherModel(now + TimeSpan.FromDays(1) * i, 
                    temp,
                    _rng.NextSingle()*5,
                    temp - _rng.NextSingle() * 5,  
                    temp + _rng.NextSingle() * 5,  
                    _rng.NextSingle() * 10 - 5,
                    Directions[_rng.Next(Directions.Length)],
                    Icons[_rng.Next(Icons.Length)]));
            }
        });

        return new ForecastModel(prognosis);
    }
}