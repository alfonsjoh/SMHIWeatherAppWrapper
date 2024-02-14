using System.Text.Json.Serialization;
using WeatherApp.Models.Toolbox;
using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models.WeatherTipGeneration.LogicBased;

public class ForecastDescriptor
{
	private readonly Random _rng = new();
	[JsonPropertyName("keyword")]
	public string Keyword { get; set; }
	[JsonPropertyName("descriptions")]
	public string[] Descriptions { get; set; }
	
	public bool IsApplicable(string mostCommonDescription) => mostCommonDescription.Contains(Keyword);

	public IEnumerable<string> GetForecastDescriptions()
	{
		yield return Descriptions.RandomElement(_rng);
	}
}