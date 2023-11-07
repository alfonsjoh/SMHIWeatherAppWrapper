namespace WeatherApp.Models;

public interface IWeatherService
{
    Task<Forecast> GetForecastAsync(WorldPosition position);

    Task<Forecast> Get10DayForecastAsync(WorldPosition position);
}