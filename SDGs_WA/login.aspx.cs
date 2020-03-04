using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;
using System.Configuration;

public partial class Login : System.Web.UI.Page
{
    private const string errorText = "Username / password incorrect. Please try again.";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.cmdLogin.ServerClick += new System.EventHandler(this.CmdLogin_ServerClick);
    }





    private void CmdLogin_ServerClick(Object sender, EventArgs e)
    {
        UsersManager um = new UsersManager();
        HttpCookie cookie = um.doLogin(txtUserName.Value, txtUserPass.Value);

        if (cookie != null)
        {

            Response.Cookies.Add(cookie);

            // Redirect to requested URL, or homepage if no previous page
            // requested
            string returnUrl = Request.QueryString["ReturnUrl"];
            if (returnUrl == null) returnUrl = "/";

            Response.Redirect(returnUrl);
        }
        else
        {
            ErrorLabel.Text = errorText;
            ErrorLabel.Visible = true;
        }


    }


}