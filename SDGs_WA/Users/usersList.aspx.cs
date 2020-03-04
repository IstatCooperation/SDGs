using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class userList : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Page.User.IsInRole("Admin"))
            {
                saveMessage.Text = "";
                QueryManager qm = new QueryManager();
                gvUsers.DataSource = GetData("SELECT User_ID, username, Role_ID FROM Users ORDER BY User_ID");
                gvUsers.DataBind();
                pnlAssignRoles.Visible = true;

                if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
                {
                    saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
                }
            }
            else
                Response.Redirect("~/index.aspx");
        }
    }

    private DataTable GetData(string query)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlRoles = (e.Row.FindControl("ddlRoles") as DropDownList);
            ddlRoles.DataSource = GetData("SELECT Role_ID, Role_Name FROM Roles");
            ddlRoles.DataTextField = "Role_Name";
            ddlRoles.DataValueField = "Role_ID";
            ddlRoles.DataBind();

            string assignedRole = (e.Row.DataItem as DataRowView)["Role_ID"].ToString();
            ddlRoles.Items.FindByValue(assignedRole).Selected = true;
            string rolename = ddlRoles.Items.FindByValue(assignedRole).Text;
            LinkButton linkindicators = e.Row.FindControl("linkindicators") as LinkButton;
            string userId = linkindicators.CommandArgument;
            linkindicators.Attributes.Add("href", "Departments.aspx?uID="+ userId);
            
            if (rolename.Equals("Admin"))
            {
                linkindicators.Attributes.Add("href", "#");
                linkindicators.Attributes.Add("disabled", "disabled");
                linkindicators.CssClass = "icon-block mouse-default disabled";
                linkindicators.OnClientClick = null;
            }


            try
            {
                String userName = (e.Row.DataItem as DataRowView)["Username"].ToString();
                LinkButton deleteUserButton = e.Row.FindControl("deleteUserButton") as LinkButton;
                if (userName.Equals(this.User.Identity.Name))
                {
                    deleteUserButton.Enabled = false;
                    deleteUserButton.CssClass = "icon-block mouse-default disabled";
                    deleteUserButton.OnClientClick = null;


                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            }
        }
    }


    protected void UpdateRole(object sender, EventArgs e)
    {
        int dbCheck = 0;
        GridViewRow row = ((sender as LinkButton).NamingContainer as GridViewRow);
        int userId = int.Parse((sender as LinkButton).CommandArgument);
        int roleId = int.Parse((row.FindControl("ddlRoles") as DropDownList).SelectedItem.Value);
        string roleName = (row.FindControl("ddlRoles") as DropDownList).SelectedItem.Text;
        QueryManager qm = null;
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Users SET Role_ID = @RoleId WHERE User_ID = @UserId"))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@RoleId", roleId);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    dbCheck = 1;
                }
            }

            if (roleName.Equals("Admin"))
            {
                qm = new QueryManager();
                SqlDataReader reader = qm.executeReader("SELECT distinct indicator_code from indicator");
                String insertUsersIndCmd = "INSERT INTO User_Indicator(USER_ID,IND_CODE) VALUES ";
                while (reader.Read())
                {
                    insertUsersIndCmd += "(" + userId + ",'" + Convert.ToString(reader["indicator_code"]).Trim() + "'),";

                }
                if (reader != null) reader.Close();
                qm.executeNonQuery(insertUsersIndCmd.Substring(0, insertUsersIndCmd.Length - 1));

            }

        }
        catch (Exception ex)
        {
            dbCheck = 2;
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
        }
        finally
        {
            if (qm != null) qm.closeConnection();
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


    protected void deleteUser_ServerClick(object sender, EventArgs e)
    {
        string userId = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;
        UsersManager um = new UsersManager();
        if (um.deleteUser(userId))
            refreshUsersList(new QueryManager(), 1);
        else
            refreshUsersList(new QueryManager(), 2);
    }

    private void refreshUsersList(QueryManager qm, int dbCheck)
    {
        try
        {

            gvUsers.DataSource = GetData("SELECT User_ID, username, Role_ID FROM Users ORDER BY User_ID");
            gvUsers.DataBind();
        }
        catch
        {
            dbCheck = 2;
        }
        finally
        {
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
            qm.closeConnection();
        }
    }
}