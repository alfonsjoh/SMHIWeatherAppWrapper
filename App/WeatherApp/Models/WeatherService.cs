namespace WeatherApp.Models;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _weatherClientFactory;

    public WeatherService(IHttpClientFactory weatherClientFactory)
    {
        _weatherClientFactory = weatherClientFactory;
    }
    
    public Task<Forecast> GetForecastAsync(WorldPosition position)
    {
        //var weatherClient = _weatherClientFactory.CreateClient("weather");
        throw new NotImplementedException();
    }

    public Task<Forecast> Get10DayForecastAsync(WorldPosition position)
    {
        //var weatherClient = _weatherClientFactory.CreateClient("weather");
        throw new NotImplementedException();
    }
}