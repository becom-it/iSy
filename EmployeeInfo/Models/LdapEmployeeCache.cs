using System;
using System.Collections.Generic;

namespace EmployeeInfo.Models
{
    public class LdapEmployeeCache
    {
        public DateTime Created { get; set; }
        public List<LdapEmployee> Employees { get; set; }
    }
}
