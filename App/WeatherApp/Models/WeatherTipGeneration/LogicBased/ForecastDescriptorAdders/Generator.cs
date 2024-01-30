using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models.WeatherTipGeneration.LogicBased.ForecastDescriptorAdders;

public class LogicBasedWeatherDescriptionGenerator : IWeatherDescriptionGenerator
{
	private readonly IForecastDescriptorAdder[] _forecastDescriptorAdders = {
		new View()
	};
    
	public void SetWeatherDescription(ref ForecastModel forecast)
	{
		var model = forecast;
		forecast.Description = string.Join(". ", _forecastDescriptorAdders
			.SelectMany(descriptorAdder => descriptorAdder.GetForecastDescriptions(model)).ToArray());
	}
}