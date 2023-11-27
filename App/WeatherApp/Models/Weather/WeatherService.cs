using WeatherApp.Models.Weather.Models;
using WeatherApp.Models.WeatherTipGeneration;

namespace WeatherApp.Models.Weather;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _weatherClientFactory;

    public WeatherService(IHttpClientFactory weatherClientFactory)
    {
        _weatherClientFactory = weatherClientFactory;
    }
    
    public Task<ForecastModel> GetForecastAsync(WorldPositionModel positionModel, IWeatherDescriptionGenerator weatherDescriptionGenerator)
    {
        //var weatherClient = _weatherClientFactory.CreateClient("weather");
        throw new NotImplementedException();
    }

    public Task<ForecastModel> Get10DayForecastAsync(WorldPositionModel positionModel, IWeatherDescriptionGenerator weatherDescriptionGenerator)
    {
        //var weatherClient = _weatherClientFactory.CreateClient("weather");
        throw new NotImplementedException();
    }
}