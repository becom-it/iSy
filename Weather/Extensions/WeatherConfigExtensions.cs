using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Weather.Models;

namespace Weather.Extensions
{
    public static class WeatherConfigExtensions
    {
        public static void AddWeatherConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\assets\\weatherconfig\\weather.json");
            //var json = System.IO.File.ReadAllText(path);
            //var wconfig = JsonSerializer.Deserialize<WeatherConfig>(json);

            var wconfig = new WeatherConfig();
            configuration.GetSection("WeatherConfig").Bind(wconfig);

            services.TryAddSingleton<WeatherConfig>(wconfig);
        }
    }
}
