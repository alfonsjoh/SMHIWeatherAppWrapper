using System.Text.Json;
using WeatherApp.Models.Toolbox;
using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models.WeatherTipGeneration.LogicBased;

public class LogicBasedWeatherDescriptionGenerator : IWeatherDescriptionGenerator
{
	private const string ForecastDescriptorAddersFile = "./ForecastDescriptions.json";

	private readonly ForecastDescriptor[] _forecastDescriptorAdders;
	
	public LogicBasedWeatherDescriptionGenerator()
	{
		// Read Forecast descriptor adders from json file
		using (var sr = new StreamReader(ForecastDescriptorAddersFile))
		{
			var json = sr.ReadToEnd();
			var forecastDescriptorAdders = JsonSerializer.Deserialize<ForecastDescriptor[]>(json);
			if (forecastDescriptorAdders == null)
			{
				throw new Exception("Failed to deserialize ForecastDescriptorAdders");
			}
			
			_forecastDescriptorAdders = forecastDescriptorAdders;
		}
	}
    
	public void SetWeatherDescription(ref ForecastModel forecast)
	{
		var model = forecast;
		var mostCommonDescription = model.Prognosis.Select(weather => weather.Icon.Description).MostCommon().ToLower();
		
		forecast.Description = string.Join(" ", _forecastDescriptorAdders
			.Where(descriptorAdder => descriptorAdder.IsApplicable(mostCommonDescription))
			.SelectMany(descriptorAdder => descriptorAdder.GetForecastDescriptions()).ToArray());
	}
}