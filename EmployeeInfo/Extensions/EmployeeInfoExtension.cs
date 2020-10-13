using EmployeeInfo.Models;
using EmployeeInfo.Services;
using FlintSoft.Ldap.Services;
using FlintSoft.StartupTasks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace EmployeeInfo.Extensions
{
    public static class EmployeeInfoExtension
    {
        public static void AddEmployeeInfo(this IServiceCollection services)
        {
            services.TryAddScoped<IEmployeeService, EmployeeService>();
            services.TryAddSingleton(new LdapEmployeeCacheService());
            services.AddStartupTask<LdapEmployeeCacheStartupTask>();
        }
    }
}
