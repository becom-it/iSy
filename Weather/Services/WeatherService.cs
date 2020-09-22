using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Weather.Models;

namespace Weather.Services
{
    public interface IWeatherService { }
    public class WeatherService: IWeatherService
    {
        const string URL = "http://api.openweathermap.org/data/2.5/group?id=<MAPIDS>&units=metric&appid=<APPID>&lang=de";

        private readonly ILogger<WeatherService> _logger;
        private readonly WeatherConfig _config;

        public WeatherService(ILogger<WeatherService> logger, WeatherConfig config)
        {
            _logger = logger;
            _config = config;
        }

        public void LoadWeather()
        {

        }
    }
}
