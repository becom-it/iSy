using EmployeeInfo.Models;
using FlintSoft.StartupTasks.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeInfo.Services
{


    public class LdapEmployeeCacheStartupTask : IStartupTask
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LdapEmployeeCacheService _cacheService;

        public LdapEmployeeCacheStartupTask(IServiceProvider serviceProvider, LdapEmployeeCacheService ldapEmployeeCacheService)
        {
            _serviceProvider = serviceProvider;
            _cacheService = ldapEmployeeCacheService;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var env = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                var empService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

                var path = env.ContentRootPath;
                var jsonDir = Path.Combine(path, "ldap");
                if (!Directory.Exists(jsonDir)) Directory.CreateDirectory(jsonDir);

                var jsonPath = Path.Combine(jsonDir, "cache.json");
                if (File.Exists(jsonPath))
                {
                    using(var sr = new StreamReader(jsonPath))
                    {
                        string json = sr.ReadToEnd();
                        var tCache = JsonSerializer.Deserialize<LdapEmployeeCache>(json);
                        if(DateTime.Now.Subtract(tCache.Created) < TimeSpan.FromDays(10))
                        {
                            _cacheService.UpdateCache(tCache);
                            return;
                        }
                    }
                }

                //Kein Cache oder zu alt -> alle Ldaps laden
                var emps = await empService.SearchWithFilter("(&(objectCategory=person)(objectClass=user)(givenName=*)(sn=*))");
                var cache = new LdapEmployeeCache
                {
                    Created = DateTime.Now,
                    Employees = emps
                };
                _cacheService.UpdateCache(cache);

                //Write Cache to disk
                var str = JsonSerializer.Serialize(cache);
                var buffer = Encoding.UTF8.GetBytes(str);
                using(var fs = new FileStream(jsonPath, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, true))
                {
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
