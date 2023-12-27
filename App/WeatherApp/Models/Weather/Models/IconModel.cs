namespace WeatherApp.Models.Weather.Models;

public class IconModel
{
    public IconModel(string source,
        string alternative)
    {
        Source = source;
        Alternative = alternative;
    }

    public IconModel()
    {
        Source = "";
        Alternative = "";
    }

    public string Source { get; init; }
    public string Alternative { get; init; }
}