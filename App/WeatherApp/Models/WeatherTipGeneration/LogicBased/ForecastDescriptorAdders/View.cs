using WeatherApp.Models.Toolbox;
using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models.WeatherTipGeneration.LogicBased.ForecastDescriptorAdders;

public class View : IForecastDescriptorAdder
{
	public IEnumerable<string> GetForecastDescriptions(ForecastModel forecast)
	{
		var mostCommon = forecast.Prognosis
			.Select(weather => weather.Icon.Alternative).MostCommon().ToLowerInvariant();
		yield return "There will be " + mostCommon;
	}
}