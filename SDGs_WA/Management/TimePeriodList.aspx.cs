using SDGsWA.Bean;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

public partial class TimePeriodList : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
  
    IndicatorsManagement im = new IndicatorsManagement();
    private string cmd = "select TIME_PERIOD from TIME_PERIOD order by 1 asc";
    

    protected void Page_Load(object sender, EventArgs e)
    {
        int min = 1900;
        int max = 2100;
        RangeValidatorYear.MinimumValue = min.ToString();
        RangeValidatorYear.MaximumValue = max.ToString();
        RangeValidatorYear.ErrorMessage = string.Format("Enter Value Between {0} and {1}", min, max);
        
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");


        if (!this.IsPostBack)
        {

            refreshGridView();

        }
    }

    private void refreshGridView()
    {
       
        tableGrid.DataSource = GetData(cmd);
        tableGrid.DataBind();
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
            LinkButton deleteButton = e.Row.FindControl("deleteButton") as LinkButton;
            string timep = deleteButton.CommandArgument;
          

        }

    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        tableGrid.PageIndex = e.NewPageIndex;
        tableGrid.DataSource = GetData(cmd);
        tableGrid.DataBind();
    }


    protected void removeTimePeriod(object sender, EventArgs e)
    {
        string tPeriod = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;

        try
        {
            im.removeTimePeriod(tPeriod);
            refreshGridView();
            saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            saveMessage.Text = "<div class='db-alert db-error '>Warning! Data not updated :" + ex.Message + " </div>";
        }
    }
  


    protected void addTimePeriod(object sender, EventArgs e)
    {
         
        string tperiod =this.time_period.Value;
 
        if (!String.IsNullOrEmpty(tperiod)) 
        {
            try
            {
                im.addTimePeriod(tperiod);
                refreshGridView();
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
                saveMessage.Text = "<div class='db-alert db-error '>Warning! Data not updated :" + ex.Message + " </div>";
            }
        }
        else
        {
            saveMessage.Text = "<div class='db-alert db-error '>Warning! No value selected! </div>";
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
     

}