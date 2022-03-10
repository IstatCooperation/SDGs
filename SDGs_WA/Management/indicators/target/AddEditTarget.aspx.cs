using SDGsWA.Bean;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Management_indicators_goal_AddEditGoal : System.Web.UI.Page
{

    bool editAction = false;
    string goalTypeID;
    string goalID;
    string targetID;
    IndicatorsManagement im = new IndicatorsManagement();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");

        goalTypeID = Request.QueryString["gtID"];
        goalID = Request.QueryString["gID"];
        targetID = Request.QueryString["tID"];

        editAction = String.Equals(Request.QueryString["ed"], "t");
        if (editAction) pageTitle.InnerText = "Edit Target";
        else pageTitle.InnerText = "Add new Target";

        backLinkGT.InnerText = im.getGoalTypeDescr(goalTypeID);
        backLinkGT.HRef = ResolveUrl("~/Management/indicators/type/typeList.aspx ");
        backLinkG.HRef = ResolveUrl("~/Management/indicators/goal/GoalList.aspx?gtID=" + goalTypeID);
        backLinkG.InnerText = im.getGoalDescr(goalID);
        backLinkT.HRef = ResolveUrl("~/Management/indicators/target/TargetList.aspx?gtID=" + goalTypeID + "&gID=" + goalID);
        backLinkT.InnerText = "Target list";

        if (!this.IsPostBack)
        {

            ClearData(null, null);
            if (editAction && !String.IsNullOrEmpty(targetID))
            {

                QueryManager qm = new QueryManager();
                Dictionary<string, object> parameters = null;
                string cmdSelectType = "SELECT  Target_ID,Target_Code, Goal_ID, Target_DescEn, Target_DescAr  FROM Target where Target_ID=@Target_ID";
                parameters = new Dictionary<string, object>
                     {
                        { "@Target_ID",targetID }
                     };
                SqlDataReader reader = qm.executeReader(cmdSelectType, parameters);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        this.Target_Code.Value = (string)reader["Target_Code"].ToString();
                    //    this.Goal_ID.Value = (string)reader["Goal_ID"].ToString();
                        
                        this.Target_DescEn.Value = (string)reader["Target_DescEn"].ToString();
                        this.Target_DescAr.Value = (string)reader["Target_DescAr"].ToString();
                
                    }
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

    }


    protected void ClearData(object sender, EventArgs e)
    {
      
        this.Target_Code.Value = "";
        this.Target_DescEn.Value = "";
  ;
        this.Target_DescAr.Value = "";
    
    }

    protected void saveCmd(object sender, EventArgs e)
    {

       IndicatorsManagement im = new IndicatorsManagement();
        try
        {
            if (!editAction)
            {
                SDGsWA.Bean.Target t = new SDGsWA.Bean.Target(null, this.Target_Code.Value, this.Target_DescEn.Value, this.Target_DescAr.Value, goalID);
                im.addTarget(t);
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";

            }
            else
            {
                SDGsWA.Bean.Target t = new SDGsWA.Bean.Target(targetID, this.Target_Code.Value, this.Target_DescEn.Value, this.Target_DescAr.Value, goalID);
                im.editTarget(t);
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