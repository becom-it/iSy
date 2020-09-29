using EmployeeInfo.Models;
using FlintSoft.Ldap.Models;
using FlintSoft.Ldap.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfo.Services
{
    public interface IEmployeeService
    {
        Task<List<LdapEmployee>> SearchEmployee(string searchText);
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
                return await _ldapService.Search<LdapEmployee>("OU=BECOM AT,DC=ad,DC=becom,DC=at", filter);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching for employee with search text {searchText}: {ex.Message}");
                return new List<LdapEmployee>();
            }
            
        } 
    }
}
