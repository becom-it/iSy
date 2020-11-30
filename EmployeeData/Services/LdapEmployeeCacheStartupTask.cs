using FlintSoft.StartupTasks.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Json;
using EmployeeData.Models;
using Becom.EDI.PersonalDataExchange.Services;
using System.Linq;

namespace EmployeeData.Services
{
    public class LdapEmployeeCacheStartupTask : IStartupTask
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LdapEmployeeCacheService _cacheService;
        private readonly IZeiterfassungsService _zeiterfassungsService;

        public LdapEmployeeCacheStartupTask(IServiceProvider serviceProvider, LdapEmployeeCacheService ldapEmployeeCacheService, IZeiterfassungsService zeiterfassungsService)
        {
            _serviceProvider = serviceProvider;
            _cacheService = ldapEmployeeCacheService;
            _zeiterfassungsService = zeiterfassungsService;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var env = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                var empService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

                var path = env.ContentRootPath;
                var jsonDir = Path.Combine(path, "ldap");
                if (!Directory.Exists(jsonDir)) Directory.CreateDirectory(jsonDir);

                var jsonPath = Path.Combine(jsonDir, "cache.json");
                if (File.Exists(jsonPath))
                {
                    using (var sr = new StreamReader(jsonPath))
                    {
                        string json = sr.ReadToEnd();
                        var tCache = JsonSerializer.Deserialize<LdapEmployeeCache>(json);
                        if (DateTime.Now.Subtract(tCache.Created) < TimeSpan.FromDays(10))
                        {
                            _cacheService.UpdateCache(tCache);
                            return;
                        }
                    }
                }

                //Verfügbare Mitarbeite aus dem EDI laden...
                var ediEmployees = await _zeiterfassungsService.GetEmployeeList(Becom.EDI.PersonalDataExchange.Model.Enums.CompanyEnum.Austria);
                var ediEmployeeIds = ediEmployees.Select(x => x.EmployeeId);

                //Kein Cache oder zu alt -> alle Ldaps laden
                var emps = await empService.SearchWithFilter("(&(objectCategory=person)(objectClass=user)(givenName=*)(sn=*))", ediEmployeeIds);
                var cache = new LdapEmployeeCache
                {
                    Created = DateTime.Now,
                    Employees = emps
                };
                _cacheService.UpdateCache(cache);

                //Write Cache to disk
                var str = JsonSerializer.Serialize(cache);
                var buffer = Encoding.UTF8.GetBytes(str);
                using (var fs = new FileStream(jsonPath, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, true))
                {
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
