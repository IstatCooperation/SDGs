using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = null;
        
        string cmdGoals = "SELECT * from GOAL";
        if (!User.IsInRole("Admin")){
           cmdGoals = "SELECT distinct g.* from GOAL g, Target t, Ind_code ic,User_indicator ui,Users u " +
                " where g.Goal_ID=t.Goal_ID and t.target_id=ic.target_ID and ui.ind_code=ic.indicator_code and ui.User_ID=u.User_Id and u.username=@username";
            parameters = new Dictionary<string, object>
            {
                { "@username", HttpContext.Current.User.Identity.Name } 
            };

        }

        SqlDataReader reader = qm.executeReader(cmdGoals, parameters);
        htmlStr.Text = "<div class='row'><div class='small-2 columns' style='padding: 6px 7px 0 0;'><img  height='120' width='120' src='images/SDGs/sdgsfirst.png' /></div>";
        try
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string goalId = (string)reader["GOAL_ID"].ToString();
                    htmlStr.Text += "<div class='small-2 columns' style='padding: 6px 7px 0 0;'><a href='target.aspx?goalId=" + goalId + "' ><img src='images/SDGs/SDGs" + (goalId.Length == 1 ? "0" : "") + goalId + ".jpg' /></a></div>";
                }
            }
            else htmlStr.Text += "<div class='small-8 columns' style='padding: 50px 5px' ><h3 >No goals enabled. Please contact the administrator!</h3></div>";

        }
        finally
        {
            qm.closeConnection();
        }
        htmlStr.Text += "</div>";
    }

    protected void cmdSignOut_ServerClick(object sender, System.EventArgs e)
    {
        FormsAuthentication.SignOut();
        FormsAuthentication.RedirectToLoginPage();
    }
}