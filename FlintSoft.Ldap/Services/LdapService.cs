using FlintSoft.Ldap.Models;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<T>> Search<T>(string filter, IEnumerable<int> employeeIds = null, string path = "")
        {
            return await Task.Run(() =>
            {
                var ldapConn = getLdapConnection(_config);
                var searchPath = string.Empty;
                if(string.IsNullOrEmpty(path)) {
                    searchPath = _config.Path;
                } else
                {
                    searchPath = path;
                }
                var search = ldapConn.Search(searchPath, LdapConnection.ScopeSub, filter, LdapHelper.GetLdapAttributes<T>().ToArray(), false);

                return LdapHelper.ConvertLdapResult<T>(_logger, search, employeeIds, (propName, attr) =>
                {
                    if (propName == "Photo")
                    {
                        byte[] picData = (byte[])(Array)attr.ByteValue;
                        return Convert.ToBase64String(picData);
                    }
                    else
                    {
                        if (attr.StringValueArray.Length > 1)
                        {
                            return attr.StringValueArray.ToList();
                        }
                        else
                        {
                            return attr.StringValue;
                        }
                    }
                });
            });
        }

        public async Task<List<T>> CustomSearch<T>(ILdapConnection conn, string filter, IEnumerable<int> employeeIds = null, string path = "")
        {
            return await Task.Run(() =>
            {
                var searchPath = string.Empty;
                if (string.IsNullOrEmpty(path))
                {
                    searchPath = _config.Path;
                }
                else
                {
                    searchPath = path;
                }

                var attrs = LdapHelper.GetLdapAttributes<T>().ToArray();
                var search = conn.Search(searchPath, LdapConnection.ScopeSub, filter, attrs, false);

                return LdapHelper.ConvertLdapResult<T>(_logger, search, employeeIds, (propName, attr) =>
                {
                    if (propName == "Photo")
                    {
                        byte[] picData = (byte[])(Array)attr.ByteValue;
                        return Convert.ToBase64String(picData);
                    }
                    else
                    {
                        if (attr.StringValueArray.Length > 1)
                        {
                            return attr.StringValueArray.ToList();
                        }
                        else
                        {
                            return attr.StringValue;
                        }
                    }
                });
            });
        }

        private static ILdapConnection getLdapConnection(LDAPConfiguration configuration)
        {
            if (!(_conn is LdapConnection ldapConn))
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

        public ILdapConnection GetCustomLdapConnection(string uname, string password)
        {
            var ldapConn = new LdapConnection() { SecureSocketLayer = true };
            ldapConn.Connect(_config.Server, LdapConnection.DefaultSslPort);
            ldapConn.Bind($"{uname}@ad.becom.at", password);
            return ldapConn;
        }
    }

    public interface ILdapService
    {
        Task<List<T>> Search<T>(string filter, IEnumerable<int> employeeIds = null, string path = "");
        ILdapConnection GetCustomLdapConnection(string uname, string password);
        Task<List<T>> CustomSearch<T>(ILdapConnection conn, string filter, IEnumerable<int> employeeIds = null, string path = "");
    }
}
