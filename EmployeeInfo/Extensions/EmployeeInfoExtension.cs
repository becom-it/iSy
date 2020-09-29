using EmployeeInfo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EmployeeInfo.Extensions
{
    public static class EmployeeInfoExtension
    {
        public static void AddEmployeeInfo(this IServiceCollection services)
        {
            services.TryAddScoped<IEmployeeService, EmployeeService>();
        }
    }
}
