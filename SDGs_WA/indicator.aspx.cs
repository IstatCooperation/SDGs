using ClosedXML.Excel;
using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    protected string indId;
    protected string targId;
    protected string goalId;

    protected void Page_Load(object sender, EventArgs e)
    {
        indId = Request.QueryString["indId"];
        targId = Request.QueryString["targId"];
        UsersManager usersManager = new UsersManager();
        if (String.IsNullOrEmpty(indId) || String.IsNullOrEmpty(targId))
        {
            Response.Redirect("index.aspx");
            return;
        }
        hiddenTargId.Text = targId;

        if (IsPostBack)
        {
            return;
        }
        if (!usersManager.checkAccessIndicator(HttpContext.Current.User, indId))
        {
            Response.Redirect("index.aspx");
            return;
        }

        newDimensions.DataSource = Properties.dimensions;
        newDimensions.DataBind();

        QueryManager qm = new QueryManager();
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@IND_ID", indId },
                { "@TARG_ID", targId }
            };
            SqlDataReader reader = qm.executeReader("SELECT  I.Indicator_Code, " +
                "I.Indicator_descEn, I.Indicator_descAr, IC.Indicator_NL, T.Target_ID, T.Target_DescEn, " +
                "T.Target_DescAr, G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr " +
                "FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                "WHERE I.Indicator_Code IN(@IND_ID) AND IC.Target_ID IN (@TARG_ID)", parameters);
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
                }

            }
            finally
            {
                reader.Close();
            }

            refreshSubindicators(qm, 0);
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
        //     if (!usersManager.checkAccessGoal(HttpContext.Current.User, goalId))
        //    {
        //       Response.Redirect("index.aspx");
        //      return;
        //  }

    }

    private void refreshSubindicators()
    {
        QueryManager qm = new QueryManager();
        try
        {
            refreshSubindicators(qm, 0);
        }
        finally
        {
            qm.closeConnection();
        }
    }

    private void refreshSubindicators(QueryManager qm, int dbCheck)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@IND_ID", indCode.Text },
                { "@TARG_ID", hiddenTargId.Text }
            };
        try
        {
           DataSet mainDs = qm.executeReaderDataSet("SELECT  I.Indicator_Code, " +
                    "I.Indicator_descEn, I.Indicator_descAr, T.Target_ID, T.Target_DescEn, " +
                    "T.Target_DescAr, G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr, " +
                    "S.Subindicator_Code, S.Subindicator_DescAr, S.Subindicator_DescEn, S.Is_Uploaded, S.Dimensions, " +
                    "S.SERIES, S.UNIT_MULT, S.UNIT_MEASURE, S.OBS_STATUS " +
                    "FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                    "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                    "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                    "INNER JOIN Subindicator S ON S.Indicator_Code = I.Indicator_Code " +
                    "WHERE I.Indicator_Code IN(@IND_ID) AND IC.Target_ID IN (@TARG_ID)" +
                    "AND S.Is_Valid = 1 " +
                    "ORDER BY S.Subindicator_Code", parameters);
            SubDetail.DataSource = mainDs;
            SubDetail.DataBind();

            DataSet dsObsStatus = qm.executeReaderDataSet("SELECT OBS_STATUS," +
                " (OBS_STATUS + ' - ' + OBS_STATUS_DESC_EN) DESC_EN" +
                " FROM CL_OBS_STATUS" +
                " ORDER BY OBS_STATUS");
            DataSet dsUnitMeasure = qm.executeReaderDataSet("SELECT UNIT_MEASURE," +
                " (UNIT_MEASURE + ' - ' + UNIT_MEASURE_DESC_EN) DESC_EN" +
                " FROM CL_UNIT_MEASURE" +
                " ORDER BY UNIT_MEASURE");
            DataSet dsUnitMultiplier = qm.executeReaderDataSet("SELECT UNIT_MULT," +
                " (UNIT_MULT + ' - ' + UNIT_MULT_DESC_EN) DESC_EN" +
                " FROM CL_UNIT_MULT" +
                " ORDER BY UNIT_MULT");

            RepeaterItem rp = this.SubDetail.Items[this.SubDetail.Items.Count - 1];
            HiddenField goalIdField = (HiddenField)rp.FindControl("goalId");
            goalId = goalIdField.Value;
            backLink.HRef = "./target.aspx?goalId=" + goalIdField.Value;
            newSubSeries.Value = "";
            newUnitMeasure.DataSource = dsUnitMeasure;
            newUnitMeasure.DataBind();
            newUnitMultiplier.DataSource = dsUnitMultiplier;
            newUnitMultiplier.DataBind();
            newObsStatus.DataSource = dsObsStatus;
            newObsStatus.DataBind();
            newSubDescEN.Value = "";
            newSubDescAR.Value = "";
            for (int i = 0; i < this.SubDetail.Items.Count; i++)
            {
                RepeaterItem rp2 = this.SubDetail.Items[i];
                DropDownList ddl = (DropDownList)rp2.FindControl("obsStatus");
                ddl.DataSource = dsObsStatus;
                ddl.DataBind();
                ddl.SelectedValue = mainDs.Tables[0].Rows[i][17].ToString().Trim();
                ddl = (DropDownList)rp2.FindControl("unitMeasure");
                ddl.DataSource = dsUnitMeasure;
                ddl.DataBind();
                ddl.SelectedValue = mainDs.Tables[0].Rows[i][16].ToString().Trim();
                ddl = (DropDownList)rp2.FindControl("unitMultiplier");
                ddl.DataSource = dsUnitMultiplier;
                ddl.DataBind();
                ddl.SelectedValue = mainDs.Tables[0].Rows[i][15].ToString().Trim();
            }
        }
        catch
        {
            //there are no subindicators from this indicator
            SqlDataReader reader = qm.executeReader(
            "SELECT T.Target_ID, G.Goal_ID FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
            "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID WHERE IC.Indicator_Code IN(@ID) AND IC.Target_ID IN (@TARG_ID)", parameters);
            try
            {
                while (reader.Read())
                {
                    goalId = reader["Goal_ID"].ToString();
                    backLink.HRef = "./target.aspx?goalId=" + goalId;

                }
            }
            finally
            {
                reader.Close();
            }
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
        cleanItems();
        RepeaterItem item = getSelectedItem();
        QueryManager qm = new QueryManager();

        try
        {
            HiddenField indicatorCode = (HiddenField)item.FindControl("indicatorCode");
            HiddenField subindicatorCode = (HiddenField)item.FindControl("subindicatorCode");
            TextBox SERIES = (TextBox)item.FindControl("SERIES");
            DropDownList UNIT_MEASURE = (DropDownList)item.FindControl("unitMeasure");
            DropDownList UNIT_MULT = (DropDownList)item.FindControl("unitMultiplier");
            DropDownList OBS_STATUS = (DropDownList)item.FindControl("obsStatus");
            TextBox descEn = (TextBox)item.FindControl("descEn");
            TextBox descAr = (TextBox)item.FindControl("descAr");

            Dictionary<string, object> actionParam = new Dictionary<string, object>
            {
                { "@IND_CODE", indicatorCode.Value },
                { "@SUB_CODE", subindicatorCode.Value },
                { "@SERIES", SERIES.Text },
                { "@UNIT_MEASURE", UNIT_MEASURE.SelectedValue },
                { "@UNIT_MULT", UNIT_MULT.SelectedValue },
                { "@OBS_STATUS", OBS_STATUS.SelectedValue },
                { "@DESC_EN", descEn.Text },
                { "@DESC_AR", descAr.Text }
            };

            qm.executeNonQuery(
                "UPDATE" +
                " Subindicator " +
                "SET" +
                " SERIES = @SERIES," +
                " UNIT_MEASURE = @UNIT_MEASURE," +
                " UNIT_MULT = @UNIT_MULT," +
                " OBS_STATUS = @OBS_STATUS," +
                " Subindicator_DescEn = @DESC_EN," +
                " Subindicator_DescAr = @DESC_AR " +
                "WHERE" +
                " Indicator_Code = @IND_CODE" +
                " AND Subindicator_Code = @SUB_CODE", actionParam);

            qm.executeNonQuery(
                "UPDATE" +
                " SUBINDICATOR_DATA " +
                "SET" +
                " UNIT_MEASURE = @UNIT_MEASURE," +
                " UNIT_MULT = @UNIT_MULT," +
                " OBS_STATUS = @OBS_STATUS " +
                "WHERE" +
                " Indicator_Code = @IND_CODE" +
                " AND Subindicator_Code = @SUB_CODE", actionParam);

            refreshSubindicators(qm, 1);
        }
        catch
        {
            refreshSubindicators(qm, 2);
        }
        finally
        {
            qm.closeConnection();
        }
    }

    protected void Delete(object sender, EventArgs e)
    {
        cleanItems();
        RepeaterItem item = getSelectedItem();
        QueryManager qm = new QueryManager();

        try
        {
            HiddenField indicatorCode = (HiddenField)item.FindControl("indicatorCode");
            HiddenField subindicatorCode = (HiddenField)item.FindControl("subindicatorCode");

            Dictionary<string, object> actionParam = new Dictionary<string, object>
            {
                { "@IND_CODE", indicatorCode.Value },
                { "@SUB_CODE", subindicatorCode.Value }
            };

            qm.executeNonQuery(
                "UPDATE" +
                " Subindicator " +
                "SET" +
                " Is_Valid = 0 " +
                "WHERE" +
                " Indicator_Code = @IND_CODE" +
                " AND Subindicator_Code = @SUB_CODE", actionParam);

            refreshSubindicators(qm, 1);
        }
        catch
        {
            refreshSubindicators(qm, 2);
        }
        finally
        {
            qm.closeConnection();
        }
    }

    protected void Upload(object sender, EventArgs e)
    {
        refreshSubindicators();
        cleanItems();
        RepeaterItem item = getSelectedItem();
        HiddenField indicatorCode = (HiddenField)item.FindControl("indicatorCode");
        HiddenField subindicatorCode = (HiddenField)item.FindControl("subindicatorCode");
        Literal uploadLiteral = (Literal)item.FindControl("uploadResult");
        FileUpload excelFileUpload = (FileUpload)item.FindControl("excelFileUpload");
        string fileName = excelFileUpload.PostedFile.FileName;
        if (fileName == null || fileName == "")
        {
            uploadLiteral.Text = "No selected file!";
            return;
        }
        if (!Directory.Exists(@Server.MapPath("~/Temp/")))
        {
            Directory.CreateDirectory(@Server.MapPath("~/Temp/"));
        }

        string excelPath = Server.MapPath("~/Temp/") + Path.GetFileName(excelFileUpload.PostedFile.FileName);
        excelFileUpload.SaveAs(excelPath);

        DataUploadResult result = DataUploadManager.Upload(excelPath, fileName, indicatorCode.Value, subindicatorCode.Value);
        refreshSubindicators();
        item = getSelectedItem();
        uploadLiteral = (Literal)item.FindControl("uploadResult");

        if (result.getCode() == 0)
        {
            uploadLiteral.Text = "<div style='display: inline-block;'><span class='db-alert db-success'>Inserted: " + result.getInsertedData() + " Uploaded: " + result.getUpdatedData() + " Error: " + result.getWrongData() + "</span></div>";
        }
        else
        {
            uploadLiteral.Text = "<div style='display: inline-block;'><span class='db-alert db-error'>" + result.getMessage() + "</span></div>";
        }
    }

    private RepeaterItem getSelectedItem()
    {
        foreach (RepeaterItem item in SubDetail.Items)
        {
            if (item.ItemType == ListItemType.Item
                || item.ItemType == ListItemType.AlternatingItem)
            {
                Button save = (Button)item.FindControl("save");
                Button btnUpload = (Button)item.FindControl("btnUpload");
                Button delete = (Button)item.FindControl("delete");
                if (Request.Form.Get(save.UniqueID) != null
                    || Request.Form.Get(btnUpload.UniqueID) != null
                    || Request.Form.Get(delete.UniqueID) != null)
                {
                    return item;
                }
            }
        }
        return null;
    }

    protected void template_ServerClick(object sender, EventArgs e)
    {
        string subId = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@ID", subId }
            };
        QueryManager qm = new QueryManager();

        string fileName = "Subindicator_" + subId + "_template";
        try
        {
            SqlDataReader reader = qm.executeReader(
                "SELECT S.Dimensions" +
                " FROM Subindicator S" +
                " WHERE S.Subindicator_Code IN(@ID)", parameters);
            reader.Read();
            List<Dimension> dims = Properties.getDimensions(Convert.ToString(reader["Dimensions"]));
            reader.Close();

            DataTable dt = new DataTable("Table");
            dt.Columns.Add("Goal_ID", Type.GetType("System.String"));
            dt.Columns.Add("Goal_DescEn", Type.GetType("System.String"));
            dt.Columns.Add("Goal_DescAr", Type.GetType("System.String"));
            dt.Columns.Add("Target_ID", Type.GetType("System.String"));
            dt.Columns.Add("Target_DescEn", Type.GetType("System.String"));
            dt.Columns.Add("Target_DescAr", Type.GetType("System.String"));
            dt.Columns.Add("Indicator_NL", Type.GetType("System.String"));
            dt.Columns.Add("Indicator_Code", Type.GetType("System.String"));
            dt.Columns.Add("Indicator_descEn", Type.GetType("System.String"));
            dt.Columns.Add("Indicator_descAr", Type.GetType("System.String"));
            dt.Columns.Add("Subindicator_Code", Type.GetType("System.String"));
            dt.Columns.Add("Subindicator_DescEn", Type.GetType("System.String"));
            dt.Columns.Add("Subindicator_DescAr", Type.GetType("System.String"));
            foreach (Dimension d in dims)
            {
                dt.Columns.Add(d.getName(), Type.GetType("System.String"));
                dt.Columns.Add(d.getName() + "_DESC", Type.GetType("System.String"));
            }
            for (int i = 0; i < Properties.variables.Length; i++)
            {
                dt.Columns.Add(Properties.variables[i], Type.GetType("System.String"));
            }

            parameters = new Dictionary<string, object>
            {
                { "@ID", subId },
                { "@TARG_ID", hiddenTargId.Text }
            };
            string query;

            int count = 1;
            foreach (Dimension d in dims)
            {
                if (d.getTable() != null)
                {
                    query = "SELECT count(*) FROM " + d.getTable();
                    count *= qm.executeScalar(query, parameters);
                }
            }

            if (count > 10000)
            {
                query = "SELECT count(*)";
                query += " FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                        "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                        "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                        "INNER JOIN Subindicator S ON IC.Indicator_Code = S.Indicator_Code " +
                        "INNER JOIN SUBINDICATOR_DATA SD ON S.Subindicator_Code = SD.SUBINDICATOR_CODE ";
                query += " WHERE S.Subindicator_Code IN(@ID) AND IC.Target_ID IN (@TARG_ID)";
                count = qm.executeScalar(query, parameters);
                
                query = "SELECT  S.Subindicator_Code, " +
                        "S.Subindicator_DescEn, S.Subindicator_DescAr, " +
                        "I.Indicator_Code, IC.Indicator_NL, I.Indicator_descEn, I.Indicator_descAr, " +
                        "T.Target_ID, T.Target_DescEn, " +
                        "T.Target_DescAr, G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr";

                if (count == 0)
                {
                    for (int i = 0; i < Properties.variables.Length; i++)
                    {
                        query += ", null " + Properties.variables[i];
                    }
                    foreach (Dimension d in dims)
                    {
                        query += ", null " + d.getName() + "_DESC";
                    }
                }
                else
                {
                    query += ", SD.* ";
                    foreach (Dimension d in dims)
                    {
                        if (d.getTable() != null)
                        {
                            query += ", ISNULL(" + d.getTable() + "." + d.getDesc() + ", " + d.getTable() + "." + d.getDescEn() + ") " + d.getName() + "_DESC";
                        }
                        else
                        {
                            query += ", null " + d.getName() + "_DESC";
                        }
                    }
                }
                query += " FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                        "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                        "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                        "INNER JOIN Subindicator S ON IC.Indicator_Code = S.Indicator_Code ";
                if (count > 0)
                {
                    query += "INNER JOIN SUBINDICATOR_DATA SD ON S.Subindicator_Code = SD.SUBINDICATOR_CODE ";
                    foreach (Dimension d in dims)
                    {
                        if (d.getTable() != null)
                        {
                            query += " JOIN " + d.getTable() + " ON SD." + d.getName() + " = " + d.getTable() + "." + d.getKey();
                        }
                    }
                }
                query += " WHERE S.Subindicator_Code IN(@ID) AND IC.Target_ID IN (@TARG_ID)";
            }
            else
            {
                query = GetTemplateFullQuery(dims);
            }

            reader = qm.executeReader(query, parameters);
            List<string> tables = createTable(reader, dt, dims);
            reader.Close();

            using (XLWorkbook wb = new XLWorkbook(tables[0]))
            {
                dt = new DataTable("Table");
                dt.Columns.Add("ID", Type.GetType("System.String"));
                dt.Columns.Add("DESC_EN", Type.GetType("System.String"));
                dt.Columns.Add("DESC_AR", Type.GetType("System.String"));
                foreach (Dimension d in dims)
                {
                    if (d.getTable() != null)
                    {
                        query = "SELECT " + d.getKey() + "," + d.getDescEn() + "," + d.getDesc() + " FROM " + d.getTable();
                        reader = qm.executeReader(query, parameters);
                        while (reader.Read())
                        {
                            DataRow dr = dt.NewRow();
                            dr["ID"] = Convert.ToString(reader[d.getKey()]).Trim();
                            dr["DESC_EN"] = Convert.ToString(reader[d.getDescEn()]).Trim();
                            dr["DESC_AR"] = Convert.ToString(reader[d.getDesc()]).Trim();
                            dt.Rows.Add(dr);
                        }
                        reader.Close();
                        wb.Worksheets.Add(dt, d.getName().ToUpper());
                        dt.Clear();
                    }
                }
                wb.Save();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                if (tables.Count == 1)
                {
                    FileStream fs = new FileStream(tables[0], FileMode.Open);
                    fs.CopyTo(ms);
                    fs.Close();
                    File.Delete(tables[0]);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xlsx");
                    ms.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        int counter = 1;
                        foreach (string file in tables)
                        {
                            zip.AddFile(file, "").FileName = fileName + "_" + (counter++) + ".xlsx";
                        }
                        zip.Save(ms);
                        foreach (string file in tables)
                        {
                            File.Delete(file);
                        }

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/zip, application/octet-stream, application/x-zip-compressed, multipart/x-zip";
                        Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".zip");
                        ms.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
        finally
        {
            qm.closeConnection();
        }
    }

    private void cleanItems()
    {
        foreach (RepeaterItem item in SubDetail.Items)
        {
            if (item.ItemType == ListItemType.Item
                || item.ItemType == ListItemType.AlternatingItem)
            {
                Literal uploadLiteral = (Literal)item.FindControl("uploadResult");
                uploadLiteral.Text = "";
            }
        }
    }

    protected void AddNewSubIndicator(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Add new");
        cleanItems();
        QueryManager qm = new QueryManager();
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@ID", indCode.Text }
            };
            SqlDataReader reader = qm.executeReader("SELECT TOP 1 Subindicator_Code FROM Subindicator " +
                    "WHERE Indicator_Code IN (@ID) ORDER BY Subindicator_Code DESC; ", parameters);
            string newSubId = "";
            try
            {
                string subindicatorCode = "";
                while (reader.Read())
                {
                    subindicatorCode = Convert.ToString(reader["Subindicator_Code"]).Trim();
                }

                int intCode = 0;
                if (subindicatorCode != null && subindicatorCode.Trim().Length > 0)
                {
                    int.TryParse(subindicatorCode.Substring(subindicatorCode.LastIndexOf("_") + 1), out intCode);
                }

                intCode++;
                newSubId = ((intCode > 9) ? indCode.Text.Trim() + "_" + intCode : indCode.Text.Trim() + "_0" + intCode++);
                System.Diagnostics.Debug.WriteLine("New Subindicator_Code " + newSubId);
            }
            catch
            {
                refreshSubindicators(qm, 2);
            }
            finally
            {
                reader.Close();
                qm.closeConnection();
            }

            string dimensions = "";
            ListItemCollection items = newDimensions.Items;
            IEnumerator it = items.GetEnumerator();
            while (it.MoveNext())
            {
                ListItem li = (ListItem)it.Current;
                dimensions += li.Selected ? "1" : "0";
            }

            qm = new QueryManager();
            Dictionary<string, object> actionParam = new Dictionary<string, object>
            {
                { "@IND_CODE", indCode.Text},
                { "@SUB_CODE", newSubId },
                { "@SERIES", newSubSeries.Value },
                { "@UNIT_MEASURE", newUnitMeasure.SelectedValue },
                { "@UNIT_MULT", newUnitMultiplier.SelectedValue },
                { "@OBS_STATUS", newObsStatus.SelectedValue },
                { "@DESC_EN", newSubDescEN.Value },
                { "@DESC_AR", newSubDescAR.Value },
                { "@DIM", dimensions }
            };

            qm.executeNonQuery(
                "INSERT INTO" +
                " Subindicator " +
                "(SERIES, Subindicator_DescEn, Subindicator_DescAr, Indicator_Code, Subindicator_Code, Dimensions, UNIT_MEASURE, UNIT_MULT, OBS_STATUS)" +
                "VALUES (@SERIES, @DESC_EN, @DESC_AR, @IND_CODE, @SUB_CODE, @DIM, @UNIT_MEASURE, @UNIT_MULT, @OBS_STATUS)", actionParam);

            refreshSubindicators(qm, 1);
        }
        catch
        {
            refreshSubindicators(qm, 2);
        }
        finally
        {
            qm.closeConnection();
        }
    }

    private List<string> createTable(SqlDataReader reader, DataTable dt, List<Dimension> dims)
    {
        int rows = 0;
        List<string> tmpFiles = new List<string>();
        while (reader.Read())
        {
            DataRow dr = dt.NewRow();
            dr["Goal_ID"] = Convert.ToString(reader["Goal_ID"]).Trim();
            dr["Goal_DescEn"] = Convert.ToString(reader["Goal_DescEn"]).Trim();
            dr["Goal_DescAr"] = Convert.ToString(reader["Goal_DescAr"]).Trim();
            dr["Target_ID"] = Convert.ToString(reader["Target_ID"]).Trim();
            dr["Target_DescEn"] = Convert.ToString(reader["Target_DescEn"]).Trim();
            dr["Target_DescAr"] = Convert.ToString(reader["Target_DescAr"]).Trim();
            dr["Indicator_NL"] = Convert.ToString(reader["Indicator_NL"]).Trim();
            dr["Indicator_Code"] = Convert.ToString(reader["Indicator_Code"]).Trim();
            dr["Indicator_descEn"] = Convert.ToString(reader["Indicator_descEn"]).Trim();
            dr["Indicator_descAr"] = Convert.ToString(reader["Indicator_descAr"]).Trim();
            dr["Subindicator_Code"] = Convert.ToString(reader["Subindicator_Code"]).Trim();
            dr["Subindicator_DescEn"] = Convert.ToString(reader["Subindicator_DescEn"]).Trim();
            dr["Subindicator_DescAr"] = Convert.ToString(reader["Subindicator_DescAr"]).Trim();
            foreach (Dimension d in dims)
            {
                if (hasColumn(reader, d.getName()))
                {
                    dr[d.getName()] = Convert.ToString(reader[d.getName()]).Trim();
                    dr[d.getName() + "_DESC"] = Convert.ToString(reader[d.getName() + "_DESC"]).Trim();
                }
                else
                {
                    dr[d.getName()] = "";
                    dr[d.getName() + "_DESC"] = "";
                }
            }
            for (int i = 0; i < Properties.variables.Length; i++)
            {
                dr[Properties.variables[i]] = Convert.ToString(reader[Properties.variables[i]]).Trim();
            }
            dt.Rows.Add(dr);
            rows++;

            if (rows == 30000)
            {
                rows = 0;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Subindicators");
                    string file = Path.GetTempFileName();
                    File.Delete(file);
                    file = file + ".xlsx";
                    wb.SaveAs(file);
                    tmpFiles.Add(file);
                }
                dt.Clear();
            }
        }

        if (rows > 0 || tmpFiles.Count == 0)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Subindicators");
                string file = Path.GetTempFileName();
                File.Delete(file);
                file = file + ".xlsx";
                wb.SaveAs(file);
                tmpFiles.Add(file);
            }
        }

        return tmpFiles;
    }

    protected static string getDimensionsDescription(object dim)
    {
        List<Dimension> dims;
        if (dim.GetType() == typeof(string))
        {
            dims = Properties.getDimensions((string)dim);
        }
        else
        {
            dims = Properties.getDimensions((string)null);
        }
        string result = "";
        foreach (Dimension d in dims)
        {
            result += "<span>" + d.getName() + "</span>";
        }
        return result;
    }

    private static bool hasColumn(SqlDataReader reader, string ColumnName)
    {
        foreach (DataRow row in reader.GetSchemaTable().Rows)
        {
            if (row["ColumnName"].ToString() == ColumnName)
            {
                return true;
            }
        }
        return false;
    }

    private static string GetTemplateFullQuery(List<Dimension> dims)
    {
        string query;
        query = "SELECT S.Subindicator_Code, " +
                "S.Subindicator_DescEn, S.Subindicator_DescAr, " +
                "S.Indicator_Code, S.Indicator_NL, S.Indicator_descEn, S.Indicator_descAr, " +
                "S.Target_ID, S.Target_DescEn, " +
                "S.Target_DescAr, S.Goal_ID, S.Goal_DescEn, S.Goal_DescAr";
        for (int i = 0; i < Properties.variables.Length; i++)
        {
            query += ", SD." + Properties.variables[i];
        }
        foreach (Dimension d in dims)
        {
            if (d.getTable() != null)
            {
                query += ", S." + d.getName();
                query += ", S." + d.getName() + "_DESC";
            }
            else
            {
                query += ", null " + d.getName();
                query += ", null " + d.getName() + "_DESC";
            }
        }
        query += GetTemplateFromQuery(dims);
        return query;
    }

    private static string GetTemplateFromQuery(List<Dimension> dims)
    {
        string query;
        query = " FROM (SELECT  S.Subindicator_Code, " +
            "S.Subindicator_DescEn, S.Subindicator_DescAr, " +
            "I.Indicator_Code, IC.Indicator_NL, I.Indicator_descEn, I.Indicator_descAr, " +
            "T.Target_ID, T.Target_DescEn, " +
            "T.Target_DescAr, G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr ";
        foreach (Dimension d in dims)
        {
            if (d.getTable() != null)
            {
                query += ", " + d.getTable() + "." + d.getKey() + " " + d.getName();
                query += ", ISNULL(" + d.getTable() + "." + d.getDesc() + ", " + d.getTable() + "." + d.getDescEn() + ") " + d.getName() + "_DESC";
            }
            else
            {
                query += ", null " + d.getName();
                query += ", null " + d.getName() + "_DESC";
            }
        }
        query += " FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
             "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
             "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
             "INNER JOIN Subindicator S ON IC.Indicator_Code = S.Indicator_Code ";
        foreach (Dimension d in dims)
        {
            if (d.getTable() != null)
            {
                query += "," + d.getTable();
            }
        }
        query += " WHERE S.Subindicator_Code IN(@ID) AND IC.Target_ID IN (@TARG_ID)";
        query += ") S LEFT JOIN SUBINDICATOR_DATA SD ON S.Subindicator_Code = SD.SUBINDICATOR_CODE ";
        foreach (Dimension d in dims)
        {
            if (d.getTable() != null)
            {
                query += " AND SD." + d.getName() + " = S." + d.getName();
            }
        }
        return query;
    }

}

