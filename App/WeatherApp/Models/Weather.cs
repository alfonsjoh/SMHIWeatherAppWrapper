namespace WeatherApp.Models;

public record Weather
(
    DateTime DateTime,
    float Temperature,
    float Precipitation,
    float LowTemp,
    float HighTemp,
    float WindSpeed,
    string WindDirection, // ex. "NE", "S", "SW"
    Icon Icon
);