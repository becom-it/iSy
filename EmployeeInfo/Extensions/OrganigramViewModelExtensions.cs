using EmployeeInfo.Models;
using System.Linq;

namespace EmployeeInfo.Extensions
{
    public static class OrganigramViewModelExtensions
    {
        public static OrgChartVm PrepareForJs(this OrganigramViewModel vm)
        {
            var ret = new OrgChartVm();
            ret.Manager = vm.Manager.MapToOrgChargEmp();
            ret.Employee = vm.Employee.MapToOrgChargEmp();
            ret.Employees = vm.Employees.Select(x => x.MapToOrgChargEmp()).ToList();
            return ret;
        }
    }
}
