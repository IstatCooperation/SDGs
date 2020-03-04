using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Users_AddUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {           
            if (!this.Page.User.IsInRole("Admin"))
                Response.Redirect("~/index.aspx");

            QueryManager qm = new QueryManager();
            this.ddlRoles.DataSource = qm.GetData("SELECT Role_ID, Role_Name FROM Roles");
            this.ddlRoles.DataTextField = "Role_Name";
            this.ddlRoles.DataValueField = "Role_ID";
            this.ddlRoles.DataBind();
            this.ddlRoles.SelectedIndex = 1;
        }
        
    }


    protected void ClearData(object sender, EventArgs e)
    {
        this.txtUser.Value = "";
        this.txtUserPass.Value = "";
        this.txtUserPass1.Value = "";
        this.saveMessage.Text = ""; 
    }

    protected void AddUser(object sender, EventArgs e)
    {

        UsersManager um = new UsersManager();
        try
        {
            um.AddUser(txtUser.Value, txtUserPass.Value, ddlRoles.Items[ddlRoles.SelectedIndex].Value, ddlRoles.Items[ddlRoles.SelectedIndex].Text);
            //   saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
            Response.Redirect("~/Users/usersList.aspx?msg=true");

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            saveMessage.Text = "<div class='db-alert db-error hideMe'>Warning! Data not updated</div>";
        }
 
    }

}