using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string paramGoalId = Request.QueryString["goalId"];
        bool isAdmin = this.Page.User.IsInRole("Admin");
        UsersManager usersManager = new  UsersManager();
        string userID = usersManager.GetIDByUserName(HttpContext.Current.User.Identity.Name);
        if (String.IsNullOrEmpty(paramGoalId)|| !usersManager.checkAccessGoal(HttpContext.Current.User,paramGoalId) )
        {
            Response.Redirect("index.aspx");
            return;
        }
        int goalId = int.Parse(paramGoalId);


        QueryManager qm = new QueryManager();

        htmlStr.Text = "";
        title.Text = "";
        try
        {
            Dictionary<string, object> parameters;
            string targetQueryStr;

            if (isAdmin)
            {
                parameters = new Dictionary<string, object>
            {
                { "@goalID", goalId }
            };
               targetQueryStr = "SELECT  I.Indicator_Code, I.Indicator_descEn, I.Indicator_descAr, IC.Indicator_NL, " +
                    "T.Target_ID, T.Target_DescEn, T.Target_DescAr, " +
                    "G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr " +
                    "from GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                    "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                    "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                    "WHERE T.Target_ID IN(SELECT Target_ID FROM Target WHERE Goal_ID IN (@goalID)) ORDER BY T.Target_ID";
                /*"SELECT  I.Indicator_Code, I.Indicator_descEn, I.Indicator_descAr," +
                " T.Target_ID FROM Ind_Code T INNER JOIN Indicator I ON T.Indicator_Code = I.Indicator_Code" +
                " WHERE T.Target_ID IN (";*/
            }
            else
            {
                parameters = new Dictionary<string, object>
            {
                { "@goalID", goalId },
                { "@userID", userID }
            };
                targetQueryStr = "SELECT  I.Indicator_Code, I.Indicator_descEn, I.Indicator_descAr, IC.Indicator_NL, " +
                     "T.Target_ID, T.Target_DescEn, T.Target_DescAr, " +
                     "G.Goal_ID, G.Goal_DescEn, G.Goal_DescAr " +
                     "from GOAL G INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                     "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                     "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                     "INNER JOIN User_Indicator UI ON IC.Indicator_Code = UI.Ind_Code " +
                     "INNER JOIN Users U ON U.user_ID = UI.USER_ID " +
                     "WHERE U.User_ID=@userID AND T.Target_ID IN(SELECT Target_ID FROM Target WHERE Goal_ID IN (@goalID)) ORDER BY T.Target_ID";

            }
            SqlDataReader reader = qm.executeReader(targetQueryStr, parameters);
            try
            {
                Goal goal = new Goal(goalId);
                while (reader.Read())
                {
                    goal.setDescEn(Convert.ToString(reader["Goal_DescEn"]).Trim());
                    string targetId = Convert.ToString(reader["Target_ID"]).Trim();
                    Target target = goal.getTarget(targetId);
                    if (target == null)
                    {
                        target = goal.createTarget(targetId, Convert.ToString(reader["Target_DescEn"]).Trim());
                    }
                    target.createIndicator(Convert.ToString(reader["Indicator_Code"]).Trim(), Convert.ToString(reader["Indicator_NL"]).Trim(), Convert.ToString(reader["Indicator_descEn"]).Trim());
                }

                title.Text = goal.getId() + " - " + goal.getDescEn();

                foreach (Target target in goal.getTargets())
                {
                    System.Diagnostics.Debug.WriteLine("target desc " + target.getDescEn());

                    htmlStr.Text += "<details><summary><b>Target " + target.getId() + "</b> - " + target.getDescEn() + "</h3></summary><ul>";

                    foreach (Indicator ind in target.getIndicators())
                    {
                        htmlStr.Text += "<li><b><a href='indicator.aspx?targId=" + target.getId() + "&indId=" + ind.getCode() + "'>Indicator " + (String.IsNullOrEmpty(ind.getIndicatorNL()) ? "No code" : ind.getIndicatorNL()) + "</b> - " + (String.IsNullOrEmpty(ind.getDescEn()) ? "No description" : ind.getDescEn()) + "</a></li>";
                    }

                    htmlStr.Text += "</ul>";
                    htmlStr.Text += "</details>";
                }
            }
            finally
            {
                reader.Close();
                qm.closeConnection();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            htmlStr.Text = "Goal not selected.";
        }


    }
}