using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Properties
{
    public static string GOAL_ID = "GOAL_ID";

    public static string[] subindicatorKey = {
        "INDICATOR_CODE",
        "SUBINDICATOR_CODE"
    };

    public static List<string> dimensions = new List<string>();

    private static Dictionary<string, Dimension> dimensionsByKey = new Dictionary<string, Dimension>();

    public static string[] variables = {
        "OBS_VALUE",
        "UNIT_MULT",
        "UNIT_MEASURE",
        "OBS_STATUS",
        "TIME_DETAIL",
        "COMMENT_OBS",
        "BASE_PER",
        "SOURCE_DETAIL",
        "SOURCE_DETAIL_AR"
    };


    public static bool isTimePeriod(string dim)
    {
      return "TIME_PERIOD".Equals(dim);
    }



    public static List<Dimension> getDimensions(string dim)
    {
        InitDimensions();
        List<Dimension> result = new List<Dimension>();
        if (string.IsNullOrEmpty(dim))
        {
            for (int i = 0; i < dimensions.Count; i++)
            {
                result.Add(dimensionsByKey[dimensions[i]]);
            }
            return result;
        }

        char[] a = dim.ToCharArray();
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] == '1')
            {
                result.Add(dimensionsByKey[dimensions[i]]);
            }
        }
        return result;
    }

    public static Dictionary<string, object> variablesTypes = new Dictionary<string, object>{
        { "OBS_VALUE", SqlDbType.Float },
        { "UNIT_MULT", SqlDbType.NChar},
        { "UNIT_MEASURE", SqlDbType.NChar},
        { "OBS_STATUS", SqlDbType.NChar},
        { "TIME_DETAIL", SqlDbType.NChar},
        { "COMMENT_OBS", SqlDbType.NChar},
        { "BASE_PER", SqlDbType.NChar},
        { "SOURCE_DETAIL", SqlDbType.NChar},
        { "SOURCE_DETAIL_AR", SqlDbType.NChar}
    };

    public static void InitDimensions()
    {
        if (dimensions.Count > 0)
        {
            return;
        }
        QueryManager qm = new QueryManager();
        InitDimensions(qm);
        qm.closeConnection();
    }

    public static void InitDimensions(QueryManager qm)
    {
        if (dimensions.Count > 0)
        {
            return;
        }
        SqlDataReader reader = qm.executeReader("SELECT NAME, LABEL, CODE, TABLE_NAME, DESCRIPTION, IS_TIME, INT_CODE, DESCRIPTION_ENG FROM DIMENSION ORDER BY SEQUENCE");
        while (reader.Read())
        {
            Dimension d = new Dimension(
                reader.GetString(0).Trim(),
                reader.GetString(1).Trim(),
                reader.IsDBNull(2) ? null : reader.GetString(2).Trim(),
                reader.IsDBNull(3) ? null : reader.GetString(3).Trim(),
                reader.IsDBNull(4) ? null : reader.GetString(4).Trim(),
                reader.GetByte(5) == 1,
                reader.GetByte(6) == 1,
                false,
                reader.IsDBNull(7) ? null : reader.GetString(7).Trim()
                );
            dimensions.Add(d.getName());
            dimensionsByKey.Add(d.getName(), d);
        }
        reader.Close();
    }

}

