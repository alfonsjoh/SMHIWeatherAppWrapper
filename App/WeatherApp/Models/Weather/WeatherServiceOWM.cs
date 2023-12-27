using System.Globalization;
using WeatherApp.Models.Weather.Models;
using WeatherApp.Models.WeatherTipGeneration;
using WeatherApp.Models.Weather.Models.openweathermap;

namespace WeatherApp.Models.Weather;

public class WeatherServiceOWM : IWeatherService
{
    private readonly IHttpClientFactory _weatherClientFactory;
    private readonly string _apiKey;

    public WeatherServiceOWM(IHttpClientFactory weatherClientFactory, string apiKey)
    {
        _weatherClientFactory = weatherClientFactory;
        _apiKey = apiKey;
    }

    private async Task<List<WeatherModel>> Get3HourForecast5Days(WorldPositionModel positionModel)
    {
        var weatherClient = _weatherClientFactory.CreateClient("weather");

        var weatherData = await weatherClient.GetFromJsonAsync<OWMWeatherData>(
            $"2.5/forecast/?" +
            $"lat={positionModel.Lat.ToString(CultureInfo.InvariantCulture)}" +
            $"&lon={positionModel.Lon.ToString(CultureInfo.InvariantCulture)}" +
            $"&appid={_apiKey}");

        if (weatherData is null)
        {
            throw new Exception("Weather data is null");
        }

        var weatherModels = new List<WeatherModel>();

        foreach (var list in weatherData.list)
        {
            foreach (var weather in list.weather)
            {
                weatherModels.Add(new WeatherModel(
                    DateTimeOffset.FromUnixTimeSeconds(list.dt).UtcDateTime,
                    (float)list.main.temp,
                    (float)((list.rain ?? Rain.None)._3h + (list.snow ?? Snow.None)._3h),
                    (float)list.main.temp_min,
                    (float)list.main.temp_max,
                    (float)(list.wind ?? Wind.Still).speed,
                    $"{(list.wind ?? Wind.Still).deg}°",
                    new IconModel($"/icons/{weather.icon}.png", weather.description)));
            }
        }

        return weatherModels;
    }

    public async Task<ForecastModel> GetForecastAsync(WorldPositionModel positionModel)
    {
        var weatherModels = await Get3HourForecast5Days(positionModel);

        // Limit to 30 hours
        weatherModels = weatherModels.Where(weather => weather.DateTime < DateTime.UtcNow + TimeSpan.FromHours(30))
            .ToList();

        var forecast = new ForecastModel(weatherModels);

        return forecast;
    }

    public async Task<ForecastModel> Get10DayForecastAsync(WorldPositionModel positionModel)
    {
        var weatherModels = await Get3HourForecast5Days(positionModel);

        // Limit to 10 days
        weatherModels = weatherModels.Where(weather => weather.DateTime < DateTime.UtcNow + TimeSpan.FromDays(10))
            .ToList();

        // Average weather data per day
        weatherModels = weatherModels.GroupBy(w => w.DateTime.Date)
            .Select(g => WeatherModel.AverageWeatherModel(g.ToList()))
            .ToList();

        var forecast = new ForecastModel(weatherModels);

        return forecast;
    }
}