const locationInput = $("#location-input");
const forecastView = $("#forecast-view")
const autoCompleteView = $("#location-autocomplete")

let currentLocation = null;

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
    return (kelvin - 273.15).toFixed(2)
}

function setLocation(location){
    currentLocation = location;
    locationInput.val(currentLocation.name);
}

function getLocationNameInput(){
    return locationInput.val();
}

function getForecastWeatherView(weather){
    let weather_div = $("<div></div>")
        .addClass("weather");
    
    let temperature = $("<p></p>")
        .addClass("weather-temp")
        .text(kelvinToCelsius(weather["temperature"]) + "°");
    
    let time = $("<p></p>")
        .addClass("weather-time");
    
    let icon = $('<img alt="weather-icon">')
        .attr("src", weather["icon"]) // Weather icon
        .addClass("weather-icon");

    temperature.innerText = kelvinToCelsius(weather["temperature"]);
    
    // Gets hours from DateTime in weather
    let date = new Date(Date.parse(weather["dateTime"]));
    let hours = date.getHours().toString();
    let minutes = date.getMinutes().toString().padStart(2, '0')
    
    time.innerText = `${hours}:${minutes}`;
    
    weather_div.append(temperature, time, icon);
    
    return weather_div;
}

function getForecast10WeatherView(weather){
    let weather_div = $("<div></div>")
        .addClass("weather10");
    
    

function updateForecastView(forecast){
    // Clear all contents of the forecast view
    forecastView.innerHTML = "";
    let weathers = forecast["prognosis"].map(getForecastWeatherView);
    
    forecastView.append(...weathers);
}

async function updateWeather(){
    if (currentLocation.index === ""){
        return;
    }
    console.log(currentLocation)
    let forecast = await getForecast(currentLocation.index);
    if (forecast == null){
        return;
    }
    
    updateForecastView(forecast);
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