using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        QueryManager qm = new QueryManager();
        SqlDataReader reader = qm.executeReader("SELECT * from GOAL");
        htmlStr.Text = "<div class='row'><div class='small-2 columns' style='padding: 6px 7px 0 0;'><img  height='120' width='120' src='/images/SDGs/sdgsfirst.png' /></div>";
        try
        {
            while (reader.Read())
            {
                string goalId = (string)reader["GOAL_ID"].ToString();
                htmlStr.Text += "<div class='small-2 columns' style='padding: 6px 7px 0 0;'><a href='target.aspx?goalId="+ goalId + "' ><img src='/images/SDGs/SDGs" + (goalId.Length == 1 ? "0" : "") + goalId + ".jpg' /></a></div>";
            }
        }
        finally
        {
            qm.closeConnection();
        }
        htmlStr.Text += "</div>";


    }
}