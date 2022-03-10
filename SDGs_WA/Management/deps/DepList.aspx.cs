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

public partial class deps_depList : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    private string cmd = "SELECT  dep_id,description FROM department order by 1 asc";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
        {
            Response.Redirect("/index.aspx");
            return;
        }
        if (!this.IsPostBack)
        {

            saveMessage.Text = "";
            QueryManager qm = new QueryManager();
            tableGrid.DataSource = GetData(cmd);
            tableGrid.DataBind();


            if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
            }

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


    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        tableGrid.PageIndex = e.NewPageIndex;

        tableGrid.DataSource = GetData(cmd);
        tableGrid.DataBind();
    }

    protected void deleteUser_ServerClick(object sender, EventArgs e)
    {
        string userId = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;

    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        lblTotal.Text = "Total Rows: " + (tableGrid.DataSource as DataTable).Rows.Count;
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        IndicatorsManagement im = new IndicatorsManagement();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            (e.Row.FindControl("lblRowNumber") as Label).Text = (e.Row.RowIndex + 1).ToString();
            LinkButton linkEdit = e.Row.FindControl("linkEdit") as LinkButton;
            string depID = linkEdit.CommandArgument;
            linkEdit.Attributes.Add("href", "AddEditDep.aspx?ed=t&dID=" + depID);
            HyperLink hIndicators = e.Row.FindControl("nIndicators") as HyperLink;
            hIndicators.Text = im.GetNumberIndicatorsByDep(depID).ToString();
            hIndicators.Attributes.Add("href", "DepIndicators.aspx?dID=" + depID);
        }

    }
}