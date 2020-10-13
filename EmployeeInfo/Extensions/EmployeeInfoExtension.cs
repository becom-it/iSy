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

        //public static void UseEmployeeInfo(this IApplicationBuilder app)
        //{
        //    var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
        //    var empService = app.ApplicationServices.GetRequiredService<IEmployeeService>();

        //    var path = env.ContentRootPath;
        //    if (File.Exists(Path.Combine(path, "ldap", "cache.json")))
        //    {

        //    }

        //    //Kein Cache oder zu alt -> alle Ldaps laden
        //    var emps = empService.SearchWithFilter("(&(objectCategory = person)(objectClass = user)(givenName = *)(sn = *))").Result;
        //    var cache = new LdapEmployeeCache
        //    {
        //        Created = DateTime.Now,
        //        Employees = emps
        //    };

        //    prov.
        //}
    }
}
