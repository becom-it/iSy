using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Services
{
    public class OWMHandler :  DelegatingHandler
    {
        private readonly WeatherConfig _config;

        public OWMHandler(WeatherConfig config): base(new HttpClientHandler())
        {
            _config = config;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var url = request.RequestUri.ToString();
            url = $"{url}&units=metric&appid={_config.AppId}&lang=de";
            request.RequestUri = new Uri(url);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
