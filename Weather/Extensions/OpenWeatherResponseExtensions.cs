using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weather.Models;

namespace Weather.Extensions
{
    public static class OpenWeatherResponseExtensions
    {
        public static List<WeatherViewModel> MapToViewModel(this OpenWeatherResponse raw, WeatherConfig config)
        {
            var l = new List<WeatherViewModel>();
            
            if (raw == null) return l;

            foreach(var r in raw.OpenWeatherList)
            {
                var c = config.Companies.First(x => x.MapId == r.Id);
                var vm = new WeatherViewModel
                {
                    MapId = c.MapId,
                    Description = c.Description,
                    City = c.City,
                    Sunrise = DateTimeOffset.FromUnixTimeMilliseconds(r.Sys.Sunrise).DateTime,
                    Sunset = DateTimeOffset.FromUnixTimeMilliseconds(r.Sys.Sunset).DateTime,
                    Temperature = r.Main.Temp,
                    WeatherType = r.Weather.First().Main,
                    WeatherIcon = r.Weather.First().Icon.TranslateIcon()
                };
                l.Add(vm);
            }
            
            return l;
        }

        public static string TranslateIcon(this string icon)
        {
            switch (icon)
            {
                case "11d":
                    return "thunderstorm.png";
                case "09d":
                    return "shower_rain.png";
                case "10d":
                    return "rain.png";
                case "10n":
                    return "rain.png";
                case "13d":
                    return "snow.png";
                case "50d":
                    return "mist.png";
                case "50n":
                    return "mist.png";
                case "01d":
                    return "clear_day.png";
                case "01n":
                    return "clear_night.png";
                case "02d":
                    return "fewclouds_day.png";
                case "02n":
                    return "fewclouds_night.png";
                case "03d":
                    return "clouds_day.png";
                case "03n":
                    return "clouds_day.png";
                case "04d":
                    return "broken_clouds_day.png";
                case "04n":
                    return "broken_clouds_day.png";
                default:
                    return "";
            }
        }
    }

    public class WeatherViewModel
    {
        public int MapId { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public double Temperature { get; set; }
        public string WeatherType { get; set; }
        public string WeatherIcon { get; set; }
    }
}
