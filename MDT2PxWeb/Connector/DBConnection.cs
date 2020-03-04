using MDT2PxWeb.Bean;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Security;

namespace MDT2PxWeb.Connector
{
    public class DBConnection
    {
        public readonly string type;
        private readonly SqlConnection con = null;
        private readonly OracleConnection conOracle = null;
        private readonly OleDbConnection conOleDb = null;
        private readonly OdbcConnection conOdbc = null;

        public DBConnection(Database database)
        {
            string url = database.connectionString;
            string username = database.username;
            string password = database.password;
            this.type = database.type;

            SecureString secureStr = null;
            if (username != null && password != null)
            {
                secureStr = new SecureString();
                for (int i = 0; i < password.Length; i++)
                {
                    secureStr.AppendChar(password[i]);
                }
                secureStr.MakeReadOnly();
            }

            switch (this.type)
            {
                case "ORACLE":
                    OracleConnectionStringBuilder ocsb = new OracleConnectionStringBuilder
                    {
                        DataSource = url
                    };
                    this.conOracle = (secureStr == null) ? new OracleConnection(ocsb.ConnectionString) : new OracleConnection(ocsb.ConnectionString, new OracleCredential(username, secureStr));
                    this.conOracle.Open();
                    break;

                case "OLEDB":
                    this.conOleDb = new OleDbConnection(url);
                    break;

                case "ODBC":
                    this.conOdbc = new OdbcConnection(url);
                    break;

                default:
                    this.con = (secureStr == null) ? new SqlConnection(url) : new SqlConnection(url, new SqlCredential(username, secureStr));
                    this.con.Open();
                    break;
            }
        }

        public void CloseConnection()
        {
            switch (this.type)
            {
                case "ORACLE":
                    this.conOracle.Close();
                    break;

                case "OLEDB":
                    this.conOleDb.Close();
                    break;

                case "ODBC":
                    this.conOdbc.Close();
                    break;

                default:
                    this.con.Close();
                    break;
            }
        }

        public DbDataReader ExecuteReader(string query, Dictionary<string, object> parameters = null)
        {
            switch (this.type)
            {
                case "ORACLE":
                    OracleCommand cmdOracle = new OracleCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOracle
                    };
                    AddParameters(cmdOracle, parameters);
                    return cmdOracle.ExecuteReader();

                case "OLEDB":
                    OleDbCommand cmdOleDb = new OleDbCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOleDb
                    };
                    AddParameters(cmdOleDb, parameters);
                    return cmdOleDb.ExecuteReader();

                case "ODBC":
                    OdbcCommand cmdOdbc = new OdbcCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOdbc
                    };
                    AddParameters(cmdOdbc, parameters);
                    return cmdOdbc.ExecuteReader();

                default:
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = con
                    };
                    AddParameters(cmd, parameters);
                    return cmd.ExecuteReader();
            }
        }

        public DataSet ExecuteDataSet(string query, Dictionary<string, object> parameters = null)
        {
            DataSet ds = new DataSet();
            switch (this.type)
            {
                case "ORACLE":
                    OracleCommand cmdOracle = new OracleCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOracle
                    };
                    AddParameters(cmdOracle, parameters);
                    OracleDataAdapter oda = new OracleDataAdapter(cmdOracle);
                    oda.Fill(ds);
                    break;

                case "OLEDB":
                    OleDbCommand cmdOleDb = new OleDbCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOleDb
                    };
                    AddParameters(cmdOleDb, parameters);
                    OleDbDataAdapter oledbda = new OleDbDataAdapter(cmdOleDb);
                    oledbda.Fill(ds);
                    break;

                case "ODBC":
                    OdbcCommand cmdOdbc = new OdbcCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOdbc
                    };
                    AddParameters(cmdOdbc, parameters);
                    OdbcDataAdapter odbcda = new OdbcDataAdapter(cmdOdbc);
                    odbcda.Fill(ds);
                    break;

                default:
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = con
                    };
                    AddParameters(cmd, parameters);
                    SqlDataAdapter sqlda = new SqlDataAdapter(cmd);
                    sqlda.Fill(ds);
                    break;
            }
            return ds;
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            switch (this.type)
            {
                case "ORACLE":
                    OracleCommand cmdOracle = new OracleCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOracle
                    };
                    AddParameters(cmdOracle, parameters);
                    return cmdOracle.ExecuteNonQuery();

                case "OLEDB":
                    OleDbCommand cmdOleDb = new OleDbCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOleDb
                    };
                    AddParameters(cmdOleDb, parameters);
                    return cmdOleDb.ExecuteNonQuery();

                case "ODBC":
                    OdbcCommand cmdOdbc = new OdbcCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = conOdbc
                    };
                    AddParameters(cmdOdbc, parameters);
                    return cmdOdbc.ExecuteNonQuery();

                default:
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = con
                    };
                    AddParameters(cmd, parameters);
                    return cmd.ExecuteNonQuery();
            }
        }

        private void AddParameters(SqlCommand cmd, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    string key = pair.Key;
                    if (pair.Value == null)
                    {
                        cmd.Parameters.AddWithValue(key, SqlDbType.VarChar);
                        cmd.Parameters[key].Value = DBNull.Value;
                    }
                    else if (pair.Value.GetType() == typeof(string))
                    {
                        cmd.Parameters.AddWithValue(key, SqlDbType.VarChar);
                        cmd.Parameters[key].Value = pair.Value.ToString().Trim();
                    }
                    else if (pair.Value.GetType() == typeof(int))
                    {
                        cmd.Parameters.AddWithValue(key, SqlDbType.Int);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                    else if (pair.Value.GetType() == typeof(float))
                    {
                        cmd.Parameters.AddWithValue(key, SqlDbType.Float);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                    else if (pair.Value.GetType() == typeof(double))
                    {
                        cmd.Parameters.AddWithValue(key, SqlDbType.Float);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                }
            }
        }

        private void AddParameters(OracleCommand cmd, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    string key = pair.Key;
                    if (pair.Value == null)
                    {
                        cmd.Parameters.Add(key, OracleDbType.Varchar2);
                        cmd.Parameters[key].Value = DBNull.Value;
                    }
                    else if (pair.Value.GetType() == typeof(string))
                    {
                        cmd.Parameters.Add(key, OracleDbType.Varchar2);
                        cmd.Parameters[key].Value = pair.Value.ToString().Trim();
                    }
                    else if (pair.Value.GetType() == typeof(int))
                    {
                        cmd.Parameters.Add(key, OracleDbType.Int32);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                    else if (pair.Value.GetType() == typeof(float))
                    {
                        cmd.Parameters.Add(key, OracleDbType.Double);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                    else if (pair.Value.GetType() == typeof(double))
                    {
                        cmd.Parameters.Add(key, OracleDbType.Double);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                }
            }
        }

        private void AddParameters(OleDbCommand cmd, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    string key = pair.Key;
                    if (pair.Value == null)
                    {
                        cmd.Parameters.AddWithValue(key, OleDbType.VarChar);
                        cmd.Parameters[key].Value = DBNull.Value;
                    }
                    else if (pair.Value.GetType() == typeof(string))
                    {
                        cmd.Parameters.AddWithValue(key, OleDbType.VarChar);
                        cmd.Parameters[key].Value = pair.Value.ToString().Trim();
                    }
                    else if (pair.Value.GetType() == typeof(int))
                    {
                        cmd.Parameters.AddWithValue(key, OleDbType.Integer);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                    else if (pair.Value.GetType() == typeof(float))
                    {
                        cmd.Parameters.AddWithValue(key, OleDbType.Double);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                    else if (pair.Value.GetType() == typeof(double))
                    {
                        cmd.Parameters.AddWithValue(key, OleDbType.Double);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                }
            }
        }

        private void AddParameters(OdbcCommand cmd, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    string key = pair.Key;
                    if (pair.Value == null)
                    {
                        cmd.Parameters.AddWithValue(key, OdbcType.VarChar);
                        cmd.Parameters[key].Value = DBNull.Value;
                    }
                    else if (pair.Value.GetType() == typeof(string))
                    {
                        cmd.Parameters.AddWithValue(key, OdbcType.VarChar);
                        cmd.Parameters[key].Value = pair.Value.ToString().Trim();
                    }
                    else if (pair.Value.GetType() == typeof(int))
                    {
                        cmd.Parameters.AddWithValue(key, OdbcType.Int);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                    else if (pair.Value.GetType() == typeof(float))
                    {
                        cmd.Parameters.AddWithValue(key, OdbcType.Double);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                    else if (pair.Value.GetType() == typeof(double))
                    {
                        cmd.Parameters.AddWithValue(key, OdbcType.Double);
                        cmd.Parameters[key].Value = pair.Value;
                    }
                }
            }
        }

    }

}
