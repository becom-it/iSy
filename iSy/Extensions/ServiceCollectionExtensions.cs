﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static TSettings AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration)
        where TSettings : class, new()
        {
            return services.AddConfig<TSettings>(configuration, options => { });
        }

        public static TSettings AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration, Action<BinderOptions> configureOptions)
            where TSettings : class, new()
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            TSettings setting = configuration.Get<TSettings>(configureOptions);
            services.TryAddSingleton(setting);
            return setting;
        }
    }
}
