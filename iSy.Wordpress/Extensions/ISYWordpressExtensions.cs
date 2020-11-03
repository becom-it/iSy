using iSy.Wordpress.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace iSy.Wordpress.Extensions
{
    public static class ISYWordpressExtensions
    {
        public static void AddiSyWordpress(this IServiceCollection services)
        {
            services.TryAddScoped<IWordpressService, WordpressService>();
        }
    }
}
