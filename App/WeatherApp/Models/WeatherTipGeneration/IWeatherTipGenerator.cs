namespace WeatherApp.Models.WeatherTipGeneration;

public interface IWeatherTipGenerator
{
    string GetWeatherTip(Forecast forecast);
}