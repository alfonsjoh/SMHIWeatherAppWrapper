namespace WeatherApp.Models.WeatherTipGeneration;

public class RandomWeatherTipGenerator : IWeatherTipGenerator
{
    private readonly Random _rng = new(); 
    private readonly string[] _weatherTips = { "It will be sunny", "It will rain. Use an umbrella." };
    
    public string GetWeatherTip(Forecast forecast)
    {
        return _weatherTips[_rng.Next(_weatherTips.Length)];
    }
}