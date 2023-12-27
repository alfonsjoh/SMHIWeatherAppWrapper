namespace WeatherApp.Models.Weather.Models.smhi;

public class SMHIWeatherData
{
    public string approvedTime { get; set; }
    public string referenceTime { get; set; }
    public Geometry geometry { get; set; }
    public TimeSeries[] timeSeries { get; set; }
}

public class Geometry
{
    public string type { get; set; }
    public double[][] coordinates { get; set; }
}

public class TimeSeries
{
    public string validTime { get; set; }
    public Parameters[] parameters { get; set; }
}

public class Parameters
{
    public string name { get; set; }
    public string levelType { get; set; }
    public int level { get; set; }
    public string unit { get; set; }
    public float[] values { get; set; }
}

