using Innovative.SolarCalculator;
using WeatherApp.Models.Toolbox;

namespace WeatherApp.Models.Weather.Models.smhi;

public static class IconConverter
{
    private static readonly Dictionary<int, string> IconMap = new()
    {
        {1, "01"},	// Clear sky
        {2, "01"},	// Nearly clear sky
        {3, "02"},	// Variable cloudiness
        {4, "02"},	// Halfclear sky
        {5, "02"},	// Cloudy sky
        {6, "03"},	// Overcast
        {7, "03"},	// Fog
        {8, "09"},	// Light rain showers
        {9, "10"},	// Moderate rain showers
        {10, "10"},	// Heavy rain showers
        {11, "11"},	// Thunderstorm
        {12, "13"},	// Light sleet showers
        {13, "13"},	// Moderate sleet showers
        {14, "13"},	// Heavy sleet showers
        {15, "13"},	// Light snow showers
        {16, "13"},	// Moderate snow showers
        {17, "13"},	// Heavy snow showers
        {18, "09"},	// Light rain
        {19, "10"},	// Moderate rain
        {20, "10"},	// Heavy rain
        {21, "11"},	// Thunder
        {22, "13"},	// Light sleet
        {23, "13"},	// Moderate sleet
        {24, "13"},	// Heavy sleet
        {25, "13"},	// Light snowfall
        {26, "13"},	// Moderate snowfall
        {27, "13"},	// Heavy snowfall
    };
    
    private static readonly Dictionary<int, string> IconDescriptionMap = new()
    {
        {1, "Clear sky"},
        {2, "Nearly clear sky"},
        {3, "Variable cloudiness"},
        {4, "Halfclear sky"},
        {5, "Cloudy sky"},
        {6, "Overcast"},
        {7, "Fog"},
        {8, "Light rain showers"},
        {9, "Moderate rain showers"},
        {10, "Heavy rain showers"},
        {11, "Thunderstorm"},
        {12, "Light sleet showers"},
        {13, "Moderate sleet showers"},
        {14, "Heavy sleet showers"},
        {15, "Light snow showers"},
        {16, "Moderate snow showers"},
        {17, "Heavy snow showers"},
        {18, "Light rain"},
        {19, "Moderate rain"},
        {20, "Heavy rain"},
        {21, "Thunder"},
        {22, "Light sleet"},
        {23, "Moderate sleet"},
        {24, "Heavy sleet"},
        {25, "Light snowfall"},
        {26, "Moderate snowfall"},
        {27, "Heavy snowfall"},
    };

    static IconConverter()
    {
        
    }
    
    private static string GetIcon(int weatherCode, WorldPositionModel positionModel, DateTime dateTime)
    {
        // Calculate if it is day or night
        // var sun = SunInfoCalculator.GetSunriseSunsetTime(positionModel, dateTime);
        var solarTimes = new SolarTimes(dateTime, positionModel.Lat, positionModel.Lon);
        
        // Set the suffix to "n" if it is night and "d" if it is day
        var suffix = dateTime < solarTimes.Sunrise || dateTime > solarTimes.Sunset ? "n" : "d";

        return IconMap.TryGetValue(weatherCode, out var icon) ? $"/icons/{icon}{suffix}.png" : "";
    }
    
    private static string GetIconDescription(int weatherCode)
    {
        return IconDescriptionMap.GetValueOrDefault(weatherCode, "");
    }
    
    public static IconModel GetIconModel(int weatherCode, WorldPositionModel positionModel, DateTime dateTime)
    {
        return new IconModel(GetIcon(weatherCode, positionModel, dateTime), GetIconDescription(weatherCode));
    }
}