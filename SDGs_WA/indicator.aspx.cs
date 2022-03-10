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
    protected string goalType;
    protected string goalTypeLabel;
    protected SWAGlobalProperties swgp;

   Dictionary<string, string> goalTypesIdDescr = new Dictionary<string, string>();
    Dictionary<string, string> newSubIindicatorCodeValuesSuggest = new Dictionary<string, string>();
    Dictionary<string, string> subIndicatorCodeValues  = new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        indId = Request.QueryString["indId"];
        targId = Request.QueryString["targId"];
        UsersManager usersManager = new UsersManager();
        goalType = (string)HttpContext.Current.Session["GOAL_TYPE"];
        goalTypeLabel = (string)HttpContext.Current.Session["GOAL_LABEL"];
        if (String.IsNullOrEmpty(indId) || String.IsNullOrEmpty(targId) || String.IsNullOrEmpty(goalType) || String.IsNullOrEmpty(goalTypeLabel))
        {
            Response.Redirect("index.aspx");
            return;
        }
        hiddenTargId.Text = targId;

        loadGoalTypesIdDescr();
        swgp = (SWAGlobalProperties)Application["SDGsWAGlobalProperties"];

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
                { "@TARG_ID", targId },
                { "@GOAL_TYPE", goalType }
            };
            SqlDataReader reader = qm.executeReader("SELECT  I.Indicator_Code, CM.Value as Indicator_Code_Value, " +
                "I.Indicator_descEn, I.Indicator_descAr, IC.Indicator_NL, T.Target_ID,T.Target_Code, T.Target_DescEn, " +
                "T.Target_DescAr, G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr " +
                "FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                "INNER JOIN Code_Mapping CM ON   (CM.CODE=I.Indicator_Code AND CM.GOAL_TYPE IN (@GOAL_TYPE)) " +
                "WHERE I.Indicator_Code IN(@IND_ID) AND IC.Target_ID IN (@TARG_ID) AND I.IS_ACTIVE=1", parameters);
            title.Text = "";
            subTitle.Text = "";
            try
            {
                if (reader.Read())
                {
                    title.Text = Convert.ToString(reader["Target_Code"]).Trim() + " - " + Convert.ToString(reader["Target_DescEn"]).Trim();
                    indCode.Text = Convert.ToString(reader["Indicator_Code"]).Trim();
                    indCodeValue.Text = Convert.ToString(reader["Indicator_Code_Value"]).Trim();
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

       
    
        populateNewSubIndicatorCodeValuesGrid();
    }

    private void loadGoalTypesIdDescr()
    {
        QueryManager qm = new QueryManager();
        SqlDataReader reader = null;
        try
        {
            reader = qm.executeReader("SELECT  GT.Type_ID,GT.Descr_Short  FROM GOAL_TYPE GT WHERE GT.IS_ACTIVE=1", null);

            while (reader.Read())
            {
                goalTypesIdDescr.Add(Convert.ToString(reader["Type_ID"]).Trim(), Convert.ToString(reader["Descr_Short"]).Trim());

            }


        }
        finally
        {
            if (reader != null) reader.Close();
            qm.closeConnection();
        }

       
    }

    private void addRowSubIndicatorCodeValuesGrid(GridView subIndicatorCodeValuesGrid, string gtype, string codeId, string codeValue)
    {
        DataTable dt = new DataTable();
       
        dt.Columns.Add(new DataColumn("subIndicatorCodeGoalTypeId", System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("subIndicatorCodeValue", System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("subIndicatorCodeGoalTypeDescr", System.Type.GetType("System.String")));

        DataRow dr = dt.NewRow();
        dr["subIndicatorCodeGoalTypeId"] = gtype;
        dr["subIndicatorCodeValue"] = codeValue;
        dr["subIndicatorCodeGoalTypeDescr"] = goalTypesIdDescr[gtype];
        dt.Rows.Add(dr);


        foreach (var item in getOtherCodeValues(codeId))
        {
             dr = dt.NewRow();
            dr["subIndicatorCodeGoalTypeId"] = item.Key;
            dr["subIndicatorCodeValue"] = item.Value ;
            dr["subIndicatorCodeGoalTypeDescr"] = goalTypesIdDescr[item.Key];
            dt.Rows.Add(dr);

        }
        dt.AcceptChanges();
        DataView view = new DataView(dt);
        subIndicatorCodeValuesGrid.DataSource = view;
        subIndicatorCodeValuesGrid.DataBind();

    }

    private void populateNewSubIndicatorCodeValuesGrid()
    {


        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("newSubIndicatorCodeGoalTypeId", System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("newSubIndicatorCodeValue", System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("newSubIndicatorCodeGoalTypeDescr", System.Type.GetType("System.String")));


        QueryManager qm = new QueryManager();
        SqlDataReader reader = null;
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@ID", indCode.Text },
                   {"@GOAL_TYPE", goalType}
            };

            reader = qm.executeReader("SELECT GT.Subindicator_Separator " +
                "FROM GOAL_TYPE GT " +
                "INNER JOIN GOAL G ON GT.Type_ID = G.Type_ID " +
                "INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                "WHERE IC.Indicator_Code = @ID AND  gt.Type_ID=@GOAL_TYPE", parameters);
            reader.Read();
            string separator = Convert.ToString(reader["Subindicator_Separator"]).Trim();
            separator = String.IsNullOrWhiteSpace(separator) ? "" : separator;
            reader.Close();

            reader = qm.executeReader("SELECT count(*) FROM Subindicator WHERE Indicator_Code = @ID", parameters);


            int subindicatorCout = 0;


            reader.Read();
            subindicatorCout =reader.GetInt32(0) + 1 ;
            newSubIndicatorID.Value = indCode.Text.Trim() + separator + ((subindicatorCout > 9) ? "" : "0") + subindicatorCout;
            System.Diagnostics.Debug.WriteLine("New Subindicator_Code " + newSubIndicatorID.Value);
            reader.Close();

            Dictionary<string, object> actionParam = new Dictionary<string, object>
            {
                { "@IND_CODE", indCode.Text}
            };


            reader = qm.executeReader("SELECT cm.Goal_Type, cm.Value, gt.Subindicator_Separator " +
                   " FROM code_mapping cm, Goal_Type gt WHERE cm.Goal_Type = gt.Type_ID  and code =@IND_CODE", actionParam);

            Dictionary<string, String> gaolTypeValues = new Dictionary<string, string>();


            while (reader.Read())
            {
                string goalTypeI = Convert.ToString(reader["Goal_Type"]).Trim();
                string separatorTypeI = Convert.ToString(reader["Subindicator_Separator"]).Trim();
                separatorTypeI = String.IsNullOrWhiteSpace(separatorTypeI) ? "" : separatorTypeI;
                string indCodeValueTypeI = Convert.ToString(reader["Value"]).Trim();
                String newSubIdValueTypeI = indCodeValueTypeI + separatorTypeI + ((subindicatorCout > 9) ? "" : "0") + subindicatorCout;
                System.Diagnostics.Debug.WriteLine("GOAL TYPE:" + goalTypeI + "; New Subindicator_Code_Value; " + newSubIdValueTypeI);
                gaolTypeValues.Add(goalTypeI, newSubIdValueTypeI);
            }//while
            reader.Close();


            foreach (var item in gaolTypeValues)
            {
                dr = dt.NewRow();
                dr["newSubIndicatorCodeGoalTypeId"] = item.Key.ToString();
                dr["newSubIndicatorCodeValue"] = item.Value.ToString();
                dr["newSubIndicatorCodeGoalTypeDescr"] = goalTypesIdDescr[item.Key];
                dt.Rows.Add(dr);

            }//for
            dt.AcceptChanges();
            DataView view = new DataView(dt);
            newSubIndicatorCodeValuesGrid.DataSource = view;
            newSubIndicatorCodeValuesGrid.DataBind();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);

        }
        finally
        {
            if (reader != null) reader.Close();
            qm.closeConnection();
        }


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

    private void refreshSubindicators(QueryManager qm, int dbCheck,string msg=null)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@IND_ID", indCode.Text },
                { "@TARG_ID", hiddenTargId.Text },
                { "@GOAL_TYPE", goalType }
            };
        try
        {

            SqlDataReader reader = qm.executeReader("SELECT  T.Goal_ID " +
                 "FROM Target T WHERE  T.Target_ID IN (@TARG_ID) ", parameters);

            reader.Read();
            goalId = Convert.ToString(reader["Goal_ID"]).Trim();

            reader.Close();

            DataSet mainDs = qm.executeReaderDataSet("SELECT  I.Indicator_Code,  " +
                     "I.Indicator_descEn, I.Indicator_descAr, T.Target_ID, T.Target_DescEn, " +
                     "T.Target_DescAr, G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr, " +
                     "S.Subindicator_Code , CMS_SubInd.Value as SubIndicator_Code_Value, S.Subindicator_DescAr, S.Subindicator_DescEn, S.Is_Uploaded, S.Dimensions, " +
                     "S.SERIES, S.UNIT_MULT, S.UNIT_MEASURE, S.OBS_STATUS, CMS_SubInd.GOAL_TYPE  " +
                     "FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                     "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                     "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                     "INNER JOIN Subindicator S ON S.Indicator_Code = I.Indicator_Code " +
                     //      "INNER JOIN Code_Mapping CM_Ind ON   (CM_Ind.CODE=I.Indicator_Code AND CM_Ind.GOAL_TYPE =@GOAL_TYPE) " +
                     "INNER JOIN Code_Mapping CMS_SubInd ON   (CMS_SubInd.CODE=S.Subindicator_Code AND CMS_SubInd.GOAL_TYPE = @GOAL_TYPE) " +
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


            backLink.HRef = "./target.aspx?goalId=" + goalId;
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
                ddl.SelectedValue = mainDs.Tables[0].Rows[i]["OBS_STATUS"].ToString().Trim();
                ddl = (DropDownList)rp2.FindControl("unitMeasure");
                ddl.DataSource = dsUnitMeasure;
                ddl.DataBind();
                ddl.SelectedValue = mainDs.Tables[0].Rows[i]["UNIT_MEASURE"].ToString().Trim();
                ddl = (DropDownList)rp2.FindControl("unitMultiplier");
                ddl.DataSource = dsUnitMultiplier;
                ddl.DataBind();
                ddl.SelectedValue = mainDs.Tables[0].Rows[i]["UNIT_MULT"].ToString().Trim();

                HiddenField subindicatorCodeHF= (HiddenField)rp2.FindControl("subindicatorCode");
                HiddenField subindicatorCodeValueHF = (HiddenField)rp2.FindControl("subindicatorCodeValue");
                GridView subIndicatorCodeValuesGrid= (GridView)rp2.FindControl("subIndicatorCodeValuesGrid");
                addRowSubIndicatorCodeValuesGrid(subIndicatorCodeValuesGrid , goalType, subindicatorCodeHF.Value, subindicatorCodeValueHF.Value);

            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            //there are no subindicators from this indicator
            SqlDataReader reader = qm.executeReader(
            "SELECT T.Target_ID, G.Goal_ID FROM GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
            "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID WHERE IC.Indicator_Code IN(@IND_ID) AND IC.Target_ID IN (@TARG_ID)", parameters);
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
                saveMessage.Text = "<div class='db-alert db-error hideMe'>Warning! Data not updated: "+msg+" </div>";
                break;
        }
    }

    protected void Save(object sender, EventArgs e)
    {
        cleanItems();
        RepeaterItem item = getSelectedItem();
        QueryManager qm = new QueryManager();
        SqlTransaction trans = null;

        try
        {
            HiddenField indicatorCode = (HiddenField)item.FindControl("indicatorCode");
            HiddenField subindicatorCode = (HiddenField)item.FindControl("subindicatorCode");
            System.Web.UI.WebControls.TextBox SERIES = (System.Web.UI.WebControls.TextBox)item.FindControl("SERIES");
            DropDownList UNIT_MEASURE = (DropDownList)item.FindControl("unitMeasure");
            DropDownList UNIT_MULT = (DropDownList)item.FindControl("unitMultiplier");
            DropDownList OBS_STATUS = (DropDownList)item.FindControl("obsStatus");
            System.Web.UI.WebControls.TextBox descEn = (System.Web.UI.WebControls.TextBox)item.FindControl("descEn");
            System.Web.UI.WebControls.TextBox descAr = (System.Web.UI.WebControls.TextBox)item.FindControl("descAr");

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
            trans = qm.getTransaction();
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
                " AND Subindicator_Code = @SUB_CODE", trans, actionParam);

            qm.executeNonQuery(
                "UPDATE" +
                " SUBINDICATOR_DATA " +
                "SET" +
                " UNIT_MEASURE = @UNIT_MEASURE," +
                " UNIT_MULT = @UNIT_MULT," +
                " OBS_STATUS = @OBS_STATUS " +
                "WHERE" +
                " Indicator_Code = @IND_CODE" +
                " AND Subindicator_Code = @SUB_CODE", trans, actionParam);

            GridView subIndicatorCodeValuesGrid = (GridView)item.FindControl("subIndicatorCodeValuesGrid");
            for (int i = 0; i < subIndicatorCodeValuesGrid.Rows.Count; i++)
            {

                String goalTypeI = ((Label)subIndicatorCodeValuesGrid.Rows[i].FindControl("subIndicatorCodeGoalTypeId")).Text.Trim();
                String codeValuesI = ((TextBox)subIndicatorCodeValuesGrid.Rows[i].FindControl("subIndicatorCodeValue")).Text.Trim();

                Dictionary<string, object> actionParamValue = new Dictionary<string, object>
            {

                { "@SUB_CODE", subindicatorCode.Value   },
                { "@GOAL_TYPE", goalTypeI },
                { "@SUB_CODE_VALUE", codeValuesI }

            };

                qm.executeNonQuery(
                             "UPDATE  Code_Mapping  set value= @SUB_CODE_VALUE " +
                             " WHERE code=@SUB_CODE AND Goal_Type=@GOAL_TYPE ", trans, actionParamValue);
            }//for
            trans.Commit();


            refreshSubindicators(qm, 1);
        }
        catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            refreshSubindicators(qm, 2, ex.Message) ;
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
        SqlTransaction trans = null;
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
            trans = qm.getTransaction();
            qm.executeNonQuery(
                "UPDATE" +
                " Subindicator " +
                "SET" +
                " Is_Valid = 0 " +
                "WHERE" +
                " Indicator_Code = @IND_CODE" +
                " AND Subindicator_Code = @SUB_CODE", trans, actionParam);

            qm.executeNonQuery("DELETE FROM Code_Mapping WHERE code=@SUB_CODE ", trans, actionParam);

            trans.Commit();

            refreshSubindicators(qm, 1);
        }
        catch
        {
            if (trans != null) trans.Rollback();
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
                System.Web.UI.WebControls.Button save = (System.Web.UI.WebControls.Button)item.FindControl("save");
                System.Web.UI.WebControls.Button btnUpload = (System.Web.UI.WebControls.Button)item.FindControl("btnUpload");
                System.Web.UI.WebControls.Button delete = (System.Web.UI.WebControls.Button)item.FindControl("delete");
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
                    if (count > 10000) break;
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
        SqlTransaction trans = null;
        SqlDataReader reader = null;
        QueryManager qm = new QueryManager();

        string dimensions = "";
        ListItemCollection items = newDimensions.Items;
        IEnumerator it = items.GetEnumerator();
        while (it.MoveNext())
        {
            ListItem li = (ListItem)it.Current;
            dimensions += li.Selected ? "1" : "0";
        }

        qm = new QueryManager();
        trans = qm.getTransaction();
        Dictionary<string, object> actionParam = new Dictionary<string, object>
            {
                { "@IND_CODE", indCode.Text},
                { "@SUB_CODE", newSubIndicatorID.Value  },
                { "@SERIES", newSubSeries.Value },
                { "@UNIT_MEASURE", newUnitMeasure.SelectedValue },
                { "@UNIT_MULT", newUnitMultiplier.SelectedValue },
                { "@OBS_STATUS", newObsStatus.SelectedValue },
                { "@DESC_EN", newSubDescEN.Value },
                { "@DESC_AR", newSubDescAR.Value },
                { "@DIM", dimensions }
            };


        // insert code mapping value for each goaltype
        try
        {


            qm.executeNonQuery(
            "INSERT INTO" +
            " Subindicator " +
            "(SERIES, Subindicator_DescEn, Subindicator_DescAr, Indicator_Code, Subindicator_Code, Dimensions, UNIT_MEASURE, UNIT_MULT, OBS_STATUS,Is_Uploaded,Is_Valid)" +
            "VALUES (@SERIES, @DESC_EN, @DESC_AR, @IND_CODE, @SUB_CODE, @DIM, @UNIT_MEASURE, @UNIT_MULT, @OBS_STATUS,0,1)", trans, actionParam);


            for (int i = 0; i < newSubIndicatorCodeValuesGrid.Rows.Count; i++)
            {



                String goalTypeI = ((Label)newSubIndicatorCodeValuesGrid.Rows[i].FindControl("newSubIndicatorCodeGoalTypeId")).Text.Trim();
                String codeValuesI = ((TextBox)newSubIndicatorCodeValuesGrid.Rows[i].FindControl("newSubIndicatorCodeValue")).Text.Trim();

                Dictionary<string, object> actionParamValue = new Dictionary<string, object>
            {

                { "@SUB_CODE", newSubIndicatorID.Value  },
                { "@GOAL_TYPE", goalTypeI },
                { "@SUB_CODE_VALUE", codeValuesI }

            };

                qm.executeNonQuery(
                             "INSERT INTO Code_Mapping (code,Goal_Type,value)" +
                             "VALUES (@SUB_CODE, @GOAL_TYPE, @SUB_CODE_VALUE)", trans, actionParamValue);
            }//for
            trans.Commit();
            }
            catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);

        }
        finally
        {
            if (reader != null) reader.Close();
            qm.closeConnection();
        }
        refreshSubindicators();

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

    protected string getOtherCodeValues(object subindicatorCode)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@CODE", ((string)subindicatorCode) } ,
                { "@GOAL_TYPE",goalType }
            };
        string result = "";
        QueryManager qm = new QueryManager();
        SqlDataReader reader = null;
        try
        {

            reader = qm.executeReader("Select cm.Value as Code,gt.Descr_Short as GoalType from Code_Mapping cm ,Goal_Type gt " +
                                 " WHERE gt.Type_ID = cm.Goal_Type and cm.code =@CODE and gt.Type_ID <> @GOAL_TYPE", parameters);

            while (reader.Read())
            {

                result += "<span>(" + Convert.ToString(reader["GoalType"]).Trim() + ": " + Convert.ToString(reader["Code"]).Trim() + ")</span>";

            }
        }
        finally
        {
            reader.Close();
        }
        return result;
    }

    protected Dictionary<string,string> getOtherCodeValues(string subindicatorCode)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@CODE",  subindicatorCode } ,
                { "@GOAL_TYPE",goalType }
            };
        Dictionary<string, string> result = new Dictionary<string, string>() ;
        QueryManager qm = new QueryManager();
        SqlDataReader reader = null;
        try
        {

            reader = qm.executeReader("Select cm.Value as Code,gt.Type_ID as GoalType from Code_Mapping cm ,Goal_Type gt " +
                                 " WHERE gt.Type_ID = cm.Goal_Type and cm.code =@CODE and gt.Type_ID <> @GOAL_TYPE", parameters);

            while (reader.Read())
            {

                result.Add(Convert.ToString(reader["GoalType"]).Trim(),Convert.ToString(reader["Code"]).Trim());

            }
        }
        finally
        {
            reader.Close();
        }
        return result;
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

