using EmployeeData.Models;
using FlintSoft.Ldap.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeData.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        Task<List<LdapEmployee>> SearchEmployee(string searchText);
        Task<LdapEmployee> LoadEmployeeWithId(string employeeId);
        Task<LdapEmployee> LoadEmployeeWithPath(string path, bool noDetail = false);
        Task<List<LdapEmployee>> SearchWithFilter(string filter, IEnumerable<int> employeeIds = null, string path = "");
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly ILdapService _ldapService;
        private readonly LdapEmployeeCacheService _cache;

        public EmployeeService(ILogger<EmployeeService> logger, ILdapService ldapService, LdapEmployeeCacheService cache)
        {
            _logger = logger;
            _ldapService = ldapService;
            _cache = cache;
        }

        public async Task<List<LdapEmployee>> SearchEmployee(string searchText)
        {
            try
            {
                return (await _cache.GetEmployees()).FindAll(x => x
                .GetType()
                .GetProperties()
                .Where(y => y.PropertyType == typeof(string))
                .Select(y => (string)y.GetValue(x, null))
                .Where(y => y != null)
                .Any(y => y.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)));
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
                var foundEmp = (await _cache.GetEmployees()).Where(x => x.Account.ToUpper() == employeeId.ToUpper()).ToList();
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

        public async Task<LdapEmployee> LoadEmployeeWithPath(string path, bool noDetail = false)
        {
            try
            {
                var foundEmp = (await _cache.GetEmployees()).Where(x => x.DistinguishedName == path).ToList();
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
                    if (noDetail)
                    {
                        emp.IsCurrent = false;
                        return emp;
                    }
                    else
                    {
                        emp.IsCurrent = true;
                        return await loadDetails(emp);
                    }
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
                if (!string.IsNullOrEmpty(emp.ManagerPath))
                {
                    var manager = await LoadEmployeeWithPath(emp.ManagerPath, true);
                    if (manager != null)
                    {
                        emp.Manager = manager;
                    }
                    else
                    {
                        throw new Exception($"Error loading the manager! (Found no manager)");
                    }
                }
                               

                //Surrogates laden
                if (emp.DirectReportPaths.Count == 0)
                {
                    //Der Mitarbeiter hat keine Mitarbeiter -> Die vom Manager holen
                    emp.Manager.DirectReports.Clear();
                    foreach (var dp in emp.Manager.DirectReportPaths)
                    {
                        if (dp.Contains("OU=Z_Obsolete User") || dp.Contains("OU=BECOM HU")) continue;
                        if (emp.DistinguishedName != dp)
                        {
                            try
                            {
                                var e = await LoadEmployeeWithPath(dp, true);
                                if (e != null)
                                {
                                    emp.Manager.DirectReports.Add(e);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Error loading the direct report employee! (Found no employee) for path {dp}: {ex.Message}");
                            }
                            //var e = await LoadEmployeeWithPath(dp, true);
                            //if (e != null)
                            //{
                            //    emp.Manager.DirectReports.Add(e);
                            //}
                            //else
                            //{
                            //    throw new Exception($"Error loading the direct report employee! (Found no employee)");
                            //}
                        }
                        else
                        {
                            emp.Manager.DirectReports.Add(emp);
                        }
                    }

                    //In diesem Fall auch den Manager vom Manager laden
                    var mm = await LoadEmployeeWithPath(emp.Manager.ManagerPath, true);
                    if (mm != null)
                    {
                        emp.Manager.Manager = mm;
                    }
                    else
                    {
                        throw new Exception($"Error loading the manager of the manager! (Found no manager)");
                    }
                }
                else
                {
                    emp.DirectReports.Clear();

                    //Die Mitarbeiter des Mitarbeiter laden
                    foreach (var dp in emp.DirectReportPaths)
                    {
                        if (dp.Contains("OU=Z_Obsolete User") || dp.Contains("OU=BECOM HU")) continue;

                        try
                        {
                            var e = await LoadEmployeeWithPath(dp, true);
                            if (e != null)
                            {
                                emp.DirectReports.Add(e);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error loading the direct report employee! (Found no employee) for path {dp}: {ex.Message}");
                        }

                        //var e = await LoadEmployeeWithPath(dp, true);
                        //if (e != null)
                        //{
                        //    emp.DirectReports.Add(e);
                        //}
                        //else
                        //{
                        //    throw new Exception($"Error loading the direct report employee! (Found no employee)");
                        //}
                    }
                }
                return emp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading the details for employee {emp.DistinguishedName}: {ex.Message}");
            }
        }

        public async Task<List<LdapEmployee>> SearchWithFilter(string filter, IEnumerable<int> employeeIds = null, string path = "")
        {
            try
            {
                return await _ldapService.Search<LdapEmployee>(filter, employeeIds, path);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching for employee with filter {filter}: {ex.Message}");
                return new List<LdapEmployee>();
            }

        }
    }
}
