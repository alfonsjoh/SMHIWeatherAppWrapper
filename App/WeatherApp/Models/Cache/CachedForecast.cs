﻿using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Meilisearch;
using StackExchange.Redis;
using WeatherApp.Models.Weather;
using WeatherApp.Models.Weather.Models;
using WeatherApp.Models.WeatherTipGeneration;

namespace WeatherApp.Models;

public partial class CachedForecast
{
    #if DEBUG // Use a shorter duration in debug mode to be able to test that the cache clears
    public const int DefaultDuration = 1*60;
    #else
    public const int DefaultDuration = 15*60;
    #endif
    
    public DateTime Timeout { get; set; }
    public ForecastModel ForecastModel { get; set; }
    
    [JsonIgnore]
    public bool IsExpired => Timeout < DateTime.Now;
    
    public CachedForecast()
    {
        Timeout = DateTime.Now;
        ForecastModel = new ForecastModel(new List<WeatherModel>(), "");
    }
    
    public CachedForecast(ForecastModel forecastModel, TimeSpan timeout)
    {
        ForecastModel = forecastModel;
        Timeout = DateTime.Now + timeout;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">The type of the forecast that will be used as redis key</param>
    /// <param name="locationId">The location given by the user</param>
    /// <param name="redisConnection"></param>
    /// <param name="meilisearchClient"></param>
    /// <param name="forecastService"></param>
    /// <param name="weatherDescriptionGenerator"></param>
    /// <param name="duration">The duration that the cache will kept in seconds</param>
    public static async Task<ForecastModel?> TryGetForecastByCacheOrServiceAsync(
        string type,
        string locationId,
        IConnectionMultiplexer redisConnection,
        MeilisearchClient meilisearchClient,
        IWeatherService.GetForecastAsyncDelegate forecastService,
        IWeatherDescriptionGenerator? weatherDescriptionGenerator=null, 
        double duration=DefaultDuration)
    {
        // Validate that the id is safe for sending to Meilisearch
        if (!IdRegex().IsMatch(locationId))
        {
            return null;
        }
        
        // Get the location from meilisearch
        City city;
        try
        {
            // This throws an exception if the location is not found. That is why this is a try function.
            city = await meilisearchClient.Index("cities").GetDocumentAsync<City>(locationId);
        }
        catch
        {
            return null;
        }
        
        // Get redis database key with GetRedisId() method 
        var redisKey = $"{type}:{city.GetRedisId()}";

        // Check if we have a cached response
        var db = redisConnection.GetDatabase();
        byte[]? cachedResponse = await db.StringGetAsync(redisKey);

        ForecastModel? forecast = null;

        if (cachedResponse != null)
        {
            var cacheString = Encoding.UTF8.GetString(cachedResponse);

            // Deserialize the cached response
            var cached = JsonSerializer.Deserialize<CachedForecast>(cacheString);

            // Check if the cached response is valid.
            if (cached is { IsExpired: false }) // Checks both null and IsExpired == false
            {
                forecast = cached.ForecastModel;
            }
        }

        // Return the cached response if it is valid
        if (forecast != null) return forecast;

        try
        {
            forecast = await forecastService(city.PositionModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        // Generate the weather description if a generator is provided
        weatherDescriptionGenerator?.SetWeatherDescription(ref forecast);

        var cacheTimespan = TimeSpan.FromSeconds(duration);
        var cachedForecast = JsonSerializer.Serialize(new CachedForecast(forecast, cacheTimespan));
        
        // Cache the response
        await db.StringSetAsync(redisKey, cachedForecast, cacheTimespan); // Cache for 15 minutes

        return forecast;
    }
    
    [GeneratedRegex("^[a-zA-Z0-9_-]+$")]
    public static partial Regex IdRegex();
}