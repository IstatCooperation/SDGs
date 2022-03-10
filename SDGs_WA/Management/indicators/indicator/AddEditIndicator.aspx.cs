using SDGsWA.Bean;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Management_indicators_AddEditIndicator : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    Boolean editAction = false;
    Boolean addExisting = false;

    string goalID;
    string goalTypeID;
    string targetID;
    string indicatorCode = null;
    IndicatorsManagement im = new IndicatorsManagement();
    protected void Page_Load(object sender, EventArgs e)
    {
        string goalID = Request.QueryString["gID"];
        goalTypeID = Request.QueryString["gtID"];

        targetID = Request.QueryString["tID"];

        if (!this.Page.User.IsInRole("Admin") || String.IsNullOrEmpty(goalID) || String.IsNullOrEmpty(goalTypeID) || String.IsNullOrEmpty(targetID))
            Response.Redirect("~/index.aspx");


        editAction = String.Equals(Request.QueryString["ed"], "t");
        addExisting = String.Equals(Request.QueryString["ex"], "t");
        if (editAction) pageTitle.InnerText = "Edit Indicator";
        else pageTitle.InnerText = "Add new Indicator";

        backLinkGT.InnerText = im.getGoalTypeDescr(goalTypeID);
        backLinkGT.HRef = ResolveUrl("~/Management/indicators/type/typeList.aspx ");
        backLinkG.HRef = ResolveUrl("~/Management/indicators/goal/GoalList.aspx?gtID=" + goalTypeID);
        backLinkG.InnerText = im.getGoalDescr(goalID);
        backLinkT.HRef = ResolveUrl("~/Management/indicators/target/TargetList.aspx?gtID=" + goalTypeID + "&gID=" + goalID);
        backLinkT.InnerText = Utility.StringCrop(im.getTargetDescr(targetID), 50);
        backLinkI.HRef = ResolveUrl("~/Management/indicators/indicator/IndicatorList.aspx?gtID=" + goalTypeID + "&gID=" + goalID + "&tID=" + targetID);
        backLinkI.InnerText = "Indicator List";
        indicatorsTarget.Text = "-";
        if (!this.IsPostBack)
        {
            string selectCmd;
            QueryManager qm = new QueryManager();
            Dictionary<string, object> parameters;

            SDGsWA.Bean.Target target = im.getTargetById(targetID);
            this.targetLabel.Text = target.code + " - " + target.descEn;
            if (editAction)
            {
                indicatorCode = Request.QueryString["ic"];
                selectCmd = "SELECT I.Indicator_Code , CM.Value as Indicator_Code_Value , " +
                      "I.Indicator_descEn, I.Indicator_descAr, IC.Indicator_NL, T.Target_ID,T.Target_Code, T.Target_DescEn, " +
                      "T.Target_DescAr FROM  Target T  " +
                          "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                          "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                          "INNER JOIN Code_Mapping CM ON(CM.CODE = I.Indicator_Code AND CM.GOAL_TYPE = @GOAL_TYPE) " +
                          "where  T.Target_ID=@Target_ID AND  I.IS_ACTIVE=1 AND T.IS_ACTIVE=1";
                parameters = new Dictionary<string, object>
                     {
                     { "@GOAL_TYPE",goalTypeID },
                        { "@Target_ID",targetID }
                     };




                SqlDataReader reader = qm.executeReader(selectCmd, parameters);



                if (reader.HasRows)
                {
                    indicatorsTarget.Text = "<ul>";
                    while (reader.Read())
                    {
                        string indicatorCodeI = (string)reader["Indicator_Code"].ToString().Trim();


                        if (!String.IsNullOrEmpty(indicatorCode) && indicatorCodeI.Equals(indicatorCode))
                        {
                            this.Indicator_NL.Value = (string)reader["Indicator_NL"].ToString().Trim();
                            Indicator_Code_ID_label.Text = indicatorCodeI;
                            this.Indicator_Code_Value.Value = (string)reader["Indicator_Code_Value"].ToString();
                            this.Indicator_descEn.Text = (string)reader["Indicator_descEn"].ToString();
                            this.Indicator_descAr.Text = (string)reader["Indicator_descAr"].ToString();
                            indicatorsTarget.Text += "<li><b>" + (string)reader["Indicator_NL"].ToString().Trim() + " - " + reader["Indicator_Code_Value"].ToString().Trim() + "</b></li>";

                            this.relatedCodes.Text = im.getOtherCodeValuesIndicator(goalTypeID, goalID, targetID, indicatorCode);
                        }
                        else
                             if (!String.IsNullOrEmpty(indicatorCode))
                            indicatorsTarget.Text += "<li>" + (string)reader["Indicator_NL"].ToString().Trim() + " - " + reader["Indicator_Code_Value"].ToString().Trim() + "</li>";


                    }
                    indicatorsTarget.Text += "</ul>";
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            if (addExisting)
            {

                loadDdl(ddlGoalType, "SELECT Type_ID ID,  Descr_Short VALUE FROM Goal_Type WHERE IS_ACTIVE=1 order by Type_ID asc ", null);

            }
        }

    }



    protected Boolean isEditAction()
    {
        return editAction;
    }
    protected Boolean isAddExisting()
    {
        return addExisting;
    }
    protected Boolean isSameGoalType()
    {
        return goalTypeID.Equals(ddlGoalType.SelectedItem.Value);
    }

    

    protected void ClearData(object sender, EventArgs e)
    {
        this.Indicator_NL.Value = "";

        this.Indicator_descEn.Text = "";
        this.Indicator_descAr.Text = "";
        ddlGoalType.SelectedIndex = 0;
        ddlGoal.Items.Clear();
        ddlTarget.Items.Clear();
        ddlIndicator.Items.Clear();
        this.relatedCodes.Text = "";
        this.Indicator_Code_ID_Ex.Text = "";
        this.Indicator_Code_Value_Label_Ex.Value = "";
        this.Indicator_descAr_Ex.Text = "";
        this.Indicator_descEn_Ex.Text = "";
        this.Indicator_NL_EX.Value = "";
        this.saveMessage.Text = "";

    }


    private void loadDdl(DropDownList ddl, string query, Dictionary<string, string> parameters)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, string> item in parameters)
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    ddl.DataSource = dt;
                    ddl.DataBind();
                    ddl.DataTextField = "VALUE";
                    ddl.DataValueField = "ID";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("Select ...", String.Empty));
                    ddl.SelectedIndex = 0;

                }
            }
        }
    }

    protected void ddlGoalType_Selection_Change(Object sender, EventArgs e)
    {
        Dictionary<string, String> parameters = new Dictionary<string, String>
            {
                { "@Type_ID",ddlGoalType.SelectedItem.Value }
            };
        ddlGoal.Items.Clear();
        ddlTarget.Items.Clear();
        ddlIndicator.Items.Clear();
        loadDdl(ddlGoal, "SELECT Goal_ID ID, Goal_Code +' - '+  Goal_DescEn VALUE FROM Goal  where Type_ID=@Type_ID AND IS_ACTIVE=1  order by 1 asc  ", parameters);
        Indicator_Code_Value_Label_Ex.Disabled=isSameGoalType(); 
    }

    protected void ddlGoal_Selection_Change(Object sender, EventArgs e)
    {
        Dictionary<string, String> parameters = new Dictionary<string, String>
            {
                { "@Goal_ID",ddlGoal.SelectedItem.Value }
            };

        ddlTarget.Items.Clear();
        ddlIndicator.Items.Clear();
        loadDdl(ddlTarget, "SELECT  Target_ID ID,Target_Code+' - '+ Target_DescEn VALUE   FROM Target where Goal_ID=@Goal_ID AND IS_ACTIVE=1 order by 1 asc ", parameters);

    }

    protected void ddlTarget_Selection_Change(Object sender, EventArgs e)
    {
        Dictionary<string, String> parameters = new Dictionary<string, String>
            {
                { "@Target_ID",ddlTarget.SelectedItem.Value },
                 { "@GOAL_TYPE",ddlGoalType.SelectedItem.Value }
            };
        ddlIndicator.Items.Clear();

        string selectCmd = "SELECT I.Indicator_Code ID, IC.Indicator_NL +' - '+ CM.Value +' - '+ I.Indicator_descEn as VALUE  FROM  Target T  " +
                "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                "INNER JOIN Code_Mapping CM ON(CM.CODE = I.Indicator_Code AND CM.GOAL_TYPE = @GOAL_TYPE) " +
                "where  T.Target_ID=@Target_ID AND  I.IS_ACTIVE=1 AND T.IS_ACTIVE=1 order by 1 asc";

        loadDdl(ddlIndicator, selectCmd, parameters);

    }


    protected void ddlIndicator_Selection_Change(Object sender, EventArgs e)
    {

        loadIndicatorInfo(ddlIndicator.SelectedItem.Value);

    }
    protected void loadIndicatorInfo(string indicatorCodeSel)
    {

        SDGsWA.Bean.Indicator indicator = im.getIndicatorByCodeAndType(indicatorCodeSel, ddlGoalType.SelectedItem.Value);

        this.Indicator_Code_Value_Label_Ex.Value = indicator.codeValue.Trim();
        this.Indicator_Code_ID_Ex.Text = indicator.codeId.Trim();
        this.Indicator_descAr_Ex.Text = indicator.descAr;
        this.Indicator_descEn_Ex.Text = indicator.descEn;

        this.relatedCodes.Text = im.getOtherCodeValuesIndicator(ddlGoalType.SelectedItem.Value, ddlGoal.SelectedItem.Value, ddlTarget.SelectedItem.Value, indicatorCodeSel);
    }


    protected void saveCmd(object sender, EventArgs e)
    {


        try
        {
            if (!editAction)
            {/// 
                if (addExisting)
                {
                    IndicatorCode ic = new IndicatorCode(targetID, ddlIndicator.SelectedItem.Value.Trim(), Indicator_Code_Value_Label_Ex.Value.Trim(), Indicator_NL_EX.Value.Trim(), "","", goalTypeID);

                    im.addExistingIndicator(ic,isSameGoalType());
                }
                else
                {
                    IndicatorCode ic = new IndicatorCode(targetID, Indicator_Code_ID.Value.Trim(), Indicator_Code_Value.Value.Trim(), Indicator_NL.Value.Trim(), Indicator_descEn.Text.Trim(), Indicator_descAr.Text.Trim(), goalTypeID);

                    im.addNewIndicator(ic);
                }
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";

            }
            else
            {
                IndicatorCode ic = new IndicatorCode(targetID, Indicator_Code_ID_label.Text, Indicator_Code_Value.Value, Indicator_NL.Value, Indicator_descEn.Text, Indicator_descAr.Text, goalTypeID);
                im.editIndicatorCode(ic);
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";

            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            saveMessage.Text = "<div class='db-alert db-error '>Warning! Data not updated :" + ex.Message + " </div>";
        }

    }

}