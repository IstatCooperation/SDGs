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


public partial class Management_indicators_target_TargetList : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    private DataTable dtGrid;
    string goalID;
    string goalTypeID;
    string cmdSql = "SELECT  Target_ID, Target_DescEn, Target_DescAr, Target_Code, Goal_ID ,  IS_ACTIVE FROM Target where Goal_ID=@Goal_ID order by 1 asc";
    IndicatorsManagement im = new IndicatorsManagement();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");

        goalTypeID = Request.QueryString["gtID"];
        goalID = Request.QueryString["gID"];

        backLinkGT.InnerText = im.getGoalTypeDescr(goalTypeID);
        backLinkGT.HRef = ResolveUrl("~/Management/indicators/type/typeList.aspx ");
        backLinkG.HRef = ResolveUrl("~/Management/indicators/goal/GoalList.aspx?gtID=" + goalTypeID);
        backLinkG.InnerText = Utility.StringCrop(im.getGoalDescr(goalID), 50);

        linkAddEdit.HRef = "AddEditTarget.aspx?ed=f&gtID=" + goalTypeID + "&gID=" + goalID;

        if (!this.IsPostBack)
        {
            refreshGridView();

        }
    }



    private void refreshGridView()
    {

        tableGrid.DataSource = GetData(cmdSql, goalID);
        tableGrid.DataBind();
    }

    private DataTable GetData(string query, string objId)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@Goal_ID", objId);
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
        refreshGridView();
    }


    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            (e.Row.FindControl("lblRowNumber") as Label).Text = (e.Row.RowIndex + 1).ToString();
            LinkButton linkEdit = e.Row.FindControl("linkEdit") as LinkButton;

            string targetID = linkEdit.CommandArgument;
            Boolean isActive = Boolean.Parse((e.Row.DataItem as DataRowView).Row["IS_ACTIVE"].ToString());
            LinkButton statusButton = e.Row.FindControl("statusButton") as LinkButton;
            if (isActive)
            {

                linkEdit.Attributes.Add("href", "AddEditTarget.aspx?gtID=" + goalTypeID + "&ed=t&gID=" + goalID + "&tID=" + targetID);

                HyperLink lIndicators = e.Row.FindControl("nIndicators") as HyperLink;
                lIndicators.Text = im.GetNumberIndicators(goalTypeID, goalID, targetID).ToString();
                lIndicators.NavigateUrl = "../indicator/IndicatorList.aspx?gtID=" + goalTypeID + "&gID=" + goalID + "&tID=" + targetID;

                Label lSubIndicators = e.Row.FindControl("nSubIndicators") as Label;
                lSubIndicators.Text = im.GetNumberSubIndicators(goalTypeID, goalID, targetID, null).ToString();

                statusButton.CssClass = "icon-minus-circled";
                statusButton.ToolTip = "DEACTIVE";
            }
            else
            {
                e.Row.CssClass = "disabled-text";

                e.Row.Cells[4].Text = "DISABLED";
                e.Row.Cells[4].ColumnSpan = 3;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;

                statusButton.CssClass = "icon-plus-circled";
                statusButton.ToolTip = "ACTIVE";
            }
        }

    }


    protected void setActive(object sender, EventArgs e)
    {

        try
        {
            string[] arg = new string[2];
            arg = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument.ToString().Split(';');
            String idRecord = arg[0];
            Boolean flagActive = Boolean.Parse(arg[1]);

            im.setActive("Target", "Target_ID", idRecord, !flagActive);
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
