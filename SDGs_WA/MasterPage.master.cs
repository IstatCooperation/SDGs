using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.IsAuthenticated)
        {
            this.userActions.Visible = false;
        }

        this.WelcomeBackMessage.Text = "Welcome, " + HttpContext.Current.User.Identity.Name + "!";
        if (this.Page.User.IsInRole("Admin"))
        {
            this.LinkButtonUserRole.PostBackUrl = "Users/usersList.aspx";
            this.LinkButtonUserRole.Text = "<i class='icon-users' title='Users Management'></i>Users Management";
        }
        else
        {
            this.LinkButtonUserRole.PostBackUrl = "Users/changePassword.aspx";
            this.LinkButtonUserRole.Text = "<i class='icon-vcard' title='Change Password'></i>Change Password";

        }
    }

    protected void cmdSignOut_ServerClick(object sender, System.EventArgs e)
    {
        FormsAuthentication.SignOut();
        FormsAuthentication.RedirectToLoginPage();
    }
}
