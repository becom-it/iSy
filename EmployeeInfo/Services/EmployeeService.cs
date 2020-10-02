using EmployeeInfo.Models;
using FlintSoft.Ldap.Models;
using FlintSoft.Ldap.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfo.Services
{
    public interface IEmployeeService
    {
        Task<List<LdapEmployee>> SearchEmployee(string searchText);
        Task<LdapEmployee> LoadEmployee(string employeeId);
        Task<List<LdapEmployee>> SearchWithFilter(string filter, string path = "");
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly ILdapService _ldapService;

        public EmployeeService(ILogger<EmployeeService> logger, ILdapService ldapService)
        {
            _logger = logger;
            _ldapService = ldapService;
        }

        public async Task<List<LdapEmployee>> SearchEmployee(string searchText)
        {
            try
            {
                var filter = $"(&(objectCategory=person)(|(samaccountname=*{searchText}*)(givenname=*{searchText}*)(sn=*{searchText}*)(cn=*{searchText}*)))";
                return await _ldapService.Search<LdapEmployee>(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching for employee with search text {searchText}: {ex.Message}");
                return new List<LdapEmployee>();
            }
            
        } 

        public async Task<LdapEmployee> LoadEmployee(string employeeId)
        {
            try
            {
                var filter = $"(samaccountname={employeeId})";
                var foundEmp = await _ldapService.Search<LdapEmployee>(filter);
                if(foundEmp.Count == 0)
                {
                    throw new Exception("No employee found!");
                } else if(foundEmp.Count > 1)
                {
                    throw new Exception($"Found {foundEmp.Count} employees! Expected to find only 1 result.");
                } else
                {
                    return foundEmp.First();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading ldap employee with id {employeeId}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<LdapEmployee>> SearchWithFilter(string filter, string path = "")
        {
            try
            { 
                return await _ldapService.Search<LdapEmployee>(filter, path);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching for employee with filter {filter}: {ex.Message}");
                return new List<LdapEmployee>();
            }

        }
    }
}
