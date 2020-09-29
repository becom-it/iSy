using FlintSoft.Ldap.Models;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlintSoft.Ldap.Services
{
    public class LdapService : ILdapService
    {
        private static ILdapConnection _conn = null;
        private readonly ILogger<LdapService> _logger;
        private readonly LDAPConfiguration _config;

        public LdapService(ILogger<LdapService> logger, LDAPConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<List<T>> Search<T>(string path, string filter)
        {
            return await Task.Run(() =>
            {
                var ldapConn = getLdapConnection(_config);
                var search = ldapConn.Search(path, LdapConnection.ScopeSub, filter, LdapHelper.GetLdapAttributes<LdapUser>().ToArray(), false);

                return LdapHelper.ConvertLdapResult<T>(_logger, search, (propName, attr) =>
                {
                    if (propName == "Photo")
                    {
                        byte[] picData = (byte[])(Array)attr.ByteValue;
                        return Convert.ToBase64String(picData);
                    }
                    else
                    {
                        return attr.StringValue;
                    }
                });
            });
        }

        private static ILdapConnection getLdapConnection(LDAPConfiguration configuration)
        {
            LdapConnection ldapConn = _conn as LdapConnection;

            if (ldapConn == null)
            {
                // Creating an LdapConnection instance 
                ldapConn = new LdapConnection()
                {
                    SecureSocketLayer = true
                };
                ldapConn.Connect(configuration.Server, LdapConnection.DefaultSslPort);

                //Bind function with null user dn and password value will perform anonymous bind to LDAP server 
                ldapConn.Bind(configuration.UName, configuration.Password);
                _conn = ldapConn;
            }

            return ldapConn;
        }
    }

    public interface ILdapService
    {
        Task<List<T>> Search<T>(string path, string filter);
    }
}
