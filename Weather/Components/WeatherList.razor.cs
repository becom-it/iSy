using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weather.Extensions;
using Weather.Models;
using Weather.Services;

namespace Weather.Components
{
    public partial class WeatherList
    {
        [Inject]
        public IWeatherService WeatherService { get; set; }

        public List<WeatherViewModel> WeatherData { get; set; }

        public bool FromCache { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var data = await WeatherService.LoadWeather();
                WeatherData = data.vm;
                FromCache = data.fromCache;

                StateHasChanged();
            }
        }
    }
}
