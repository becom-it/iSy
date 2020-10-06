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
        Task<LdapEmployee> LoadEmployeeWithId(string employeeId);
        Task<LdapEmployee> LoadEmployeeWithPath(string path);
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

        public async Task<LdapEmployee> LoadEmployeeWithId(string employeeId)
        {
            try
            {
                var filter = $"(samaccountname={employeeId})";
                var foundEmp = await _ldapService.Search<LdapEmployee>(filter);
                if (foundEmp.Count == 0)
                {
                    throw new Exception("No employee found!");
                }
                else if (foundEmp.Count > 1)
                {
                    throw new Exception($"Found {foundEmp.Count} employees! Expected to find only 1 result.");
                }
                else
                {
                    var emp = foundEmp.First();
                    return await loadDetails(emp);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading ldap employee with id {employeeId}: {ex.Message}");
                return null;
            }
        }

        public async Task<LdapEmployee> LoadEmployeeWithPath(string path)
        {
            try
            {

                var foundEmp = await _ldapService.Search<LdapEmployee>("(objectclass=*)", path);
                if (foundEmp.Count == 0)
                {
                    throw new Exception("No employee found!");
                }
                else if (foundEmp.Count > 1)
                {
                    throw new Exception($"Found {foundEmp.Count} employees! Expected to find only 1 result.");
                }
                else
                {
                    var emp = foundEmp.First();
                    return await loadDetails(emp);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading ldap employee with path {path}: {ex.Message}");
                return null;
            }
        }

        private async Task<LdapEmployee> loadDetails(LdapEmployee emp)
        {
            try
            {
                //Manager laden
                var manager = await SearchWithFilter("(objectclass=*)", emp.ManagerPath);
                if (manager.Count > 0)
                {
                    emp.Manager = manager.First();
                }
                else
                {
                    throw new Exception($"Error loading the manager! (Found {manager.Count})");
                }

                //Surrogates laden
                if (emp.DirectReportPaths.Count() == 0)
                {
                    //Der Mitarbeiter hat keine Mitarbeiter -> Die vom Manager holen
                    foreach (var dp in emp.Manager.DirectReportPaths)
                    {
                        if (emp.DistinguishedName != dp)
                        {
                            var e = await SearchWithFilter("(objectclass=*)", dp);
                            if (e.Count > 0)
                            {
                                emp.Manager.DirectReports.Add(e.First());
                            }
                            else
                            {
                                throw new Exception($"Error loading the direct report employee! (Found {e.Count})");
                            }
                        }
                        else
                        {
                            emp.Manager.DirectReports.Add(emp);
                        }
                    }

                    //In diesem Fall auch den Manager vom Manager laden
                    var mm = await SearchWithFilter("(objectclass=*)", emp.Manager.ManagerPath);
                    if (mm.Count > 0)
                    {
                        emp.Manager.Manager = mm.First();
                    }
                    else
                    {
                        throw new Exception($"Error loading the manager of the manager! (Found {mm.Count})");
                    }
                }
                else
                {
                    //Die Mitarbeiter des Mitarbeiter laden
                    foreach (var dp in emp.DirectReportPaths)
                    {
                        var e = await SearchWithFilter("(objectclass=*)", dp);
                        if (e.Count > 0)
                        {
                            emp.DirectReports.Add(e.First());
                        }
                        else
                        {
                            throw new Exception($"Error loading the direct report employee! (Found {e.Count})");
                        }

                    }
                }
                return emp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading the details for employee {emp.DistinguishedName}: {ex.Message}");
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
