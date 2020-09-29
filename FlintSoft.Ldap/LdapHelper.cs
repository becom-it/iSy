using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlintSoft.Ldap
{
    public class LdapHelper
    {
        public static List<string> GetLdapAttributes<T>()
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.IsDefined(typeof(LdapUserAttribute), true))
                .Select(x => (x.GetCustomAttributes(typeof(LdapUserAttribute)).First() as LdapUserAttribute).AttributeName).ToList();

            return props;
        }

        public static List<T> ConvertLdapResult<T>(ILogger logger, ILdapSearchResults result, Func<string, LdapAttribute, object> retrieveValue = null)
        {
            var ret = new List<T>();

            while (result.HasMore())
            {
                if (result.Count > 0)
                {
                    var nextEntry = result.Next();
                    var x = ConvertLdapEntry<T>(logger, nextEntry, retrieveValue);
                    ret.Add(x);
                    //Console.WriteLine(nextEntry.getAttribute(DisplayNameAttribute)); 
                }
            }

            return ret;
        }

        public static T ConvertLdapEntry<T>(ILogger logger, LdapEntry entry, Func<string, LdapAttribute, object> retrieveValue = null)
        {
            var attributes = GetLdapAttributes<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.IsDefined(typeof(LdapUserAttribute), true)).ToList();

            var data = Activator.CreateInstance<T>();

            foreach (var a in attributes)
            {
                try
                {
                    var e = entry.GetAttribute(a);
                    if (e != null)
                    {
                        var prop = properties.Where(x => (x.GetCustomAttributes(typeof(LdapUserAttribute)).First() as LdapUserAttribute).AttributeName == a).First();
                        if (retrieveValue == null)
                        {
                            typeof(T).GetProperty(prop.Name).SetValue(data, e.StringValue);
                        }
                        else
                        {
                            var val = retrieveValue(prop.Name, e);
                            typeof(T).GetProperty(prop.Name).SetValue(data, val);
                        }

                    }
                }
                catch(KeyNotFoundException kex)
                {
                    logger.LogWarning($"Attribute {a} not found in directory entry! ({kex.Message})");
                    continue;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error converting and LdapEntry to a c# object: {ex.Message}");
                }
            }

            return data;
        }
    }
}
