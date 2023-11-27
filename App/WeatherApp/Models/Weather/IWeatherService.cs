using WeatherApp.Models.Weather.Models;
using WeatherApp.Models.WeatherTipGeneration;

namespace WeatherApp.Models.Weather;

/// <summary>
/// A class that implements this interface produces
/// forecasts for example by retrieving it from other APIs
/// or creating random forecasts.
///
/// A forecast contains a description, that this interface is
/// not responsible for producing. Therefore the methods that produces forecast takes in
/// a IWeatherDescriptionGenerator that will produce it.
/// </summary>
public interface IWeatherService
{
    Task<ForecastModel> GetForecastAsync(WorldPositionModel positionModel, IWeatherDescriptionGenerator weatherDescriptionGenerator);

    Task<ForecastModel> Get10DayForecastAsync(WorldPositionModel positionModel, IWeatherDescriptionGenerator weatherDescriptionGenerator);

    delegate Task<ForecastModel> GetForecastAsyncDelegate(WorldPositionModel positionModel,
        IWeatherDescriptionGenerator weatherDescriptionGenerator);
}