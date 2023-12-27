using WeatherApp.Models.Weather.Models;

namespace WeatherApp.Models.Toolbox;

public class SunInfoCalculator
{
    public static SunInfoModel GetSunriseSunsetTime(WorldPositionModel worldPositionModel, DateTime dateTime)
    {
        TimeZoneInfo timeZone = TimeZoneInfo.Local;
        double offsetHours = timeZone.GetUtcOffset(dateTime).TotalHours;

        // Calculate the day of the year
        int dayOfYear = dateTime.DayOfYear;

        double meanSolarNoon = (worldPositionModel.Lon / 360.0) + dayOfYear - 81.0;
        double solarNoon = meanSolarNoon - 7.0 * Math.Sin((360.0 * (meanSolarNoon - 81.0)) / 365.25);
        double solarMeanAnomaly = (solarNoon - 4.0 * worldPositionModel.Lon) - 180.0;

        double equationOfCenter = 1.9148 * Math.Sin(solarMeanAnomaly * Math.PI / 180.0)
                                  + 0.02 * Math.Sin(2.0 * solarMeanAnomaly * Math.PI / 180.0)
                                  + 0.0003 * Math.Sin(3.0 * solarMeanAnomaly * Math.PI / 180.0);
        
        double eclipticLongitude = solarMeanAnomaly + equationOfCenter + 102.9372 + 180.0;
        double solarTransit = solarNoon + 0.0053 * Math.Sin(solarMeanAnomaly * Math.PI / 180.0) - 0.0069 * Math.Sin(2.0 * eclipticLongitude * Math.PI / 180.0);

        double declination = Math.Asin(Math.Sin(eclipticLongitude * Math.PI / 180.0) * Math.Sin(23.44 * Math.PI / 180.0));
        double hourAngle = Math.Acos((Math.Sin(-0.83 * Math.PI / 180.0) - Math.Sin(worldPositionModel.Lat * Math.PI / 180.0) * Math.Sin(declination)) / (Math.Cos(worldPositionModel.Lat * Math.PI / 180.0) * Math.Cos(declination)));
        
        double sunrise = solarTransit - (hourAngle * 180.0 / Math.PI) / 360.0;
        double sunset = solarTransit + (hourAngle * 180.0 / Math.PI) / 360.0;

        DateTime sunriseTime = DateTime.Today.AddHours(sunrise * 24.0 + offsetHours);
        DateTime sunsetTime = DateTime.Today.AddHours(sunset * 24.0 + offsetHours);

        
        return new SunInfoModel(sunriseTime, sunsetTime);
    }
}