namespace WeatherApp.Models.Weather.Models;

public record WeatherModel
(
    DateTime DateTime,
    float Temperature,
    float Precipitation,
    float LowTemp,
    float HighTemp,
    float WindSpeed,
    string WindDirection, // ex. "NE", "S", "SW"
    IconModel Icon
);