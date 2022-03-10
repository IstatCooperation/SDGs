using SDGsWA.Bean;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

public partial class DepIndicators : System.Web.UI.Page
{
    private static string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public string openDetails = "";
    private List<string> depIndicatorsList = new List<string>();

    string depID;
    IndicatorsManagement im = new IndicatorsManagement();
    private string cmd = "select * from dep_indicator di inner join indicator i on i.indicator_code=di.ind_code WHERE di.DEP_ID = @depId";
    Dictionary<string, String> parametersDepID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");

        depID = Request.QueryString["dID"];
        parametersDepID = new Dictionary<string, String> { { "@depId", depID } };

        if (!this.IsPostBack)
        {
            this.Departement.Text = im.GetDepartmentDescriptionByID(depID);
      refreshGridView();
              loadDdl(ddlGoalType, "SELECT Type_ID ID,  Descr_Short VALUE FROM Goal_Type order by Type_ID asc ", null);

        }
    }

    private void refreshGridView()
    {
        depIndicatorsList.Clear();
        tableGrid.DataSource = GetData(cmd, parametersDepID);
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
            string indCode = deleteButton.CommandArgument;
            depIndicatorsList.Add(indCode);
            (e.Row.FindControl("relatedCodes") as Literal).Text = im.getAllCodeValuesIndicator(indCode);
           
        }

    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        tableGrid.PageIndex = e.NewPageIndex;
        tableGrid.DataSource = GetData(cmd, parametersDepID);
        tableGrid.DataBind();
    }


    protected void removeIndicator(object sender, EventArgs e)
    {
        string indicator = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;
       
        try
        {
            im.removeDepIndicators(depID, indicator);
            refreshGridView();
            saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            saveMessage.Text = "<div class='db-alert db-error '>Warning! Data not updated :" + ex.Message + " </div>";
        }
    }
    public void loadIndicators(string goalTypeSelected, string targedID)
    {
        QueryManager qm = new QueryManager();

        SqlDataReader reader = null;
        this.htmlStr.Text = "";

        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Target_ID",targedID},
                 { "@GOAL_TYPE",goalTypeSelected }
            };


        string selectCmd = "SELECT I.Indicator_Code ID, IC.Indicator_NL +' - '+ CM.Value +' '+  I.Indicator_descEn as VALUE  FROM  Target T  " +
                "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                "INNER JOIN Code_Mapping CM ON(CM.CODE = I.Indicator_Code AND CM.GOAL_TYPE = @GOAL_TYPE) " +
                "where  T.Target_ID=@Target_ID order by 1 asc";


        htmlStr.Text += " <ul style='font-size: 0.8rem;line-height: 1;font-style: italic;margin-bottom:0;padding:2px;'>";

        reader = qm.executeReader(selectCmd, parameters);
        while (reader.Read())
        {
            String indCodI = Convert.ToString(reader["ID"]).Trim();
            if (!depIndicatorsList.Contains(indCodI))
            {
                htmlStr.Text += "<li ><a  href='javascript: void(0);' ><label><input type='checkbox' class='indicator-checkbox indicator-dep-" + indCodI + "'   value=" + indCodI + " runat='server'  name ='indicators' >" +
                   Convert.ToString(reader["VALUE"]).Trim() + "</label></a></li>";
            }


        }
        htmlStr.Text += "</ul>";

    }





    protected void saveIndicators(object sender, EventArgs e)
    {

        string indicatorsRequest = Request["indicators"];
        string[] indicators = new string[] { };
        if (!String.IsNullOrEmpty(indicatorsRequest)) indicators = RemoveDuplicates(indicatorsRequest.Split(','));
        if (indicators.Length > 0)
        {
            try
            {
                im.addDepIndicators(depID, indicators);
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
            saveMessage.Text = "<div class='db-alert db-error '>Warning! No Indicators selected! </div>";
        }

    }

    public static string[] RemoveDuplicates(string[] s)
    {
        HashSet<string> set = new HashSet<string>(s);
        string[] result = new string[set.Count];
        set.CopyTo(result);
        return result;
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

    private DataTable GetData(string query, Dictionary<string, String> parameters)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {

                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }


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

    protected void ddlGoalType_Selection_Change(Object sender, EventArgs e)
    {
        Dictionary<string, String> parameters = new Dictionary<string, String>
            {
                { "@Type_ID",ddlGoalType.SelectedItem.Value }
            };
        ddlGoal.Items.Clear();
        ddlTarget.Items.Clear();
        cmdSave.Visible = false;
        loadDdl(ddlGoal, "SELECT Goal_ID ID, Goal_Code +' - '+  Goal_DescEn VALUE FROM Goal  where Type_ID=@Type_ID  order by 1 asc  ", parameters);
        openDetails = "open='true'";
    }

    protected void ddlGoal_Selection_Change(Object sender, EventArgs e)
    {
        Dictionary<string, String> parameters = new Dictionary<string, String>
            {
                { "@Goal_ID",ddlGoal.SelectedItem.Value }
            };

        ddlTarget.Items.Clear();
        cmdSave.Visible = false;
        loadDdl(ddlTarget, "SELECT  Target_ID ID,Target_Code+' - '+ Target_DescEn VALUE   FROM Target where Goal_ID=@Goal_ID order by 1 asc ", parameters);
        openDetails = "open='true'";
    }

    protected void ddlTarget_Selection_Change(Object sender, EventArgs e)
    {
        Dictionary<string, String> parameters = new Dictionary<string, String>
            {
                { "@Target_ID",ddlTarget.SelectedItem.Value },
                 { "@GOAL_TYPE",ddlGoalType.SelectedItem.Value }
            };

        loadIndicators(ddlGoalType.SelectedItem.Value, ddlTarget.SelectedItem.Value);
        cmdSave.Visible = true;
        openDetails = "open='true'";
    }



    private void loadDdl(DropDownList ddl, string query, Dictionary<string, string> parameters)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, string> item in parameters)
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    ddl.DataSource = dt;
                    ddl.DataBind();
                    ddl.DataTextField = "VALUE";
                    ddl.DataValueField = "ID";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("Select ...", String.Empty));
                    ddl.SelectedIndex = 0;

                }
            }
        }

    }


}