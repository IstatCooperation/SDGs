using SDGsWA.Bean;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

public partial class ClassificationsList : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
  
    IndicatorsManagement im = new IndicatorsManagement();
    private string cmd = "SELECT  SEQUENCE, NAME, LABEL, TABLE_NAME, CODE, DESCRIPTION, IS_TIME, INT_CODE, LABEL_ENG, DESCRIPTION_ENG FROM DIMENSION order by 1 asc";
    

    protected void Page_Load(object sender, EventArgs e)
    {
             
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
            LinkButton linkEdit = e.Row.FindControl("linkEdit") as LinkButton;
            string name = linkEdit.CommandArgument;
            linkEdit.Attributes.Add("href", "AddEditCls.aspx?ed=t&clsname=" + name);
          }

    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        tableGrid.PageIndex = e.NewPageIndex;
        tableGrid.DataSource = GetData(cmd);
        tableGrid.DataBind();
    }


    protected void removeCls(object sender, EventArgs e)
    {
         
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