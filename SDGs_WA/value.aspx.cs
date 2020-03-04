using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }

        string subId = Request.QueryString["subId"];
        string targId = Request.QueryString["targId"];
        if (subId == null || targId == null)
        {
            Response.Redirect("index.aspx");
            return;
        }

        QueryManager qm = new QueryManager();
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@ID", subId },
                { "@TARGID", targId }
            };
            SqlDataReader reader = qm.executeReader("SELECT I.Indicator_Code, " +
                "I.Indicator_descEn, I.Indicator_descAr, T.Target_ID, T.Target_DescEn, " +
                "T.Target_DescAr, G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr, " +
                "S.Subindicator_Code, S.Subindicator_DescAr, S.Subindicator_DescEn, S.Subindicator_Code, IC.Indicator_NL " +
                "FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                "INNER JOIN Subindicator S ON S.Indicator_Code = I.Indicator_Code " +
                "WHERE S.Subindicator_Code = @ID AND T.Target_ID = @TARGID", parameters);
            title.Text = "";
            subTitle.Text = "";
            try
            {
                if (reader.Read())
                {
                    title.Text = Convert.ToString(reader["Target_ID"]).Trim() + " - " + Convert.ToString(reader["Target_DescEn"]).Trim();
                    indCode.Text = Convert.ToString(reader["Indicator_Code"]).Trim();
                    indicatorNL.Text = Convert.ToString(reader["Indicator_NL"]).Trim();
                    subTitle.Text = Convert.ToString(reader["Indicator_descEn"]).Trim();
                    subindicatorCode.Text = Convert.ToString(reader["Subindicator_Code"]).Trim();
                    indId.Text = Convert.ToString(reader["Indicator_Code"]).Trim();
                    goalId.Text = Convert.ToString(reader["Goal_ID"]).Trim();
                    targIdBack.Text = Convert.ToString(reader["Target_Id"]).Trim();
                }

            }
            finally
            {
                reader.Close();
            }

            refreshValues(qm, 0);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
        }
        finally
        {
            qm.closeConnection();
        }


        //check User access Goal
        UsersManager usersManager = new UsersManager();
        if (!usersManager.checkAccessGoal(HttpContext.Current.User, goalId.Text))
        {
            Response.Redirect("index.aspx");
            return;
        }

    }

    private void refreshValues()
    {
        QueryManager qm = new QueryManager();
        try
        {
            refreshValues(qm, 0);
        }
        finally
        {
            qm.closeConnection();
        }
    }

    private void refreshValues(QueryManager qm, int dbCheck)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@ID", subindicatorCode.Text }
            };

        SqlDataReader reader = qm.executeReader(
            "SELECT S.Dimensions" +
            " FROM Subindicator S" +
            " WHERE S.Subindicator_Code IN(@ID)", parameters);
        reader.Read();
        List<Dimension> dims = Properties.getDimensions(Convert.ToString(reader["Dimensions"]));
        reader.Close();

        try
        {
            DataTable table = new DataTable();
            table.Columns.Add("Key", typeof(string));
            table.Columns.Add("OBS_VALUE", typeof(string));
            table.Columns.Add("TIME_DETAIL", typeof(string));
            table.Columns.Add("COMMENT_OBS", typeof(string));
            table.Columns.Add("BASE_PER", typeof(string));
            table.Columns.Add("SOURCE_DETAIL", typeof(string));
            table.Columns.Add("SOURCE_DETAIL_AR", typeof(string));
            table.Columns.Add("HiddenKey", typeof(string));

            string query = "SELECT SD.Subindicator_Code";
            foreach (Dimension d in dims)
            {
                query += ", SD. " + d.getName();
                if (d.getTable() != null)
                {
                    query += ", " + d.getTable() + "." + d.getDesc() + " " + d.getName() + "_DESC";
                }
                else
                {
                    query += ", null " + d.getName() + "_DESC";
                }
            }
            for (int i = 0; i < Properties.variables.Length; i++)
            {
                query += ", SD." + Properties.variables[i] + " " + Properties.variables[i];
            }
            query += " FROM SUBINDICATOR_DATA SD";
            foreach (Dimension d in dims)
            {
                if (d.getTable() != null)
                {
                    query += " JOIN " + d.getTable() + " ON SD." + d.getName() + " = " + d.getTable() + "." + d.getKey();
                }
            }
            query += " WHERE SD.Subindicator_Code = @ID";

            reader = qm.executeReader(query, parameters);
            string header = createTable(reader, table, dims);
            if (header == null)
            {
                reader.Close();

                query = "SELECT S.Subindicator_Code";
                foreach (Dimension d in dims)
                {
                    if (d.getTable() != null)
                    {
                        query += "," + d.getKey() + " " + d.getName();
                        query += "," + d.getDesc() + " " + d.getName() + "_DESC";
                    }
                    else
                    {
                        query += ", null " + d.getName();
                        query += ", null " + d.getName() + "_DESC";
                    }
                }
                for (int i = 0; i < Properties.variables.Length; i++)
                {
                    query += ", null " + Properties.variables[i];
                }
                query += " FROM Subindicator S";
                foreach (Dimension d in dims)
                {
                    if (d.getTable() != null)
                    {
                        query += "," + d.getTable();
                    }
                }
                query += " WHERE S.Subindicator_Code IN(@ID)";

                reader = qm.executeReader(query, parameters);
                header = createTable(reader, table, dims);
            }

            ValueDetail.DataSource = table;
            ValueDetail.DataBind();

            headerKey.Text = header;
        }
        finally
        {
            reader.Close();
        }

        switch (dbCheck)
        {
            case 0:
                saveMessage.Text = "";
                break;
            case 1:
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
                break;
            case 2:
                saveMessage.Text = "<div class='db-alert db-error hideMe'>Warning! Data not updated.</div>";
                break;
        }
    }

    protected void Save(object sender, EventArgs e)
    {
        QueryManager qm = new QueryManager();

        try
        {
            foreach (RepeaterItem item in ValueDetail.Items)
            {
                if (item.ItemType == ListItemType.Item
                    || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox obsValue = (TextBox)item.FindControl("OBS_VALUE");
                    TextBox timeDetail = (TextBox)item.FindControl("TIME_DETAIL");
                    TextBox basePer = (TextBox)item.FindControl("BASE_PER");
                    TextBox commmentObs = (TextBox)item.FindControl("COMMENT_OBS");
                    TextBox sourceDetail = (TextBox)item.FindControl("SOURCE_DETAIL");
                    TextBox sourceDetailAr = (TextBox)item.FindControl("SOURCE_DETAIL_AR");
                    HiddenField hKey = (HiddenField)item.FindControl("HKey");

                    string[] dimensions = hKey.Value.Split('#');
                    for (int i = 0; i < dimensions.Length; i++)
                    {
                        if (String.IsNullOrEmpty(dimensions[i]))
                        {
                            dimensions[i] = null;
                        }
                    }

                    SqlDataReader reader = qm.executeReader(
                        "SELECT S.Dimensions, S.UNIT_MEASURE, S.UNIT_MULT, S.OBS_STATUS" +
                        " FROM Subindicator S" +
                        " WHERE S.Subindicator_Code IN(@ID)", new Dictionary<string, object>
                        {
                            { "@ID", subindicatorCode.Text },
                        });
                    reader.Read();
                    List<Dimension> dims = Properties.getDimensions(Convert.ToString(reader["Dimensions"]));
                    string UNIT_MEASURE = Convert.ToString(reader["UNIT_MEASURE"]);
                    string UNIT_MULT = Convert.ToString(reader["UNIT_MULT"]);
                    string OBS_STATUS = Convert.ToString(reader["OBS_STATUS"]);
                    reader.Close();

                    string[] key = new string[] { indCode.Text, subindicatorCode.Text };
                    string[] values = new string[] {
                        obsValue.Text, UNIT_MULT, UNIT_MEASURE, OBS_STATUS, timeDetail.Text,
                        commmentObs.Text, basePer.Text, sourceDetail.Text, sourceDetailAr.Text
                    };

                    int count = qm.executeSelectSql(key, dimensions, values, dims);
                    if (count == 0)
                    {
                        qm.executeInsertSql(key, dimensions, values, dims);
                    }
                    else
                    {
                        qm.executeUpdateSql(key, dimensions, values, dims);
                    }
                }
            }

            refreshValues(qm, 1);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Update exc: " + ex.Message);
            refreshValues(qm, 2);
        }
        finally
        {
            qm.closeConnection();
        }
    }

    private string createTable(SqlDataReader reader, DataTable table, List<Dimension> dims)
    {
        string header = null;
        while (reader.Read())
        {
            string key = "";
            string hKey = "";
            header = "";
            foreach (Dimension d in dims)
            {
                key += "<td>";
                key += Convert.ToString(reader[d.getName()]).Trim();
                key += "</td>";
                key += "<td>";
                key += Convert.ToString(reader[d.getName() + "_DESC"]).Trim();
                key += "</td>";
                hKey += Convert.ToString(reader[d.getName()]).Trim() + "#";
                //hKey += Convert.ToString(reader[d.getName() + "_DESC"]).Trim() + "#";
                header += "<th>";
                header += d.getName();
                header += "</th>";
                header += "<th>";
                header += d.getName() + " DESC";
                header += "</th>";
            }
            string OBS_VALUE = Convert.ToString(reader["OBS_VALUE"]).Trim();
            string TIME_DETAIL = Convert.ToString(reader["TIME_DETAIL"]).Trim();
            string COMMENT_OBS = Convert.ToString(reader["COMMENT_OBS"]).Trim();
            string BASE_PER = Convert.ToString(reader["BASE_PER"]).Trim();
            string SOURCE_DETAIL = Convert.ToString(reader["SOURCE_DETAIL"]).Trim();
            string SOURCE_DETAIL_AR = Convert.ToString(reader["SOURCE_DETAIL_AR"]).Trim();

            table.Rows.Add(key, OBS_VALUE, TIME_DETAIL, COMMENT_OBS, BASE_PER, SOURCE_DETAIL, SOURCE_DETAIL_AR, hKey);
        }
        return header;
    }

}

