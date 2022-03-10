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


public partial class Management_indicators_goal_GoalList : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    private DataTable dtGrid;
    string goalTypeID;
    IndicatorsManagement im = new IndicatorsManagement();
    string cmdSql = "SELECT  [Goal_ID] ,[Goal_DescEn] ,[Goal_DescAr],[GoalImageEn],[GoalImageAr],[Type_ID],[Goal_Code], [IS_ACTIVE] FROM [Goal] where Type_ID=@Type_ID  order by 1 asc";
    protected void Page_Load(object sender, EventArgs e)
    {
        goalTypeID = Request.QueryString["gtID"];
        backLinkGT.InnerText = im.getGoalTypeDescr(goalTypeID);
        backLinkGT.HRef = ResolveUrl("~/Management/indicators/type/typeList.aspx ");
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");


        linkAddEdit.HRef = "AddEditGoal.aspx?ed=f&gtID=" + goalTypeID;

        if (!this.IsPostBack)
        {
            refreshGridView();
        }
    }
    private DataTable GetData(string query, string goalTypeID)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@Type_ID", goalTypeID);
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

    private void refreshGridView()
    {

        tableGrid.DataSource = GetData(cmdSql, goalTypeID);
        tableGrid.DataBind();
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
            string goalID = linkEdit.CommandArgument;
            Boolean isActive = Boolean.Parse((e.Row.DataItem as DataRowView).Row["IS_ACTIVE"].ToString());
            LinkButton statusButton = e.Row.FindControl("statusButton") as LinkButton;
            if (isActive)
            {

                linkEdit.Attributes.Add("href", "AddEditGoal.aspx?gtID=" + goalTypeID + "&ed=t&gID=" + goalID);
                HyperLink lTargets = e.Row.FindControl("nTargets") as HyperLink;
                lTargets.Text = im.GetNumberTargets(goalTypeID, goalID).ToString();
                lTargets.NavigateUrl = "../target/TargetList.aspx?gtID=" + goalTypeID + "&gID=" + goalID;

                Label lIndicators = e.Row.FindControl("nIndicators") as Label;
                lIndicators.Text = im.GetNumberIndicators(goalTypeID, goalID, null).ToString();
                Label lSubIndicators = e.Row.FindControl("nSubIndicators") as Label;
                lSubIndicators.Text = im.GetNumberSubIndicators(goalTypeID, goalID, null, null).ToString();

                statusButton.CssClass = "icon-minus-circled";
                statusButton.ToolTip = "DEACTIVE";
            }
            else
            {
                e.Row.CssClass = "disabled-text";

                e.Row.Cells[6].Text = "DISABLED";
                e.Row.Cells[6].ColumnSpan = 4;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;

                e.Row.Cells[9].Visible = false;

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

            im.setActive("Goal", "Goal_ID", idRecord, !flagActive);
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
