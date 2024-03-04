using Meilisearch;
using StackExchange.Redis;
using WeatherApp.Models.Weather;
using WeatherApp.Models.Weather.Models.smhi;
using WeatherApp.Models.WeatherTipGeneration;
using WeatherApp.Models.WeatherTipGeneration.LogicBased;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddResponseCaching();

// Connect to redis
var redis = await ConnectionMultiplexer.ConnectAsync("localhost");
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// Connect to meilisearch
var meilisearchClient = new MeilisearchClient("http://localhost:7700",
    Environment.GetEnvironmentVariable("meilisearch_master_key"));
builder.Services.AddSingleton(meilisearchClient);

builder.Services.AddSingleton<IconConverter>(_ =>
    new IconConverter("SMHIIcons.json")
);

WeatherServiceManager.ConfigureWeatherService(builder);


builder.Services.AddSingleton<IWeatherDescriptionGenerator>(provider =>
    new LogicBasedWeatherDescriptionGenerator()
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
