using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public class DataUploadManager
{

    public static DataUploadResult Upload(string excelPath, string fileName, string indCode, string subCode)
    {
        DataSet ds = ExcelReader.readExcelFile(excelPath, fileName);
        /* delete the uploaded file after reading it */
        File.Delete(excelPath);
        if (ds == null)
        {
            return new DataUploadResult(2, "The uploaded file is not an excel file!");
        }

        DataTable dt = ds.Tables[0];
        DataColumnCollection columns = dt.Columns;

        /* check if the file as the primary keys properties */
        if (!ExcelReader.checkExcel(columns))
        {
            return new DataUploadResult(3, "The excel file does not respect the schema!");
        }

        int insertedData = 0;
        int updatedData = 0;
        int wrongData = 0;
        Dictionary<string, int> goals = new Dictionary<string, int>();
        QueryManager qm = new QueryManager();

        SqlDataReader reader = qm.executeReader(
            "SELECT S.Dimensions, S.UNIT_MEASURE, S.UNIT_MULT, S.OBS_STATUS" +
            " FROM Subindicator S" +
            " WHERE S.Subindicator_Code IN(@ID)", new Dictionary<string, object>
            {
                { "@ID", subCode },
            });
        reader.Read();
        string UNIT_MEASURE = Convert.ToString(reader["UNIT_MEASURE"]);
        string UNIT_MULT = Convert.ToString(reader["UNIT_MULT"]);
        string OBS_STATUS = Convert.ToString(reader["OBS_STATUS"]);
        reader.Close();

        DataTable dtCloned = dt.Clone();
        dtCloned.Columns["UNIT_MEASURE"].DataType = typeof(string);
        dtCloned.Columns["UNIT_MULT"].DataType = typeof(string);
        dtCloned.Columns["OBS_STATUS"].DataType = typeof(string);
        foreach (DataRow row in dt.Rows)
        {
            dtCloned.ImportRow(row);
        }

        for (int rowNum = 0; rowNum < dtCloned.Rows.Count; rowNum++)
        {
            DataRow row = dtCloned.Rows[rowNum];

            if (!row[Properties.subindicatorKey[0]].ToString().Trim().Equals(indCode.Trim()) ||
                !row[Properties.subindicatorKey[1]].ToString().Trim().Equals(subCode.Trim()))
            {
                continue;
            }

            if (ExcelReader.checkRow(row))
            {
                row["UNIT_MEASURE"] = UNIT_MEASURE;
                row["UNIT_MULT"] = UNIT_MULT;
                row["OBS_STATUS"] = OBS_STATUS;
                string select = qm.getSelectSql(row, columns);
                if (qm.executeScalar(select) == 0)
                {
                    string insert = qm.getInsertSql(row, columns);
                    qm.executeNonQuery(insert);
                    insertedData++;
                }
                else
                {
                    string update = qm.getUpdateSql(row, columns);
                    qm.executeNonQuery(update);
                    updatedData++;
                }
            }
            else
            {
                wrongData++;
            }

        }

        if (insertedData > 0 || updatedData > 0)
        {
            Dictionary<string, object> actionParam = new Dictionary<string, object>
            {
                { "@IND_CODE", indCode },
                { "@SUB_CODE", subCode },
            };

            qm.executeNonQuery(
                "UPDATE Subindicator" +
                " SET Is_Uploaded = 1" +
                " WHERE" +
                " Indicator_Code = @IND_CODE" +
                " AND" +
                " Subindicator_Code = @SUB_CODE",
                actionParam);
        }

        qm.closeConnection();

        return new DataUploadResult(insertedData, updatedData, wrongData);
    }

}
