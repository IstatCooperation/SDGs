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

public partial class Management_typeList : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    IndicatorsManagement im = new IndicatorsManagement();
    private string cmd = "SELECT Type_ID,Label_En, Label_Ar, Descr_En, Descr_Ar, url_img, Descr_Short, Subindicator_Separator, order_code, IS_ACTIVE FROM Goal_Type order by Type_ID asc";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Page.User.IsInRole("Admin"))
            {
                saveMessage.Text = "";
                 
                refreshGridView();
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
    private void refreshGridView()
    {

        tableGrid.DataSource = GetData(cmd);
        tableGrid.DataBind();
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
        tableGrid.DataBind();
    }
 

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
   
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            (e.Row.FindControl("lblRowNumber") as Label).Text = (e.Row.RowIndex + 1).ToString();
            Boolean isActive=  Boolean.Parse ((e.Row.DataItem as DataRowView).Row["IS_ACTIVE"].ToString())  ;
            LinkButton statusButton = e.Row.FindControl("statusButton") as LinkButton;

            LinkButton linkFamilly = e.Row.FindControl("linkFamilly") as LinkButton;
            string typeID = linkFamilly.CommandArgument;
            linkFamilly.Attributes.Add("href", "addType.aspx?ed=t&tID=" + typeID);
           
            if (isActive)
            {
                HyperLink lGoals = e.Row.FindControl("nGoals") as HyperLink;
                lGoals.Text = im.GetNumberGoals(typeID).ToString();
                lGoals.NavigateUrl = "../goal/GoalList.aspx?gtID=" + typeID;
                Label lTargets = e.Row.FindControl("nTargets") as Label;
                lTargets.Text = im.GetNumberTargets(typeID, null).ToString();
                Label lIndicators = e.Row.FindControl("nIndicators") as Label;
                lIndicators.Text = im.GetNumberIndicators(typeID, null, null).ToString();
                Label lSubIndicators = e.Row.FindControl("nSubIndicators") as Label;
                lSubIndicators.Text = im.GetNumberSubIndicators(typeID, null, null, null).ToString();
       
                statusButton.CssClass = "icon-minus-circled";
                statusButton.ToolTip = "DEACTIVE";
            }
            else {
                e.Row.CssClass = "disabled-text";

                e.Row.Cells[8].Text = "DISABLED";
                e.Row.Cells[8].ColumnSpan = 6;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = true;

                statusButton.CssClass = "icon-plus-circled";
                statusButton.ToolTip = "ACTIVE";
            }

        }

    }


    protected void setActive( object sender, EventArgs e)
    {
        
            try
        {
            string[] arg = new string[2];
            arg = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument.ToString().Split(';');
            String idRecord =  arg[0];
            Boolean flagActive= Boolean.Parse(arg[1]);

            im.setActive("Goal_Type","Type_ID",idRecord, !flagActive);
                refreshGridView();
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
                saveMessage.Text = "<div class='db-alert db-error '>Warning! Data not updated :" + ex.Message + " </div>";
            }
         
    }
}