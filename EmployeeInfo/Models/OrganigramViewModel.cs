using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeInfo.Models
{
    public class OrganigramViewModel
    {
        public LdapEmployee Manager { get; set; } = null;
        public LdapEmployee Employee { get; set; } = null;
        public List<LdapEmployee> Employees { get; set; } = new List<LdapEmployee>();
    }
}
