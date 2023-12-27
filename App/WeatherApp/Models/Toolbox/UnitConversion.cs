namespace WeatherApp.Models.Toolbox;

public static class UnitConversion
{
    private static readonly string[] directions = {"N", "NE", "E", "SE", "S", "SW", "W", "NW"};
    
    
    public static float CelsiusToKelvin(float celsius)
    {
        return celsius + 273.15f;
    }
    
    public static string DegreesToDirection(float degrees)
    {
        return directions[(int)((degrees-45.0/2) % 360 / 45.0)];
    }
}