using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;

public class ExcelReader
{

    public static DataSet readExcelFile(string excelPath, string fileName)
    {
        DataSet ds = new DataSet();
        string conString = string.Empty;
        string sheet1 = string.Empty;
        string extension = Path.GetExtension(fileName);
        switch (extension)
        {
            case ".xls":
                conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                break;
            case ".xlsx":
                conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                break;
        }

        conString = string.Format(conString, excelPath);
        using (OleDbConnection excel_con = new OleDbConnection(conString))
        {
            try
            {
                excel_con.Open();
                if (excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows.Count > 1)
                {
                    for (int i = 0; i < excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows.Count; i++)
                    {
                        if (excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[i]["TABLE_NAME"].ToString().ToUpper().StartsWith("SUBIND"))
                        {
                            sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[i]["TABLE_NAME"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                }
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet1 + "]", excel_con);
                OleDbDataAdapter oleda = new OleDbDataAdapter
                {
                    SelectCommand = cmd
                };
                oleda.Fill(ds, "matrix");
                excel_con.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception caught. " + e.Message);
                return null;
            }
        }
        return ds;
    }

    public static bool checkExcel(DataColumnCollection columns)
    {
        for (int colNum = 0; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            if (!columns.Contains(Properties.subindicatorKey[colNum]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool checkRow(DataRow row)
    {
        Properties.InitDimensions();
        for (int colNum = 0; colNum < Properties.subindicatorKey.Length; colNum++)
        {
            object value = row[Properties.subindicatorKey[colNum]];
            if (value == null || value is System.DBNull)
            {
                return false;
            }
        }
        for (int colNum = 0; colNum < Properties.dimensions.Count; colNum++)
        {
            if (row.Table.Columns.Contains(Properties.dimensions[colNum]))
            {
                object value = row[Properties.dimensions[colNum]];
                if (value != null && !(value is System.DBNull))
                    return true;
            }
        }
        return false;
    }

    public static string getGoalId(DataRow row)
    {
        return "" + row[Properties.GOAL_ID];
    }

    public static String getValue(DataRow row, DataColumnCollection columns, string columnName)
    {
        if (!columns.Contains(columnName))
        {
            return "null";
        }
        object value = row[columnName];
        if (value == null || value is System.DBNull)
        {
            return "null";
        }
        if (Properties.isTimePeriod(columnName))
        {
            return "" + Convert.ToInt32(Convert.ToDouble(value));
        }
        if (Properties.variablesTypes.ContainsKey(columnName) && SqlDbType.Float.Equals(Properties.variablesTypes[columnName]))
        {
            return ("" + value).Replace(",", ".");
        }
        if (value is System.String)
        {
            return "'" + ((string)value).Trim() + "'";
        }

        return "" + value;
    }

}

