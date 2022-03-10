using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string typeId = Request.QueryString["typeId"];
        bool isAdmin = this.Page.User.IsInRole("Admin");
        UsersManager usersManager = new UsersManager();
        string userID = usersManager.GetIDByUserName(HttpContext.Current.User.Identity.Name);


        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = null;

        htmlStr.Text = "";
        itemsMenu.Text = "";


        string labelDescr = "";
        String urlImg = "";

        string cmdGoalType;
        if (User.IsInRole("Admin"))
        {
            cmdGoalType = "SELECT TYPE_ID,LABEL_EN,DESCR_EN,URL_IMG,DESCR_SHORT from GOAL_TYPE WHERE IS_ACTIVE=1 order by ORDER_CODE asc";
        }
        else
        {
            cmdGoalType = "SELECT distinct gt.* from GOAL_TYPE gt, GOAL g, Target t, Ind_code ic,User_indicator ui, Indicator I,Users u where  gt.type_id = g.Type_ID and g.Goal_ID=t.Goal_ID and t.target_id=ic.target_ID and ui.ind_code=ic.indicator_code and i.indicator_code=ic.indicator_code and ui.User_ID=u.User_Id and u.username=@username and gt.IS_ACTIVE=1 and g.IS_ACTIVE=1 and i.IS_ACTIVE=1 and t.IS_ACTIVE=1 order by gt.TYPE_ID asc";
        }
        parameters = new Dictionary<string, object>
                     {
                        { "@username", HttpContext.Current.User.Identity.Name }
                     };
        SqlDataReader reader = qm.executeReader(cmdGoalType, parameters);

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                string typeIdR = (string)reader["TYPE_ID"].ToString();
                string typeDescrENR = (string)reader["DESCR_EN"].ToString(); 
                string labelDescrENR = (string)reader["LABEL_EN"].ToString();
                string urlImgENR = (string)reader["URL_IMG"].ToString();
                string descrShort = (string)reader["DESCR_SHORT"].ToString();
                itemsMenu.Text += " <li id = 'type-" + typeIdR + "' class='menu-item'><a href = '"+ResolveUrl("~/index.aspx?typeId=" + typeIdR) + "' > " + descrShort + " </a ></li>";
                if (String.IsNullOrEmpty(typeId) || typeIdR.Equals(typeId))
                {
                    typeId = typeIdR;
                    typeDescr.Text = typeDescrENR;
                    urlImg = urlImgENR;
                    labelDescr = labelDescrENR;
                    HttpContext.Current.Session["GOAL_TYPE"] = typeId;
                    HttpContext.Current.Session["GOAL_LABEL"] = labelDescr;
                }
            }
        }

        if (!String.IsNullOrEmpty(typeId) )
            {
                parameters = new Dictionary<string, object>
                     {
                        { "@typeID", typeId },
                        { "@username", HttpContext.Current.User.Identity.Name }
                     };

        string cmdGoals;
        if (User.IsInRole("Admin"))
        {
            cmdGoals = "SELECT * from GOAL g WHERE g.TYPE_ID=@typeID and g.IS_ACTIVE=1";
        }
        else
        {
            cmdGoals = "SELECT distinct g.* from GOAL g, Target t, Ind_code ic,User_indicator ui,Users u, Indicator i " +
                 " where g.TYPE_ID=@typeID AND g.Goal_ID=t.Goal_ID and t.target_id=ic.target_ID and ui.ind_code=ic.indicator_code and i.indicator_code=ic.indicator_code and ui.User_ID=u.User_Id and u.username=@username and  g.IS_ACTIVE=1 and i.IS_ACTIVE=1 and t.IS_ACTIVE=1";
        }

        string goalCss = "";
        if (  Int32.Parse(typeId) > 1)
        {
            goalCss = "monitoringGoals";
        }

        htmlStr.Text = "<div class='row'>";
        if (!String.IsNullOrWhiteSpace(urlImg))
        {
            htmlStr.Text += "<div class='small-2 columns text-center " + goalCss + "' style='padding: 6px 7px 0 0; float: left !important;'><img height='120' width='120' src='" + urlImg + "' alt='" + labelDescr + "' /></div>";
        }

        if (reader != null)
        {
            reader.Close();
        }

        reader = qm.executeReader(cmdGoals, parameters);
        try
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string goalId = (string)reader["GOAL_ID"].ToString();
                    string goalImg = (string)reader["GOALImageEn"].ToString();
                    string goaldescr = (string)reader["GOAL_DescEn"].ToString();
                    htmlStr.Text += "<div class='small-2 columns text-center " + goalCss + "' style='padding: 6px 7px 0 0; float: left !important;'><a href='target.aspx?goalId=" + goalId + "' ><img src='" + goalImg + "' alt='" + goaldescr + "' style='max-width: 120px;' />";
                    if (Int32.Parse(typeId) > 1)
                    {
                        htmlStr.Text += "<div style='text-transform: uppercase;'>" + goaldescr + "</div>";
                    }
                    htmlStr.Text += "</a></div>";
                }
            }
            else
            {
                htmlStr.Text += "<div class='small-8 columns text-center' style='padding: 50px 5px'><h3>The current user is not authorized to any data</h3></div>";
            }
        }
        finally
        {
            qm.closeConnection();
        }
        htmlStr.Text += "</div>";
        }
        else
        {
            htmlStr.Text += "<div class='small-8 columns text-center' style='padding: 50px 5px'><h3>The current user is not authorized to any data</h3></div>";
        }
    }

    protected void cmdSignOut_ServerClick(object sender, System.EventArgs e)
    {
        FormsAuthentication.SignOut();
        FormsAuthentication.RedirectToLoginPage();
    }
}