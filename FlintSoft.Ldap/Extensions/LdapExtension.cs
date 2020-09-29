using FlintSoft.Ldap.Models;
using FlintSoft.Ldap.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FlintSoft.Ldap.Extensions
{
    public static class LdapExtension
    {
        public static void AddLdap(this IServiceCollection services, IConfiguration configuration)
        {
            var ldapConfig = new LDAPConfiguration();
            configuration.Bind("LDAP", ldapConfig);
            services.AddSingleton(ldapConfig);

            services.TryAddScoped<ILdapService, LdapService>();
        }
    }
}
