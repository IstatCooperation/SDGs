using SDGsWA.Bean;
using System;
using System.Collections.Generic;
 
using System.Data.SqlClient;
 

public partial class AddType : System.Web.UI.Page
{
    bool editAction = false;
    string goalTypeID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");

          goalTypeID = Request.QueryString["tID"];
          editAction = String.Equals(Request.QueryString["ed"], "t") ;
        if (editAction) pageTitle.InnerText = "Edit Type";
        else pageTitle.InnerText = "Add new Type";
        if (!this.IsPostBack)
        {

            ClearData(null, null);
            if (editAction && !String.IsNullOrEmpty(goalTypeID))
            {
 
                QueryManager qm = new QueryManager();
                Dictionary<string, object> parameters = null;
                string cmdSelectType = "SELECT Type_ID, Label_En, Label_Ar, Descr_En, Descr_Ar, url_img, Descr_Short, Subindicator_Separator, order_code FROM Goal_Type where Type_ID=@goalTypeID";
                parameters = new Dictionary<string, object>
                     {
                        { "@goalTypeID",goalTypeID }
                     };
                SqlDataReader reader = qm.executeReader(cmdSelectType, parameters);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                    
                        this.Label_En.Value = (string)reader["LABEL_EN"].ToString();
                        this.Label_Ar.Value = (string)reader["LABEL_AR"].ToString();
                        this.Descr_En.Value = (string)reader["DESCR_EN"].ToString();
                        this.Descr_Ar.Value = (string)reader["DESCR_AR"].ToString();
                        this.url_img.Value = (string)reader["URL_IMG"].ToString();
                        this.Descr_Short.Value = (string)reader["DESCR_SHORT"].ToString();
                        this.Subindicator_Separator.Value = (string)reader["Subindicator_Separator"].ToString();
                        this.order_code.Value = (string)reader["order_code"].ToString();

                    }
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

    }


    protected void ClearData(object sender, EventArgs e)
    {
        
        this.Label_En.Value = "";
        this.Label_Ar.Value = "";
        this.Descr_En.Value = "";
        this.Descr_Ar.Value = "";
        this.url_img.Value = "";
        this.Descr_Short.Value = "";
        this.Subindicator_Separator.Value = "";
        this.order_code.Value = "";
        this.saveMessage.Text = "";
    }

    protected void saveType(object sender, EventArgs e)
    {

        IndicatorsManagement im = new IndicatorsManagement();
        try
        {
            if (!editAction)
            {
                GoalType gt = new GoalType(null, this.Label_En.Value, this.Label_Ar.Value, this.Descr_En.Value, this.Descr_Ar.Value, this.url_img.Value, this.Descr_Short.Value, this.Subindicator_Separator.Value, this.order_code.Value);
                im.addType(gt);
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
            
            }
            else
            {
                GoalType gt = new GoalType(goalTypeID, this.Label_En.Value, this.Label_Ar.Value, this.Descr_En.Value, this.Descr_Ar.Value, this.url_img.Value, this.Descr_Short.Value, this.Subindicator_Separator.Value, this.order_code.Value);
                im.editType(gt);
               saveMessage.Text = "<div class='db-alert db-success '>Data updated successfully</div>";
            
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            saveMessage.Text = "<div class='db-alert db-error '>Warning! Data not updated :" + ex.Message + " </div>";
        }

    }

}