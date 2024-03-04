using System.Text.Json;
using System.Text.Json.Serialization;
using Innovative.SolarCalculator;
using WeatherApp.Models.Toolbox;

namespace WeatherApp.Models.Weather.Models.smhi;

public class IconConverter
{
    private List<IconConversion> _iconConversions;
    
    public IconConverter(string iconsFile)
    {
        LoadIcons(iconsFile);
    }
    
    private void LoadIcons(string iconsFile)
    {
        using var stream = File.OpenRead(iconsFile);
        _iconConversions = JsonSerializer.Deserialize<List<IconConversion>>(stream) ?? new List<IconConversion>();
    }
    
    private bool TryGetIconConversion(int smhiId, out IconConversion iconConversion)
    {
        for (int i = 0, len = _iconConversions.Count; i < len; i++)
        {
            if (_iconConversions[i].SmhiId != smhiId) continue;
            iconConversion = _iconConversions[i];
            return true;
        }

        iconConversion = new IconConversion(0, "", "");
        return false;
    }
    
    private string GetIcon(int weatherCode, WorldPositionModel positionModel, DateTime dateTime)
    {
        // Calculate if it is day or night
        // var sun = SunInfoCalculator.GetSunriseSunsetTime(positionModel, dateTime);
        var solarTimes = new SolarTimes(dateTime, positionModel.Lat, positionModel.Lon);
        
        // Set the suffix to "n" if it is night and "d" if it is day
        var suffix = dateTime < solarTimes.Sunrise || dateTime > solarTimes.Sunset ? "n" : "d";

        return TryGetIconConversion(weatherCode, out var iconConversion) ? $"/icons/{iconConversion.IconId}{suffix}.png" : "";
    }
    
    private string GetIconDescription(int weatherCode)
    {
        return TryGetIconConversion(weatherCode, out var iconConversion) ? iconConversion.Description : "";
    }
    
    public IconModel GetIconModel(int weatherCode, WorldPositionModel positionModel, DateTime dateTime)
    {
        return new IconModel(GetIcon(weatherCode, positionModel, dateTime), GetIconDescription(weatherCode));
    }
}