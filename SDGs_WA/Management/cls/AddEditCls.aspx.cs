using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class Cls_AddEditCls : System.Web.UI.Page
{

    bool editAction = false;
    string clsName;

    IndicatorsManagement im = new IndicatorsManagement();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");

        clsName = Request.QueryString["name"];

        editAction = String.Equals(Request.QueryString["ed"], "t");
        if (editAction) pageTitle.InnerText = "Edit Classification";
        else pageTitle.InnerText = "Add new Classification";



    }
   

    protected void ClearData(object sender, EventArgs e)
{

 

}

protected void saveCmd(object sender, EventArgs e)
{

    

}

}