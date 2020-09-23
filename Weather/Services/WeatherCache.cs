using System;
using System.Collections.Generic;
using System.Text;
using Weather.Models;

namespace Weather.Services
{
    public class WeatherCache
    {
        public DateTime LastCheck { get; set; } = default;

        public OpenWeatherResponse Data { get; set; } = null;
    }
}
