using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Users_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UsersManager um = new UsersManager();
        if (!this.Page.User.IsInRole("Admin"))
        {
            this.txtUser.Text = HttpContext.Current.User.Identity.Name;
            this.txtUserId.Value = um.GetIDByUserName(txtUser.Text);
            this.linkUsers.Visible = false;
           

        }
        else
        {
            if (Request.QueryString["uID"] != null && Request.QueryString["uID"] != "")
            {
                string userid = Request.QueryString["uID"];
                this.txtUserId.Value = userid;
                this.txtUser.Text = um.GetUserNameByID(this.txtUserId.Value);
               
            }
            else
            {
                this.txtUser.Text = HttpContext.Current.User.Identity.Name;
                this.txtUserId.Value = um.GetIDByUserName(txtUser.Text);
                           }
          
        }
        
    }

    protected void ClearData(object sender, EventArgs e)
    {

        this.txtUserPass.Value = "";
        this.txtUserPass1.Value = "";
        this.saveMessage.Text = "";
    }

    protected void ChangePassword(object sender, EventArgs e)
    {

        UsersManager um = new UsersManager();
        try
        {
            um.ChangePassword(this.txtUserId.Value, this.txtUserPass.Value);
            saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            saveMessage.Text = "<div class='db-alert db-error hideMe'>Warning! Data not updated </div>";
        }
    }
    
}