using EmployeeData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeData.Services
{
    public interface ILdapEmployeeCacheService
    {
        Task<List<LdapEmployee>> GetEmployees();
        void UpdateCache(LdapEmployeeCache cache);
    }

    public class LdapEmployeeCacheService : ILdapEmployeeCacheService
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