using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Weather.Extensions;
using Weather.Models;

namespace Weather.Services
{
    public interface IWeatherService {
        Task<(List<WeatherViewModel> vm, bool fromCache)> LoadWeather();
    }

    public class WeatherService: IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly WeatherConfig _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherCache _cache;

        public WeatherService(ILogger<WeatherService> logger, WeatherConfig config, IHttpClientFactory httpClientFactory, WeatherCache cache)
        {
            _logger = logger;
            _config = config;
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public async Task<(List<WeatherViewModel> vm, bool fromCache)> LoadWeather()
        {
            _logger.LogInformation("Retrieving weather data...");
            var (data, fromCache) = await loadData();
            _logger.LogInformation("Mapping weather data to view model...");
            return (data.MapToViewModel(_config), fromCache);
        }

        private async Task<(OpenWeatherResponse data, bool fromCache)> loadData()
        {
            var fromCache = true;
            _logger.LogInformation("Checking cache first...");
            if (DateTime.Now.Subtract(_cache.LastCheck) >= TimeSpan.FromMinutes(30))
            {
                _logger.LogInformation("Cache expired! Loading weather data from openweather map...");
                var groups = _config.Companies.Select(x => x.MapId.ToString()).Aggregate((a, x) => $"{a},{x}");
                _logger.LogDebug($"Loading weather data for map ids: {groups}...");
                var client = _httpClientFactory.CreateClient("owm");
                var res = await client.GetAsync($"group?id={groups}");
                
                if (res.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Request successfull! Deserialyzing data...");
                    var body = await res.Content.ReadAsStringAsync();
                    var data = await JsonSerializer.DeserializeAsync<OpenWeatherResponse>(await res.Content.ReadAsStreamAsync());
                    _cache.Data = data;
                    _cache.LastCheck = DateTime.Now;
                    fromCache = false;
                }
            }
            return (_cache.Data, fromCache);
        }
    }
}
