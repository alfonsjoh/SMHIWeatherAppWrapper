using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models.WeatherTipGeneration;

/// <summary>
/// A class that implements this interface produces
/// forecast descriptions for example by retrieving it from other APIs
/// or creating random forecasts.
/// </summary>
public interface IWeatherDescriptionGenerator
{
    void SetWeatherDescription(ref ForecastModel forecast);
}