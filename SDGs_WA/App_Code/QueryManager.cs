using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class QueryManager
{

    private readonly string insertValues;
    private readonly SqlConnection con;

    public QueryManager()
    {
        string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        this.con = new SqlConnection(constr);
        this.con.Open();
        Properties.InitDimensions(this);
        this.insertValues = getInsertValues();
    }

    public void closeConnection()
    {
        this.con.Close();
    }

    public SqlTransaction getTransaction()
    {
        return this.con.BeginTransaction();
    }

    public String getInsertSql(DataRow row, DataColumnCollection columns)
    {
        string insert = "INSERT INTO Subindicator_Data (" + insertValues + ") VALUES (";
        insert += ExcelReader.getValue(row, columns, Properties.subindicatorKey[0]);
        for (int colNum = 1; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            insert += "," + ExcelReader.getValue(row, columns, Properties.subindicatorKey[colNum]);
        }
        for (int colNum = 0; colNum < Properties.dimensions.Count; colNum++)
        {
            insert += "," + ExcelReader.getValue(row, columns, Properties.dimensions[colNum]);
        }
        for (int colNum = 0; colNum < Properties.variables.Length; colNum++)
        {
            insert += "," + ExcelReader.getValue(row, columns, Properties.variables[colNum]);
        }
        insert += ")";
        return insert;
    }

    public String getUpdateSql(DataRow row, DataColumnCollection columns)
    {
        string update = "UPDATE Subindicator_Data SET ";
        update += Properties.variables[0] + " = " + ExcelReader.getValue(row, columns, Properties.variables[0]);
        for (int colNum = 1; colNum < Properties.variables.Length; colNum++)
        {
            update += "," + Properties.variables[colNum] + " = " + ExcelReader.getValue(row, columns, Properties.variables[colNum]);
        }
        update += " WHERE ";
        string value = ExcelReader.getValue(row, columns, Properties.subindicatorKey[0]);
        update += Properties.subindicatorKey[0] + (value.Equals("null") ? " is null" : " = " + value);
        for (int colNum = 1; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            value = ExcelReader.getValue(row, columns, Properties.subindicatorKey[colNum]);
            update += " AND " + Properties.subindicatorKey[colNum] + (value.Equals("null") ? " is null" : " = " + value);
        }
        for (int colNum = 0; colNum < Properties.dimensions.Count; colNum++)
        {
            value = ExcelReader.getValue(row, columns, Properties.dimensions[colNum]);
            update += " AND " + Properties.dimensions[colNum] + (value.Equals("null") ? " is null" : " = " + value);
        }
        return update;
    }

    public void executeUpdateSql(string[] subindicatorKeys, string[] dimValues, string[] values, List<Dimension> dims)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        int paramCount = 0;
        Dictionary<string, string> variablesNames = new Dictionary<string, string>();
        string update = "UPDATE Subindicator_Data SET ";
        string value = (0 < values.Length && !String.IsNullOrEmpty(values[0])) ? values[0] : "null";
        update += Properties.variables[0] + (value.Equals("null") ? " = null" : " = @ID" + paramCount);
        if (!value.Equals("null"))
        {
            int index = paramCount++;
            parameters.Add("@ID" + index, value);
            variablesNames.Add("@ID" + index, Properties.variables[0]);
        }
        for (int colNum = 1; colNum < Properties.variables.Length; colNum++)
        {
            value = (colNum < values.Length && !String.IsNullOrEmpty(values[colNum])) ? values[colNum] : "null";
            update += "," + Properties.variables[colNum] + (value.Equals("null") ? " = null" : " = @ID" + paramCount);
            if (!value.Equals("null"))
            {
                int index = paramCount++;
                parameters.Add("@ID" + index, value);
                variablesNames.Add("@ID" + index, Properties.variables[0]);
            }
        }
        update += " WHERE ";
        value = subindicatorKeys[0];
        update += Properties.subindicatorKey[0] + (value.Equals("null") ? " is null" : " = @ID" + paramCount);
        if (!String.IsNullOrEmpty(value))
            parameters.Add("@ID" + paramCount++, value);
        for (int colNum = 1; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            value = subindicatorKeys[colNum];
            update += " AND " + Properties.subindicatorKey[colNum] + (value.Equals("null") ? " is null" : " = @ID" + paramCount);
            if (!String.IsNullOrEmpty(value))
                parameters.Add("@ID" + paramCount++, value);
        }
        for (int colNum = 0; colNum < dims.Count; colNum++)
        {
            value = (colNum < dimValues.Length && !String.IsNullOrEmpty(dimValues[colNum])) ? dimValues[colNum] : null;
            update += " AND " + dims[colNum].getName() + (String.IsNullOrEmpty(value) ? " is null" : " = @ID" + paramCount);
            if (!String.IsNullOrEmpty(value))
                parameters.Add("@ID" + paramCount++, value);
        }
        executeNonQuery(update, parameters, variablesNames, Properties.variablesTypes);
    }

    public void executeInsertSql(string[] subindicatorKeys, string[] dimValues, string[] values, List<Dimension> dims)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        int paramCount = 0;
        Dictionary<string, string> variablesNames = new Dictionary<string, string>();
        string update = "INSERT INTO Subindicator_Data (";
        update += Properties.subindicatorKey[0];
        for (int colNum = 1; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            update += "," + Properties.subindicatorKey[colNum];
        }
        for (int colNum = 0; colNum < dims.Count; colNum++)
        {
            update += "," + dims[colNum].getName();
        }
        for (int colNum = 0; colNum < Properties.variables.Length; colNum++)
        {
            update += "," + Properties.variables[colNum];
        }
        update += ") VALUES (";
        string value = subindicatorKeys[0];
        update += value.Equals("null") ? "null" : "@ID" + paramCount;
        if (update.EndsWith("@ID" + paramCount)) parameters.Add("@ID" + paramCount++, value);
        for (int colNum = 1; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            value = subindicatorKeys[colNum];
            update += ", " + (value.Equals("null") ? "null" : "@ID" + paramCount);
            if (update.EndsWith("@ID" + paramCount)) parameters.Add("@ID" + paramCount++, value);
        }
        for (int colNum = 0; colNum < dims.Count; colNum++)
        {
            value = (colNum < dimValues.Length && !String.IsNullOrEmpty(dimValues[colNum])) ? dimValues[colNum] : null;
            update += "," + (String.IsNullOrEmpty(value) ? "null" : "@ID" + paramCount);
            if (update.EndsWith("@ID" + paramCount)) parameters.Add("@ID" + paramCount++, value);
        }
        for (int colNum = 0; colNum < Properties.variables.Length; colNum++)
        {
            value = (colNum < values.Length && !String.IsNullOrEmpty(values[colNum])) ? values[colNum] : null;
            update += "," + (String.IsNullOrEmpty(value) ? "null" : "@ID" + paramCount);
            if (update.EndsWith("@ID" + paramCount))
            {
                int index = paramCount++;
                parameters.Add("@ID" + index, value);
                variablesNames.Add("@ID" + index, Properties.variables[colNum]);
            }
        }
        update += ")";

        executeNonQuery(update, parameters, variablesNames, Properties.variablesTypes);
    }

    public int executeSelectSql(string[] subindicatorKeys, string[] dimValues, string[] values, List<Dimension> dims)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        int paramCount = 0;

        string update = "SELECT count(*) FROM Subindicator_Data WHERE ";
        string value = subindicatorKeys[0];
        update += Properties.subindicatorKey[0] + (value.Equals("null") ? " is null" : " = @ID" + paramCount);
        if (update.EndsWith("@ID" + paramCount)) parameters.Add("@ID" + paramCount++, value);
        for (int colNum = 1; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            value = subindicatorKeys[colNum];
            update += " AND " + Properties.subindicatorKey[colNum] + (value.Equals("null") ? " is null" : " = @ID" + paramCount);
            if (update.EndsWith("@ID" + paramCount)) parameters.Add("@ID" + paramCount++, value);
        }
        for (int colNum = 0; colNum < dims.Count; colNum++)
        {
            value = (colNum < dimValues.Length || String.IsNullOrEmpty(dimValues[colNum])) ? dimValues[colNum] : null;
            update += " AND " + dims[colNum].getName() + (String.IsNullOrEmpty(value) ? " is null" : " = @ID" + paramCount);
            if (update.EndsWith("@ID" + paramCount)) parameters.Add("@ID" + paramCount++, value);
        }

        return executeScalar(update, parameters);
    }

    public String getSelectSql(DataRow row, DataColumnCollection columns)
    {
        string select = "SELECT count(*) FROM Subindicator_Data WHERE ";
        string value = ExcelReader.getValue(row, columns, Properties.subindicatorKey[0]);
        select += Properties.subindicatorKey[0] + (value.Equals("null") ? " is null" : " = " + value);
        for (int colNum = 1; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            value = ExcelReader.getValue(row, columns, Properties.subindicatorKey[colNum]);
            select += " AND " + Properties.subindicatorKey[colNum] + (value.Equals("null") ? " is null" : " = " + value);
        }
        for (int colNum = 0; colNum < Properties.dimensions.Count; colNum++)
        {
            value = ExcelReader.getValue(row, columns, Properties.dimensions[colNum]);
            select += " AND " + Properties.dimensions[colNum] + (value.Equals("null") ? " is null" : " = " + value);
        }
        return select;
    }

    public void executeNonQueryTrans(string query, SqlTransaction trans)
    {
        SqlCommand cmd = new SqlCommand
        {
            CommandType = CommandType.Text,
            CommandText = query,
            Connection = con,
            Transaction = trans
        };
        cmd.ExecuteNonQuery();
    }

    public void executeNonQuery(string query, Dictionary<string, object> parameters = null)
    {
        SqlCommand cmd = new SqlCommand
        {
            CommandType = CommandType.Text,
            CommandText = query,
            Connection = con
        };
        addParameters(cmd, parameters);
        cmd.ExecuteNonQuery();
    }

    public void executeNonQuery(string query, Dictionary<string, object> parameters, Dictionary<string, string> variableNames, Dictionary<string, object> varTypes)
    {
        SqlCommand cmd = new SqlCommand
        {
            CommandType = CommandType.Text,
            CommandText = query,
            Connection = con
        };
        addParameters(cmd, parameters, variableNames, varTypes);
        cmd.ExecuteNonQuery();
    }

    public SqlDataReader executeReader(string query, Dictionary<string, object> parameters = null)
    {
        SqlCommand cmd = new SqlCommand
        {
            CommandType = CommandType.Text,
            CommandText = query,
            Connection = con
        };
        addParameters(cmd, parameters);
        return cmd.ExecuteReader();
    }

    public DataSet executeReaderDataSet(string query, Dictionary<string, object> parameters = null)
    {
        SqlCommand cmd = new SqlCommand
        {
            CommandType = CommandType.Text,
            CommandText = query,
            Connection = con
        };
        addParameters(cmd, parameters);
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        return ds;
    }

    public int executeScalar(string query, Dictionary<string, object> parameters = null)
    {
        SqlCommand cmd = new SqlCommand
        {
            CommandType = CommandType.Text,
            CommandText = query,
            Connection = con
        };
        addParameters(cmd, parameters);
        return Int32.Parse(cmd.ExecuteScalar().ToString());
    }

    private String getInsertValues()
    {
        string insertValues = Properties.subindicatorKey[0];
        for (int colNum = 1; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            insertValues += "," + Properties.subindicatorKey[colNum];
        }
        for (int colNum = 0; colNum < Properties.dimensions.Count; colNum++)
        {
            insertValues += "," + Properties.dimensions[colNum];
        }
        for (int colNum = 0; colNum < Properties.variables.Length; colNum++)
        {
            insertValues += "," + Properties.variables[colNum];
        }
        return insertValues;
    }

    private void addParameters(SqlCommand cmd, Dictionary<string, object> parameters)
    {
        if (parameters != null)
        {
            foreach (KeyValuePair<string, object> pair in parameters)
            {
                string key = pair.Key;
                if (pair.Value.GetType() == typeof(string))
                {
                    cmd.Parameters.Add(key, SqlDbType.VarChar);
                    cmd.Parameters[key].Value = pair.Value.ToString().Trim();
                }
                else if (pair.Value.GetType() == typeof(int))
                {
                    cmd.Parameters.Add(key, SqlDbType.Int);
                    cmd.Parameters[key].Value = pair.Value;
                }
                else if (pair.Value.GetType() == typeof(float))
                {
                    cmd.Parameters.Add(key, SqlDbType.Float);
                    cmd.Parameters[key].Value = pair.Value;
                }
            }
        }
    }

    private void addParameters(SqlCommand cmd, Dictionary<string, object> parameters, Dictionary<string, string> variableNames, Dictionary<string, object> parametersTypes)
    {
        if (parameters != null)
        {
            foreach (KeyValuePair<string, object> pair in parameters)
            {
                string key = pair.Key;
                object type;
                string varName;
                if (variableNames.TryGetValue(key, out varName) && parametersTypes.TryGetValue(varName, out type)
                    && type.Equals(SqlDbType.Float))
                {
                    cmd.Parameters.AddWithValue(key, type);
                    cmd.Parameters[key].Value = pair.Value.ToString().Trim().Replace(',', '.');
                }
                else
                {
                    cmd.Parameters.Add(key, SqlDbType.VarChar);
                    cmd.Parameters[key].Value = pair.Value.ToString().Trim();
                }
            }
        }
    }

    public DataTable GetData(string query)
    {
        string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }

}

