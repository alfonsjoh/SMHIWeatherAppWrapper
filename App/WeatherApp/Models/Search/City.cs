using System.Text.Json.Serialization;
using WeatherApp.Models.Weather;
using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models;

public class City
{
    public string Index { get; set; }
    public string Name { get; set; }
    public float Lon { get; set; }
    public float Lat { get; set; }
    public string Municipality { get; set; }
    public string County { get; set; }
    
    [JsonIgnore]
    public WorldPositionModel PositionModel => new WorldPositionModel(Lon, Lat);
    
    public override string ToString()
    {
        if (Name == Municipality)
        {
            return $"{Name}, {County}";
        }
        
        return $"{Name}, {Municipality}, {County}";
    }
    
    public ulong GetRedisId()
    {
        return PositionModel.GetByteRepresentation();
    }
    
    public ApiCity ToApiCity()
    {
        return new ApiCity(ToString(), Index);
    }
}