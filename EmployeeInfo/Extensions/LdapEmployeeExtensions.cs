using EmployeeInfo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeInfo.Extensions
{
    public static class LdapEmployeeExtensions
    {
        public static string GetManagerUserName(this LdapEmployee emp)
        {
            if (String.IsNullOrEmpty(emp.Manager)) return string.Empty;
            return "";
        }
    }
}
