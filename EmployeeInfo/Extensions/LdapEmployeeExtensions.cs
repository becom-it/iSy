using EmployeeData.Models;
using EmployeeInfo.Models;

namespace EmployeeInfo.Extensions
{
    public static class LdapEmployeeExtensions
    {
        public static OrgChargEmp MapToOrgChargEmp(this LdapEmployee emp)
        {
            if (emp != null)
            {
                return new OrgChargEmp
                {
                    Id = emp.DistinguishedName,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Photo = emp.Photo,
                    JobTitle = emp.JobTitle,
                    IsCurrent = emp.IsCurrent,
                    Tel = emp.Phone,
                    Email = emp.EMail
                };
            }
            else return null;
        }
    }
}
