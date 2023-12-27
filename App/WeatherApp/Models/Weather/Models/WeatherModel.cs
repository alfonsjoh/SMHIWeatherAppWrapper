using WeatherApp.Models.Toolbox;

namespace WeatherApp.Models.Weather.Models;

public class WeatherModel
{
    public WeatherModel(DateTime dateTime,
        float temperature,
        float precipitation,
        float lowTemp,
        float highTemp,
        float windSpeed,
        string windDirection, // ex. "NE", "S", "SW"
        IconModel icon)
    {
        DateTime = dateTime;
        Temperature = temperature;
        Precipitation = precipitation;
        LowTemp = lowTemp;
        HighTemp = highTemp;
        WindSpeed = windSpeed;
        WindDirection = windDirection;
        Icon = icon;
    }

    public WeatherModel()
    {
        WindDirection = "";
        Icon = new IconModel();
    }

    public DateTime DateTime { get; set; }
    public float Temperature { get; set; }
    public float Precipitation { get; set; }
    public float LowTemp { get; set; }
    public float HighTemp { get; set; }
    public float WindSpeed { get; set; }
    public string WindDirection { get; set; }
    public IconModel Icon { get; set; }

    
    public static WeatherModel AverageWeatherModel(List<WeatherModel> weatherModels)
    {
        var weatherModel = new WeatherModel();

        // Calculate average date time. // This is not done with linq because that generates a arithmetic overflow exception.
        // The difference is that the ticks get divided by the count before adding to the sum.
        double tickAverage = weatherModels.Sum(weather => weather.DateTime.Ticks / (double)weatherModels.Count);

        weatherModel.DateTime = new DateTime((long)tickAverage);
        
        weatherModel.Temperature = weatherModels.Select(weather => weather.Temperature).Average();
        weatherModel.Precipitation = weatherModels.Select(weather => weather.Precipitation).Sum();
        
        // Calculate min and max with the low or high temps first. If those do not provide a valid temp calculate high and low based on the 'Temperature'.
        // Since the unit is in kelvin if the min or max is not set it will be 0.
        weatherModel.LowTemp = weatherModels.Select(weather => weather.LowTemp).Min();
        if (weatherModel.LowTemp == 0) 
            weatherModel.LowTemp = weatherModels.Select(weather => weather.Temperature).Min();
        
        weatherModel.HighTemp = weatherModels.Select(weather => weather.HighTemp).Max();
        if (weatherModel.HighTemp == 0)
            weatherModel.HighTemp = weatherModels.Select(weather => weather.Temperature).Max();
        
        weatherModel.WindSpeed = weatherModels.Select(weather => weather.WindSpeed).Average();
        
        weatherModel.WindDirection = weatherModels.Select(weather => weather.WindDirection).MostCommon();
        weatherModel.Icon = weatherModels.Select(weather => weather.Icon).MostCommon();

        return weatherModel;
    }
}