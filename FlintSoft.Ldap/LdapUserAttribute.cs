using System;

namespace FlintSoft.Ldap
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LdapUserAttribute : Attribute
    {
        public string AttributeName = "Uknown";

        public LdapUserAttribute()
        {

        }

        public LdapUserAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }
    }
}
