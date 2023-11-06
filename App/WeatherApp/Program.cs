using Meilisearch;
using StackExchange.Redis;
using WeatherApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddResponseCaching();

builder.Services.AddHttpClient(
    "weather",
    client =>
    {
        client.BaseAddress = new Uri("https://api.openweathermap.org/data/");
    }
);

var redis = await ConnectionMultiplexer.ConnectAsync("localhost");
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);


builder.Services.AddSingleton<IWeatherService>(provider =>
    //new WeatherService(provider.GetService<IHttpClientFactory>()!)
    new RandomWeatherService()
);

// Connect to meilisearch
var meilisearchClient = new MeilisearchClient("http://localhost:7700", Environment.GetEnvironmentVariable("meilisearch_master_key"));
builder.Services.AddSingleton(meilisearchClient);

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