using FlintSoft.Ldap;

namespace iSy.Models
{
    public class LdapAuthUser
    {
        [LdapUser("sAMAccountName")]
        public string UserName { get; set; }

        [LdapUser("displayName")]
        public string DisplayName { get; set; }

        [LdapUser("initials")]
        public string EmployeeId { get; set; }

        [LdapUser("extensionAttribute4")]
        public string CompanyKey { get; set; }
    }
}
