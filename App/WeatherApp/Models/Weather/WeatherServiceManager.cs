namespace WeatherApp.Models.Weather;

public static class WeatherServiceManager
{
    public static void ConfigureWeatherService(WebApplicationBuilder webApplicationBuilder)
    {
        switch (webApplicationBuilder.Configuration["WeatherService"])
        {
            case "owp":
                ConfigureOWM(webApplicationBuilder);
                break;
            case "smhi":
                ConfigureSMHI(webApplicationBuilder);
                break;
            case "random":
                ConfigureRandom(webApplicationBuilder);
                break;
            default:
                throw new Exception("Bad weather service was not provided in configuration file");
        }
    }

    private static void ConfigureRandom(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddSingleton<IWeatherService>(provider =>
            new RandomWeatherService()
        );
    }

    private static void ConfigureSMHI(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddHttpClient(
            "weather",
            client =>
            {
                client.BaseAddress = new Uri("https://opendata-download-metfcst.smhi.se/");
            }
        );
        webApplicationBuilder.Services.AddSingleton<IWeatherService>(provider =>
            new WeatherServiceSMHI(provider.GetService<IHttpClientFactory>()!)
        );
    }

    private static void ConfigureOWM(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddHttpClient(
            "weather",
            client =>
            {
                client.BaseAddress = new Uri("https://api.openweathermap.org/data/");
            }
        );
        webApplicationBuilder.Services.AddSingleton<IWeatherService>(provider =>
            new WeatherServiceOWM(provider.GetService<IHttpClientFactory>()!,
                Environment.GetEnvironmentVariable("weatherapi_key")!)
        );
    }
}