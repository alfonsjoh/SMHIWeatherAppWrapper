namespace WeatherApp.Models;

public class ApiCity
{
    public string Name { get; set; }
    public string Index { get; set; }

    public ApiCity(string name, string index)
    {
        Name = name;
        Index = index;
    }
}