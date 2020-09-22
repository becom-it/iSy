using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Weather.Models;

namespace Weather.Components
{
    public partial class WeatherList
    {
        [Inject]
        public WeatherConfig WeatherConfig { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Console.WriteLine(WeatherConfig.AppId);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine(WeatherConfig.AppId);
        }
    }
}
