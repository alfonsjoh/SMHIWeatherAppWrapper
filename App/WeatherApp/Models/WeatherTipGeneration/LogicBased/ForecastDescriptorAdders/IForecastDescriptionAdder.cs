using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models.WeatherTipGeneration.LogicBased.ForecastDescriptorAdders;

public interface IForecastDescriptorAdder
{
	public IEnumerable<string> GetForecastDescriptions(ForecastModel forecast);
}