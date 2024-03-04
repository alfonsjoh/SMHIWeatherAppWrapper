using System.Globalization;
using WeatherApp.Models.Toolbox;
using WeatherApp.Models.Weather.Models;
using WeatherApp.Models.Weather.Models.smhi;

namespace WeatherApp.Models.Weather;

public class WeatherServiceSMHI : IWeatherService
{
    private readonly IHttpClientFactory _weatherClientFactory;
    private readonly IconConverter _iconConverter;
    
    public WeatherServiceSMHI(IHttpClientFactory weatherClientFactory, IconConverter iconConverter)
    {
        _weatherClientFactory = weatherClientFactory;
        _iconConverter = iconConverter;
    }

    private async Task<List<WeatherModel>> Get3HourForecast5Days(WorldPositionModel positionModel)
    {
        var weatherClient = _weatherClientFactory.CreateClient("weather");

        var weatherData = await weatherClient.GetFromJsonAsync<WeatherData>(
            $"api/category/pmp3g/version/2/geotype/point/" +
            $"lon/{positionModel.Lon.ToString("F", CultureInfo.InvariantCulture)}/" +
            $"lat/{positionModel.Lat.ToString("F", CultureInfo.InvariantCulture)}/data.json");

        if (weatherData is null)
        {
            throw new Exception("Weather data is null");
        }

        var weatherModels = new List<WeatherModel>();

        foreach (var list in weatherData.timeSeries)
        {
            var weatherModel = new WeatherModel
            {
                DateTime = DateTime.Parse(list.validTime, null, DateTimeStyles.RoundtripKind),
            };
            
            foreach (var weather in list.parameters)
            {
                if (weather.values.Length == 0)
                {
                    continue;
                }
                switch (weather.name)
                {
                    case "t":
                        weatherModel.Temperature = UnitConversion.CelsiusToKelvin(weather.values[0]);
                        break;
                    case "ws":
                        weatherModel.WindSpeed = weather.values[0];
                        break;
                    case "wd":
                        weatherModel.WindDirection = UnitConversion.DegreesToDirection(weather.values[0]);
                        break;
                    case "pmean":
                        weatherModel.Precipitation = weather.values[0];
                        break;
                    case "Wsymb2":
                        weatherModel.Icon = _iconConverter.GetIconModel((int)weather.values[0], positionModel, weatherModel.DateTime);
                        break;
                }
            }
            weatherModels.Add(weatherModel);
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