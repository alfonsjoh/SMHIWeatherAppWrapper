namespace WeatherApp.Models.Weather.Models.smhi;

public record IconConversion(
    int SmhiId,
    string IconId,
    string Description);