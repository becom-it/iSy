using EmployeeInfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeInfo.Services
{
    public class LdapEmployeeCacheService
    {
        private LdapEmployeeCache _empCache = null;

        public void UpdateCache(LdapEmployeeCache cache)
        {
            _empCache = cache;
        }

        public async Task<List<LdapEmployee>> GetEmployees()
        {
            return await Task.Run(() => _empCache.Employees);
        }
    }
}
