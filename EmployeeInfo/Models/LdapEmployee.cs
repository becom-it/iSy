using FlintSoft.Ldap;
using System.Collections.Generic;

namespace EmployeeInfo.Models
{
    public class LdapEmployee
    {
        [LdapUser("samaccountname")]
        public string Account { get; set; }
        [LdapUser("displayName")]
        public string DisplayName { get; set; }

        [LdapUser("thumbnailphoto")]
        public string Photo { get; set; }

        [LdapUser("distinguishedname")]
        public string DistinguishedName { get; set; }

        [LdapUser("givenname")]
        public string FirstName { get; set; }

        [LdapUser("sn")]
        public string LastName { get; set; }

        [LdapUser("mail")]
        public string EMail { get; set; }

        [LdapUser("manager")]
        public string ManagerPath { get; set; }

        [LdapUser("directreports")]
        public List<string> DirectReportPaths { get; set; } = new List<string>();

        [LdapUser("title")]
        public string JobTitle { get; set; }

        [LdapUser("extensionattribute1")]
        public string Title { get; set; }

        [LdapUser("telephonenumber")]
        public string Phone { get; set; }

        [LdapUser("initials")]
        public string EmployeeId { get; set; }

        public  LdapEmployee Manager { get; set; }

        public List<LdapEmployee> DirectReports { get; set; } = new List<LdapEmployee>();

    }
}
