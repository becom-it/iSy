using Becom.EDI.PersonalDataExchange.Model.Config;
using Becom.EDI.PersonalDataExchange.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Becom.EDI.PersonalDataExchange.Extensions
{
    public static class PersonalDataExchangeExtensions
    {
        public static void AddPersonalDataExchange(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<IZeiterfassungsService, ZeiterfassungsService>();

            var pdeConfig = new PersonalDataExchangeConfig();
            configuration.GetSection("EdiConfig").Bind(pdeConfig);
            services.TryAddSingleton(pdeConfig);

            services.AddHttpClient("edi", c =>
            {
                c.BaseAddress = new Uri(pdeConfig.Endpoint);
            });


            services.TryAddScoped<IIBMiSQLApi, IBMiSQLApi>();

            var epConf = new EndpointConfiguration();
            configuration.GetSection("SqlEndpoint").Bind(epConf);
            services.TryAddSingleton(epConf);

            services.AddHttpClient("sqlapi", c =>
            {
                c.BaseAddress = new Uri(epConf.Api);
            });
        }
    }
}
