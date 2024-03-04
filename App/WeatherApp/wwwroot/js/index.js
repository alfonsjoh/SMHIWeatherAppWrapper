const locationInput = $("#location-input");

const forecastView = $("#forecast-view")
const forecast10View = $("#forecast10-view")
const forecastDescription = $("#forecast-description")

const autoCompleteView = $("#location-autocomplete")

let currentLocation = {name: "", index: ""};

// Toggles if wind should be shown in the 10 day forecast
const SHOW_WIND = true;

async function getForecast(location){
    let result = await fetch("/api/weather/forecast?" + new URLSearchParams({location: location}));
    if (!result.ok){
        return null;
    }
    return await result.json();
}

async function getForecast10(location){
    let result = await fetch("/api/weather/forecast10?" + new URLSearchParams({location: location}));
    if (!result.ok){
        return null;
    }
    return await result.json();
}

function kelvinToCelsius(kelvin){
    return (kelvin - 273.15).toFixed(0)
}

function getDayName(dateStr, locale) {
    let date = new Date(dateStr);
    return date.toLocaleDateString(locale, { weekday: 'long' });
}

function setLocation(location){
    currentLocation = location;
    locationInput.val(currentLocation.name);
}

function getLocationNameInput(){
    return locationInput.val();
}

function getForecastWeatherView(weather){
    // Gets hours from DateTime in weather
    let date = new Date(Date.parse(weather["dateTime"]));
    let hours = date.getHours().toString();
    let minutes = date.getMinutes().toString().padStart(2, '0')
    
    let weather_div = $("<div></div>")
        .addClass("weather");
    
    let time = $("<p></p>")
        .addClass("weather-time")
        .text(`${hours}:${minutes}`);
    
    let icon = $('<img>')
        .attr("alt", weather["icon"]["alternative"])
        .attr("src", weather["icon"]["source"]) // Weather icon
        .addClass("weather-icon");

    let wind = $("<p></p>")
        .addClass("wind")
        .text(weather["windSpeed"].toFixed(1).replace(".", ",") + " m/s");
    
    let temperature = $("<p></p>")
        .addClass("temperature")
        .text(kelvinToCelsius(weather["temperature"]) + "°");
    
    weather_div.append(time, icon);
    if (SHOW_WIND) {
        weather_div.append(wind);
    }
    weather_div.append(temperature)
    
    return weather_div;
}

function getForecast10WeatherView(weather) {
    // Get name of day
    let dayName = getDayName(weather["dateTime"], "sv-SE");
    
    let weather_div = $("<div></div>")
        .addClass("weather10");
    
    let day = $("<p></p>")
        .addClass("day")
        .text(dayName);
    
    let rain = $("<p></p>")
        .addClass("precipitation");
    if (weather["precipitation"] !== 0) {
        rain.text(weather["precipitation"].toFixed(1).replace(".", ",") + " mm")
    }

    let wind = $("<p></p>")
        .addClass("wind");
    if (SHOW_WIND) {
        wind.text(weather["windSpeed"].toFixed(1).replace(".", ",") + " m/s")
    }
    
    let icon = $('<img/>')
        .attr("alt", weather["icon"]["alternative"])
        .attr("src", weather["icon"]["source"]) // Weather icon
        .addClass("weather-icon");
    
    let lowestTemperature = $("<p></p>")
        .addClass("temperature")
        .text(kelvinToCelsius(weather["lowTemp"]) + "°");

    let highestTemperature = $("<p></p>")
        .addClass("temperature")
        .text(kelvinToCelsius(weather["highTemp"]) + "°");
    
    weather_div.append(day, rain, wind, icon, lowestTemperature, highestTemperature);
    
    return weather_div;
}

function updateForecastView(forecast){
    // Clear all contents of the forecast view
    forecastView.innerHTML = "";
    let weathers = forecast["prognosis"].map(getForecastWeatherView);
    
    forecastView.append(...weathers);
    
    forecastDescription.text(forecast["description"]);
    if (forecast["description"] === ""){
        forecastDescription.addClass("hidden");
    }
    else {
        forecastDescription.removeClass("hidden");
    }
}

function updateForecast10View(forecast){
    // Clear all contents of the forecast view
    forecastView.innerHTML = "";
    let prognosis = forecast["prognosis"].map(getForecast10WeatherView);

    forecast10View.append(...prognosis);
}

async function updateWeather(){
    if (currentLocation.index === ""){
        return;
    }
    const [forecast, forecast10] = await Promise.all([
        getForecast(currentLocation.index),
        getForecast10(currentLocation.index)]);
    
    if (forecast == null || forecast10 == null){
        return;
    }

    updateForecastView(forecast);
    updateForecast10View(forecast10);
}

async function searchCities(location){
    let result = await fetch("/api/search/cities?" + new URLSearchParams({q: location}));
    if (!result.ok){
        return null;
    }
    return await result.json();
}

async function getCity(id) {
    let result = await fetch("/api/search/cities/" + encodeURIComponent(id));
    if (!result.ok){
        return null;
    }
    return await result.json();
}

async function autoCompleteSearch(){
    let location = getLocationNameInput();
    let cities = await searchCities(location);
    
    autoCompleteView.empty();
    autoCompleteView.append(...cities.map((city) => 
        $("<p></p>")
            .text(city.name)
            .on("click", () => {
                autoCompleteView.empty();
                setLocation(city);
                setSearchParams();
            })
    ));
}

async function setLocationBySearchParams(){
    const urlParams = new URLSearchParams(window.location.search);
    
    if (!urlParams.has("q")){
        return;    
    }
    let query = urlParams.get("q");
    
    let city = await getCity(query);
    setLocation(city);
}

function setSearchParams(){
    let location = getLocationNameInput();
    if (location === ""){
        return;
    }
    window.location.search = new URLSearchParams({'q': currentLocation.index}).toString()
}

function setupEvents(){
    locationInput.on("input", async () => {
        await autoCompleteSearch();
    });
    
    /*
    document.addEventListener("focusin", async (e) => {
        console.log("focusin document");
        if (document.activeElement === locationInput){
            console.log("return")
            return;
        }
        autoCompleteView.classList.add("hidden");
    });
    locationInput.addEventListener("focusin", async () => {
        console.log("focusin locationInput");
        autoCompleteView.classList.remove("hidden");
    });*/
}

setLocationBySearchParams().then(async () => {
    await updateWeather();
});


setupEvents();