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


public partial class Management_indicators_IndicatorList : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    private DataTable dtGrid;
    string goalID;
    string goalTypeID;
    string targetID;
    string selectCmd = "SELECT I.Indicator_Code, CM.Value as Indicator_Code_Value, " +
                "I.Indicator_descEn, I.Indicator_descAr, IC.Indicator_NL,I.IS_ACTIVE, T.Target_ID,T.Target_Code, T.Target_DescEn, " +
                "T.Target_DescAr FROM  Target T  " +
                    "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                    "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                    "INNER JOIN Code_Mapping CM ON(CM.CODE = I.Indicator_Code AND CM.GOAL_TYPE = @GOAL_TYPE) " +
                    "where  T.Target_ID=@Target_ID AND T.IS_ACTIVE=1 order by 1 asc";

    IndicatorsManagement im = new IndicatorsManagement();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");

        goalTypeID = Request.QueryString["gtID"];
        goalID = Request.QueryString["gID"];
        targetID = Request.QueryString["tID"];

        backLinkGT.InnerText = im.getGoalTypeDescr(goalTypeID);
        backLinkGT.HRef = ResolveUrl("~/Management/indicators/type/typeList.aspx ");
        backLinkG.HRef = ResolveUrl("~/Management/indicators/goal/GoalList.aspx?gtID=" + goalTypeID);
        backLinkG.InnerText = im.getGoalDescr(goalID);
        backLinkT.HRef = ResolveUrl("~/Management/indicators/target/TargetList.aspx?gtID=" + goalTypeID + "&gID=" + goalID);
        backLinkT.InnerText = Utility.StringCrop(im.getTargetDescr(targetID), 50);

        linkAddE.HRef = "AddEditIndicator.aspx?ed=f&ex=t&gtID=" + goalTypeID + "&gID=" + goalID + "&tID=" + targetID;
        linkAddN.HRef = "AddEditIndicator.aspx?ed=f&ex=f&gtID=" + goalTypeID + "&gID=" + goalID + "&tID=" + targetID;

        if (!this.IsPostBack)
        {
            refreshGridView();

        }
    }
    private DataTable GetData(string query, string goalTypeID, string targetID)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@GOAL_TYPE", goalTypeID);
                cmd.Parameters.AddWithValue("@Target_ID", targetID);
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

        tableGrid.DataSource = GetData(selectCmd, goalTypeID, targetID);
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
            string indicatorCode = linkEdit.CommandArgument;

            Boolean isActive = Boolean.Parse((e.Row.DataItem as DataRowView).Row["IS_ACTIVE"].ToString());
            LinkButton statusButton = e.Row.FindControl("statusButton") as LinkButton;
            if (isActive)
            {

                linkEdit.Attributes.Add("href", "AddEditIndicator.aspx?ed=t&ex=f&gtID=" + goalTypeID + "&gID=" + goalID + "&tID=" + targetID + "&ic=" + indicatorCode);


            Literal relatedCodes = e.Row.FindControl("relatedCodes") as Literal;
            relatedCodes.Text = im.getOtherCodeValuesIndicator(goalTypeID, goalID, targetID, indicatorCode);


            Label lSubIndicators = e.Row.FindControl("nSubIndicators") as Label;
            lSubIndicators.Text = im.GetNumberSubIndicators(goalTypeID, goalID, targetID, indicatorCode).ToString();

                statusButton.CssClass = "icon-minus-circled";
                statusButton.ToolTip = "DEACTIVE";
            }
            else
            {
                e.Row.CssClass = "disabled-text";

                e.Row.Cells[6].Text = "DISABLED";
                e.Row.Cells[6].CssClass = "text-center";
                e.Row.Cells[6].ColumnSpan =3;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;

                statusButton.CssClass = "icon-plus-circled";
                statusButton.ToolTip = "ACTIVE";
            }
        }

    }
    protected string getOtherCodeValuesIndicator(string indicatorCode)
    {

        return im.getOtherCodeValuesIndicator(goalTypeID, goalID, targetID, indicatorCode);
    }

    protected void setActive(object sender, EventArgs e)
    {

        try
        {
            string[] arg = new string[2];
            arg = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument.ToString().Split(';');
            String idRecord = arg[0];
            Boolean flagActive = Boolean.Parse(arg[1]);

            im.setActive("INDICATOR", "Indicator_Code", idRecord, !flagActive);
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
