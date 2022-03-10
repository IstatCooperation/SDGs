using SDGsWA.Bean;
using System;
using System.Collections.Generic;
 
using System.Data.SqlClient;
 

public partial class AdEditDep : System.Web.UI.Page
{
    bool editAction = false;
    string depID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.IsInRole("Admin"))
            Response.Redirect("~/index.aspx");

        depID = Request.QueryString["dID"];
          editAction = String.Equals(Request.QueryString["ed"], "t") ;
        if (editAction) pageTitle.InnerText = "Edit Departement";
        else pageTitle.InnerText = "Add new Departement";
        if (!this.IsPostBack)
        {

            ClearData(null, null);
            if (editAction && !String.IsNullOrEmpty(depID))
            {
 
                QueryManager qm = new QueryManager();
                Dictionary<string, object> parameters = null;
                string cmdSelectType = " SELECT  dep_id,description FROM department where dep_id=@depID";
                parameters = new Dictionary<string, object>
                     {
                        { "@depID",depID }
                     };
                SqlDataReader reader = qm.executeReader(cmdSelectType, parameters);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                    
                        this.Label_ID.Text = (string)reader["dep_id"].ToString();
                        this.dep_description.Value = (string)reader["description"].ToString();
                       

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
        
       
        this.dep_description.Value = "";
        this.saveMessage.Text = "";
    }

    protected Boolean isEditAction()
    {
        return editAction;
    }
    protected void saveType(object sender, EventArgs e)
    {

        IndicatorsManagement im = new IndicatorsManagement();
        try
        {
            if (!editAction)
            {
                SDGsWA.Bean.Department bean = new SDGsWA.Bean.Department(null, this.dep_description.Value);
                im.addDepartment(bean);
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
            
            }
            else
            {
                SDGsWA.Bean.Department bean = new SDGsWA.Bean.Department(depID, this.dep_description.Value);
                im.saveDepartment(bean);
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