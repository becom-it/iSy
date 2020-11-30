using EmployeeData.Services;
using FlintSoft.StartupTasks.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EmployeeData.Extensions
{
    public static class EmployeeDataExtension
    {
        public static void AddEmployeeData(this IServiceCollection services)
        {
            services.TryAddScoped<IEmployeeService, EmployeeService>();
            services.TryAddSingleton(new LdapEmployeeCacheService());
            services.AddStartupTask<LdapEmployeeCacheStartupTask>();
        }
    }
}
