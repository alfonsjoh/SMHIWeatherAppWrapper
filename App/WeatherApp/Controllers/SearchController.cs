using System.Text.RegularExpressions;
using Meilisearch;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

namespace WeatherApp.Controllers;

[Route("/api/search")]
public partial class SearchController : Controller
{
    private readonly ILogger<SearchController> _logger;
    private readonly MeilisearchClient _meilisearchClient;

    public SearchController(ILogger<SearchController> logger, MeilisearchClient meilisearchClient)
    {
        _logger = logger;
        _meilisearchClient = meilisearchClient;
    }
    
    [HttpGet("cities")]
    public async Task<IResult> Cities(string? q)
    {
        if (q == null)
        {
            return Results.BadRequest("Query is required");
        }

        var index = _meilisearchClient.Index("cities");
        var results = await index.SearchAsync<City>(q, new SearchQuery { Limit = 5 });
        
        return Results.Json(results.Hits.Select(city => city.ToApiCity()));
    }
    
    [HttpGet("cities/{id}")]
    public async Task<IResult> City(string id)
    {
        // Validate that the id is safe for sending to Meilisearch
        if (!CachedForecast.IdRegex().IsMatch(id))
        {
            return Results.BadRequest("The provided city id is not valid.");
        }
        
        City city;
        try
        {
            city = await _meilisearchClient.Index("cities").GetDocumentAsync<City>(id);
        }
        catch (MeilisearchApiError error)
        {
            // If the document is not found, return a 404
            if (error.Code == "document_not_found")
            {
                return Results.BadRequest("The provided city id does not exist.");
            }

            // This should not occur and therefor it is important to log it
            _logger.LogError(error, "Failed to retrieve city");
            return Results.Problem("Internal server error. Failed to retrieve city.");
        }
        
        
        return Results.Json(city.ToApiCity());
    }
}