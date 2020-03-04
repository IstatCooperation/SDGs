using MDT2PxWeb.Connector;
using System.Collections.Generic;
using System.Data.Common;

namespace MDT2PxWeb.Bean
{
    public class Query
    {
        private readonly string sql;
        private readonly Dictionary<string, object> parameters;

        public Query(string sql, Dictionary<string, object> parameters = null)
        {
            this.sql = sql;
            this.parameters = parameters;
        }

        public string ToSql(Database d)
        {
            string s = ResolveDbNatives(sql, d.type);
            if ("GO".Equals(s))
            {
                return s;
            }
            return ParseParameters(s) + ";";
        }

        public int ExecuteQuery(DBConnection con)
        {
            if ("GO".Equals(sql))
            {
                return 0;
            }
            return con.ExecuteNonQuery(ResolveDbNatives(sql, con.type), parameters);
        }

        public DbDataReader ExecuteReader(DBConnection con)
        {
            return con.ExecuteReader(ResolveDbNatives(sql, con.type), parameters);
        }

        private string ResolveDbNatives(string s, string dbType)
        {
            switch (dbType)
            {
                case "ORACLE":
                    s = s.Replace("#TIME#", "SYSDATE");
                    break;
                default:
                    s = s.Replace("#TIME#", "GETDATE()");
                    break;
            }
            return ParseParameters(s);
        }

        private string ParseParameters(string s)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    string key = pair.Key;
                    if (pair.Value == null)
                    {
                        s = s.Replace(pair.Key, "null");
                    }
                    else if (pair.Value.GetType() == typeof(string))
                    {
                        s = s.Replace(pair.Key, "'" + Escape(pair.Value.ToString()) + "'");
                    }
                    else
                    {
                        s = s.Replace(pair.Key, pair.Value.ToString());
                    }
                }
            }
            return s;
        }

        private static string Escape(string s)
        {
            return s.Replace("'", "''").Replace("’", "''");
        }

    }
}
