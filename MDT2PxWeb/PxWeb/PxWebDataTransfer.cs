using MDT2PxWeb.Bean;
using MDT2PxWeb.Connector;
using System.Collections.Generic;
using System.Data.Common;

namespace MDT2PxWeb.PxWeb
{
    class PxWebDataTransfer
    {
        public static int Transfer(Config c, List<Query> queries = null)
        {
            DBConnection con1 = null;
            DBConnection con2 = null;
            try
            {
                con1 = new DBConnection(c.mdtDb);
                con2 = (queries == null) ? new DBConnection(c.pxwebDb) : null;

                // to init dimensions if needed
                c.GetGoals();

                string selectColumns = c.mdtDb.dataTable.code + "";
                foreach (Dimension d in c.dimensions)
                {
                    selectColumns += "," + d.name;
                }
                selectColumns += "," + c.obsValue;

                DbDataReader reader = con1.ExecuteReader("SELECT " + selectColumns + " FROM " + c.mdtDb.dataTable.name + " WHERE " + c.obsValue + " IS NOT NULL");

                if (queries == null)
                {
                    con2.ExecuteNonQuery("TRUNCATE TABLE " + c.pxwebDb.dataTable.name);
                }
                else
                {
                    queries.Add(new Query("TRUNCATE TABLE " + c.pxwebDb.dataTable.name));
                }

                int counter = 0;
                while (reader.Read())
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    string columns = c.pxwebDb.dataTable.code;
                    string values = "@" + c.pxwebDb.dataTable.code;
                    if (c.pxwebDb.dataTable.integerCode)
                    {
                        parameters.Add("@" + c.pxwebDb.dataTable.code, reader.GetInt32(0));
                    }
                    else
                    {
                        parameters.Add("@" + c.pxwebDb.dataTable.code, reader.GetString(0).Trim());
                    }
                    int dIndex = 1;
                    foreach (Dimension d in c.dimensions)
                    {
                        columns += "," + d.name;
                        values += ",@" + d.name;
                        if (reader.IsDBNull(dIndex))
                        {
                            parameters.Add("@" + d.name, null);
                        }
                        else if (d.table.integerCode)
                        {
                            parameters.Add("@" + d.name, reader.GetInt32(dIndex));
                        }
                        else
                        {
                            parameters.Add("@" + d.name, reader.GetString(dIndex).Trim());
                        }
                        dIndex++;
                    }
                    columns += "," + c.obsValue;
                    values += ",@" + c.obsValue;
                    if (reader.IsDBNull(dIndex))
                    {
                        parameters.Add("@" + c.obsValue, null);
                    }
                    else
                    {
                        parameters.Add("@" + c.obsValue, reader.GetDouble(dIndex));
                    }

                    string sql = "INSERT INTO " + c.pxwebDb.dataTable.name + " (" + columns + ") VALUES (" + values + ")";
                    if (queries == null)
                    {
                        con2.ExecuteNonQuery(sql, parameters);
                    }
                    else
                    {
                        queries.Add(new Query(sql, parameters));
                    }
                    counter++;
                }
                reader.Close();
                return counter;
            }
            finally
            {
                if (con1 != null)
                {
                    con1.CloseConnection();
                }
                if (con2 != null)
                {
                    con2.CloseConnection();
                }
            }
        }
    }
}
