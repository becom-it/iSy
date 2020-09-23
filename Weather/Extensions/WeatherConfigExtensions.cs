using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Weather.Models;
using Weather.Services;

namespace Weather.Extensions
{
    public static class WeatherConfigExtensions
    {
        public static void AddWeatherConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var wconfig = new WeatherConfig();
            configuration.GetSection("WeatherConfig").Bind(wconfig);

            services.TryAddSingleton(wconfig);

            services.TryAddSingleton<WeatherCache>();

            services.TryAddTransient<OWMHandler>();
            services.AddHttpClient("owm", c => {
                c.BaseAddress = new Uri(wconfig.Link);
            
            }).ConfigurePrimaryHttpMessageHandler<OWMHandler>();

            services.TryAddTransient<IWeatherService, WeatherService>();
        }
    }
}
