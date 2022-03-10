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
    IndicatorsManagement im = new IndicatorsManagement();
    string goalID ;
    string goalTypeID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");

          goalID = Request.QueryString["gID"];
          goalTypeID = Request.QueryString["gtID"];
        editAction = String.Equals(Request.QueryString["ed"], "t");
        if (editAction) pageTitle.InnerText = "Edit Goal";
        else pageTitle.InnerText = "Add new Goal";

 
        backLinkGT.InnerText = im.getGoalTypeDescr(goalTypeID);
        backLinkGT.HRef = ResolveUrl("~/Management/indicators/type/typeList.aspx ");
        backLinkG.HRef = ResolveUrl("/Management/indicators/goal/GoalList.aspx?gtID=" + goalTypeID);
        backLinkG.InnerText = "Goal List";

        if (!this.IsPostBack)
        {

            ClearData(null, null);
            if (editAction && !String.IsNullOrEmpty(goalID))
            {

                QueryManager qm = new QueryManager();
                Dictionary<string, object> parameters = null;
                string cmdSelectType = "SELECT [Goal_ID] ,[Goal_Code] ,[Goal_DescEn],[Goal_DescAr] ,[GoalImageEn],[GoalImageAr],[Type_ID]  FROM  [Goal] where Goal_ID=@Goal_ID order by 1 asc";
                parameters = new Dictionary<string, object>
                     {
                        { "@Goal_ID",goalID }
                     };
                SqlDataReader reader = qm.executeReader(cmdSelectType, parameters);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {   
                        this.Goal_Code.Value = (string)reader["Goal_Code"].ToString();
                        this.Goal_DescEn.Value = (string)reader["Goal_DescEn"].ToString();
                        this.Goal_DescAr.Value = (string)reader["Goal_DescAr"].ToString();
                        this.GoalImageEn.Value = (string)reader["GoalImageEn"].ToString();
                        this.GoalImageAr.Value = (string)reader["GoalImageAr"].ToString();
     

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
        
        this.Goal_Code.Value = "";
        this.Goal_DescEn.Value = "";
        this.Goal_DescAr.Value = "";
        this.GoalImageEn.Value = "";
        this.GoalImageAr.Value = "";
        this.saveMessage.Text = "";
    }

    protected void saveGoal(object sender, EventArgs e)
    {

        IndicatorsManagement im = new IndicatorsManagement();
        try
        {
            if (!editAction)
            {
                SDGsWA.Bean.Goal goal = new SDGsWA.Bean.Goal(null, goalTypeID, this.Goal_Code.Value, this.Goal_DescEn.Value, this.Goal_DescAr.Value,  this.GoalImageEn.Value , this.GoalImageAr.Value );
                im.addGoal(goal);
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";

            }
            else
            {
                SDGsWA.Bean.Goal goal = new SDGsWA.Bean.Goal(goalID, goalTypeID, this.Goal_Code.Value, this.Goal_DescEn.Value, this.Goal_DescAr.Value, this.GoalImageEn.Value, this.GoalImageAr.Value);
                im.editGoal(goal);
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