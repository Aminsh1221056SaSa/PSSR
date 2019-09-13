using PSSR.UserSecurity.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PSSR.Security.Helpers
{
    public static class HelperExtension
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }

        public static object GetPropValue<T>(string propName)
        {
            Type type = typeof(T); // MyClass is static class with static properties
            object val = null;
            foreach (var p in type.GetProperties(BindingFlags.Public |
                                                          BindingFlags.Static))
            {
                if(string.Equals(p.Name,propName,StringComparison.InvariantCultureIgnoreCase))
                {
                    val = p.GetValue(null, null); // static classes cannot be instanced, so use null...
                }
            }
            return val;
        }

        public static Person FromDataReaderPerson(this IDataReader r)
        {
            return new Person
            {
                Id = r["Id"] is DBNull ? 0 : Convert.ToInt32(r["Id"]),
                Name =( r["FirstName"] is DBNull ? "" : r["FirstName"].ToString() )+( r["LastName"] is DBNull ? "" : r["LastName"].ToString()),
               
                NationalId = r["NationalId"] is DBNull ? "نامشخص" : r["NationalId"].ToString()
            };
        }
    }
}
