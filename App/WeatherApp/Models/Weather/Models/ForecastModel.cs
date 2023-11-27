using System.Text.Json.Serialization;

namespace WeatherApp.Models.Weather.Models;

public class ForecastModel
{
    public List<WeatherModel> Prognosis { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Description { get; set; }
    
    public ForecastModel(List<WeatherModel> prognosis, string description = "")
    {
        Prognosis = prognosis;
        Description = description;
    }
}