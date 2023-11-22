using Meilisearch;
using StackExchange.Redis;
using WeatherApp.Models;
using WeatherApp.Models.WeatherTipGeneration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddResponseCaching();

// Connect to redis
var redis = await ConnectionMultiplexer.ConnectAsync("localhost");
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// Connect to meilisearch
var meilisearchClient = new MeilisearchClient("http://localhost:7700", Environment.GetEnvironmentVariable("meilisearch_master_key"));
builder.Services.AddSingleton(meilisearchClient);

builder.Services.AddHttpClient(
    "weather",
    client =>
    {
        client.BaseAddress = new Uri("https://api.openweathermap.org/data/");
    }
);

builder.Services.AddSingleton<IWeatherService>(provider =>
    //new WeatherService(provider.GetService<IHttpClientFactory>()!)
    new RandomWeatherService()
);

// Http client for analysing weather data with google generative ai
// TODO - Fix url
builder.Services.AddHttpClient(
    "googleGPT",
    client =>
    {
        client.BaseAddress = new Uri("https://${API_ENDPOINT}/v1/projects/${PROJECT_ID}/locations/${LOCATION_ID}/publishers/google/models/${MODEL_ID}:predict");
    }
);




builder.Services.AddSingleton<IWeatherTipGenerator>(provider =>
    new RandomWeatherTipGenerator()
);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseResponseCaching();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();