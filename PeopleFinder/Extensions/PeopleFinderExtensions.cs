using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PeopleFinder.Services;

namespace PeopleFinder.Extensions
{
    public static class PeopleFinderExtensions
    {
        public static void AddPeopleFinder(this IServiceCollection services)
        {
            services.AddScoped<PeopleFinderState>();
        }
    }
}
