using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Meilisearch;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using WeatherApp.Models;

namespace WeatherApp.Controllers;

[Route("/api/weather")]
public class WeatherController : Controller
{
    private readonly ILogger<WeatherController> _logger;
    private readonly IWeatherService _weatherService;
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly MeilisearchClient _meilisearchClient;

    public WeatherController(
        ILogger<WeatherController> logger,
        IWeatherService weatherService,
        IConnectionMultiplexer redisConnection,
        MeilisearchClient meilisearchClient)
    {
        _logger = logger;
        _weatherService = weatherService;
        _redisConnection = redisConnection;
        _meilisearchClient = meilisearchClient;
    }

    [HttpGet("forecast")]
    [ResponseCache(VaryByQueryKeys = new[]{"location"}, Duration = CachedForecast.DefaultDuration)] // Cache response for 15 minutes
    public async Task<IResult> Forecast(string? location)
    {
        if (location == null)
        {
            return Results.BadRequest("Location is required");
        }
        
        var forecast = await CachedForecast.TryGetForecastByCacheOrServiceAsync(
            "forecast",
            location,
            _redisConnection,
            _meilisearchClient,
            _weatherService.GetForecastAsync
        );
        
        if (forecast == null)
        {
            return Results.NotFound("Could not find forecast. Likely due to an invalid location.");
        }
        
        return Results.Ok(forecast);
    }
    
    [HttpGet("forecast10")]
    [ResponseCache(VaryByQueryKeys = new[]{"location"}, Duration = CachedForecast.DefaultDuration)] // Cache response for 15 minutes
    public async Task<IResult> Forecast10(string? location)
    {
        if (location == null)
        {
            return Results.BadRequest("Location is required");
        }
        
        var forecast = await CachedForecast.TryGetForecastByCacheOrServiceAsync(
            "forecast10",
            location,
            _redisConnection,
            _meilisearchClient,
            _weatherService.Get10DayForecastAsync
        );
        
        if (forecast == null)
        {
            return Results.NotFound("Could not find forecast. Likely due to an invalid location.");
        }
        
        return Results.Ok(forecast);
    }
}