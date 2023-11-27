using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models.WeatherTipGeneration;

public class RandomWeatherDescriptionGenerator : IWeatherDescriptionGenerator
{
    private readonly Random _rng = new(); 
    private readonly string[] _weatherDescriptions = { "It will be sunny", "It will rain. Use an umbrella." };
    
    public void SetWeatherDescription(ref ForecastModel forecast)
    {
        forecast.Description = _weatherDescriptions[_rng.Next(_weatherDescriptions.Length)];
    }
}