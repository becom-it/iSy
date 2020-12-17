using System;
using System.Linq;
using System.Threading.Tasks;
using FlintSoft.Ldap.Services;
using iSy.Models;
using Microsoft.Extensions.Logging;

namespace iSy.Services
{
    public interface IAuthenticationService
    {
        Task<LdapAuthUser> Login(string userName, string password);
    }

    public class LdapAuthenticationService : IAuthenticationService
    {
        private readonly ILdapService _ldapService;

        public LdapAuthenticationService(ILdapService ldapService)
        {
            _ldapService = ldapService;
        }

        public async Task<LdapAuthUser> Login(string userName, string password)
        {
            try
            {
                var conn = _ldapService.GetCustomLdapConnection(userName, password);

                var user = await _ldapService.CustomSearch<LdapAuthUser>(conn, $"(&(objectCategory=person)(objectClass=user)(samaccountname={userName}))");//$"(&(objectCategory=person)(objectClass=user)(sAMAccountName=mprattinge))"); //(givenName=Michael)(sn=Prattinger))");   //$"(sAMAccountName={userName})");

                return user.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when logging in: {ex.Message}", ex);
            }
        }
    }
}
