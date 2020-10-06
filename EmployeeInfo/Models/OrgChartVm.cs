using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeInfo.Models
{
    public class OrgChartVm
    {
        public OrgChargEmp Manager { get; set; }
        public OrgChargEmp Employee { get; set; }
        public List<OrgChargEmp> Employees { get; set; } = new List<OrgChargEmp>();
    }
}
