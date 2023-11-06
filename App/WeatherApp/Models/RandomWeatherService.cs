namespace WeatherApp.Models;

public class RandomWeatherService : IWeatherService
{
    private readonly Random _rng = new();
    private static readonly string[] Directions = { "S", "N", "W", "E", "SE", "SW", "NE", "NW" };

    public async Task<Forecast> GetForecastAsync(WorldPosition position)
    {
        var prognosis = new List<Weather>();
        await Task.Run(() =>
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            
            for (int i = 0; i < 20; i++)
            {
                var temp = _rng.NextSingle() * 40 + 273.15f - 10;
                
                prognosis.Add(new Weather(now + TimeSpan.FromHours(1) * i, 
                    temp,
                    temp - _rng.NextSingle() * 5,  
                    temp + _rng.NextSingle() * 5,  
                    _rng.NextSingle() * 10 - 5,
                    Directions[_rng.Next(Directions.Length)]));
            }
        });

        return new Forecast(prognosis);
    }

    public async Task<Forecast> Get10DayForecastAsync(WorldPosition position)
    {
        var prognosis = new List<Weather>();
        await Task.Run(() =>
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            
            for (int i = 0; i < 10; i++)
            {
                var temp = _rng.NextSingle() * 40 + 273.15f - 10;
                
                prognosis.Add(new Weather(now + TimeSpan.FromDays(1) * i, 
                    temp,
                    temp - _rng.NextSingle() * 5,  
                    temp + _rng.NextSingle() * 5,  
                    _rng.NextSingle() * 10 - 5,
                    Directions[_rng.Next(Directions.Length)]));
            }
        });

        return new Forecast(prognosis);
    }
}