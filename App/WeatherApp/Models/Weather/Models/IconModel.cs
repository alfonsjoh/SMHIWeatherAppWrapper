namespace WeatherApp.Models.Weather.Models;

public class IconModel
{
    public IconModel(string source,
        string description)
    {
        Source = source;
        Description = description;
    }

    public IconModel()
    {
        Source = "";
        Description = "";
    }

    public string Source { get; init; }
    public string Description { get; init; }
}