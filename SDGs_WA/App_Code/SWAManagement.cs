using SDGsWA.Bean;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class SWAManagement
{
    public SWAManagement()
    {

    }
 

    public SWAGlobalProperties getSWAGlobalProperties()
    {
        SWAGlobalProperties ret = new SWAGlobalProperties();
        QueryManager qm = new QueryManager();
        
        string selectCmd = "SELECT TOP (1) [id] ,[name] ,[label] ,[description],[description2],[url_logo],[url_website],[secondary_language],[secondary_language_code] FROM [swa_properties]";

        SqlDataReader reader = null;
        try
        {
            reader = qm.executeReader(selectCmd);

            while (reader.Read())
            {
                ret.name = reader["name"].ToString();
                ret.label= reader["label"].ToString();
                ret.description = reader["description"].ToString();
                ret.description2 = reader["description2"].ToString();
                ret.urlLogo = reader["url_logo"].ToString();
                ret.urlWebsite = reader["url_website"].ToString();
                ret.secondaryLanguage = reader["secondary_language"].ToString();
                ret.secondaryLanguageCode = reader["secondary_language_code"].ToString();

            }
        }
        finally
        {
            reader.Close();
        }

        qm.closeConnection();
        return ret;
    }

}