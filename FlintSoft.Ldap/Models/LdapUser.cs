using System;
using System.ComponentModel.DataAnnotations;

namespace FlintSoft.Ldap.Models
{
    public class LdapUser
    {
        [Key]
        [LdapUserAttribute("samaccountname")]
        public string Account { get; set; }
        [LdapUserAttribute("displayName")]
        public string DisplayName { get; set; }

        [LdapUserAttribute("thumbnailphoto")]
        public string Photo { get; set; }

        [LdapUserAttribute("distinguishedname")]
        public string DistinguishedName { get; set; }

        [LdapUserAttribute("givenname")]
        public string FirstName { get; set; }

        [LdapUserAttribute("sn")]
        public string LastName { get; set; }

        public DateTime CreationDate { get; set; }

        public bool DrinksCoffee { get; set; }
    }
}
