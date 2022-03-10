using System;
 
using System.Web;
using System.Web.Security;
 

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.IsAuthenticated)
        {
            this.userActions.Visible = false;
        }
         SWAGlobalProperties swgp = (SWAGlobalProperties)Application["SDGsWAGlobalProperties"];
        this.logoA.HRef = swgp.urlWebsite;
        this.logoImg.Src = ResolveUrl(swgp.urlLogo);
        this.logoImg.Attributes.Add("title", swgp.label);
        this.logoD1.InnerText= swgp.description;
        this.logoD2.InnerText = swgp.description2;
        this.WelcomeBackMessage.Text = "Welcome, " + HttpContext.Current.User.Identity.Name + "!";

        if (this.Page.User.IsInRole("Admin"))
        {
            this.LinkButtonUserRole.PostBackUrl = "management.aspx";
            this.LinkButtonUserRole.Text = "<i class='icon-cog' title='Management'></i>Management";
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
