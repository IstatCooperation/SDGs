using System;
using System.Collections.Generic;
using System.Data.SqlClient;


public partial class Users_Departments : System.Web.UI.Page
{
    private SortedDictionary<string, Department> departments = new SortedDictionary<string, Department>(new SDGCodeSorter());
    string userId;
    Boolean isAdmin = false;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.Page.User.IsInRole("Admin"))
        {
            Response.Redirect("/index.aspx");
            return;
        }
        userId = Request.QueryString["uID"];
        if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(userId))
        {
            Response.Redirect("/index.aspx");
            return;
        }
        UsersManager um = new UsersManager();
        this.Username.Text = um.GetUserNameByID(userId);
        isAdmin = um.IsAdmin(userId);
        if (isAdmin)
        {
            Response.Redirect("/Users/usersList.aspx");
            return;
        }
        cmdSave.Visible = !isAdmin;
        if (!IsPostBack)
        {
            loadIndicators();
        }

    }

    public void loadIndicators()
    {
        List<string> userIndicators = new List<string>();
        QueryManager qm = new QueryManager();
        SqlDataReader readerInd = null;
        SqlDataReader reader = null;
        this.htmlStr.Text = "";

        try
        {
            string depsQueryStr = "select dp.DEP_ID ,dp.description DEPARTMENT, ic.Indicator_NL, ind.Indicator_Code,ind.Indicator_descEn,ic.target_ID,g.goal_ID,g.Goal_descEn" +
                " from department dp,dep_indicator di,Ind_Code ic,Indicator ind,Target t, Goal g " +
                "where dp.dep_id = di.DEP_ID and ic.Indicator_code = di.Ind_code and ind.Indicator_Code = ic.Indicator_Code and ic.target_ID=t.Target_ID and t.Goal_ID=g.goal_ID " +
                " order by 1 asc";

            reader = qm.executeReader(depsQueryStr);
            while (reader.Read())
            {
                String depId = Convert.ToString(reader["DEP_ID"]).Trim();
                Department department = getDepartment(depId);
                if (department == null)
                {
                    department = new Department(Convert.ToString(reader["DEP_ID"]).Trim(), Convert.ToString(reader["DEPARTMENT"]).Trim());
                    departments.Add(depId, department);
                }
                department.createIndicator(Convert.ToString(reader["Indicator_Code"]).Trim(), Convert.ToString(reader["Indicator_NL"]).Trim(), Convert.ToString(reader["Indicator_descEn"]).Trim(), Convert.ToString(reader["target_ID"]).Trim(), Convert.ToString(reader["goal_ID"]).Trim(), Convert.ToString(reader["goal_DescEn"]).Trim());
            }

            if (reader != null) reader.Close();

            string userIndicatorStr = "select IND_CODE from User_Indicator  where  User_ID=@userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId }
            };

            readerInd = qm.executeReader(userIndicatorStr, parameters);

            while (readerInd.Read())
            {
                userIndicators.Add(Convert.ToString(readerInd["IND_CODE"]).Trim());
            }

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            htmlStr.Text = "";
        }
        finally
        {
            if (reader != null) reader.Close();
            if (readerInd != null) readerInd.Close();
            qm.closeConnection();
        }



        foreach (Department department in departments.Values)
        {
            htmlStr.Text += "<details><summary><b>" + department.getDescEn() + " Department</b> </h3></summary> <a href='javascript: void(0);'><label><input type='checkbox' onclick='selectDep("+department.getId()+ ");' class='indicator-checkbox' value=\"\" id='checkbox-dep-" + department.getId() + "'><span id='label-select-all'>Select All</span></label></a> <ul>";

            foreach (Indicator ind in department.getIndicators())
            {
                htmlStr.Text += "<li ><a  href='javascript: void(0);' ><label><input type='checkbox' class='indicator-checkbox indicator-dep-" + department.getId() + "'   value=" + ind.getCode() + " runat='server'  name ='indicators'  " + (userIndicators.Contains(ind.getCode()) ? "Checked='Checked' " : "") + "  " + (isAdmin ? "disabled='disabled' " : "") + "  >" +
                    " Goal " + ind.getGoalID() + ": Indicator " + (String.IsNullOrEmpty(ind.getIndicatorNL()) ? "No code" : ind.getIndicatorNL()) + " - " + (String.IsNullOrEmpty(ind.getDescEn()) ? "No description" : ind.getDescEn()) + "</label></a></li>";
            }

            htmlStr.Text += "</ul>";
            htmlStr.Text += "</details>";
        }

    }

    protected void saveIndicators(object sender, EventArgs e)
    {

        int dbCheck = 0;
        string indicatorsRequest = Request["indicators"];
        string[] indicators = new string[] {};
        if (!String.IsNullOrEmpty(indicatorsRequest) )  indicators = RemoveDuplicates(indicatorsRequest.Split(','));
        QueryManager qm = new QueryManager();        
        SqlTransaction trans = qm.getTransaction();
        try
        {
            qm.executeNonQueryTrans("Delete from User_Indicator where User_ID=" + userId, trans);
            //qm.executeNonQuery("Delete from User_Indicator where User_ID=" + userId);
            dbCheck = 1;
            if (indicators.Length > 0)
            {
                String insertUsersIndCmd = "INSERT INTO User_Indicator(USER_ID,IND_CODE) VALUES ";
                foreach (string indicator in indicators)
                {
                    insertUsersIndCmd += "(" + userId + ",'" + indicator + "'),";
                }
                //qm.executeNonQuery(insertUsersIndCmd.Substring(0, insertUsersIndCmd.Length - 1));
                qm.executeNonQueryTrans(insertUsersIndCmd.Substring(0, insertUsersIndCmd.Length - 1), trans);
                dbCheck = 1;
            }
            trans.Commit();
        }
        catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
            dbCheck = 2;
        }
        finally
        {
            qm.closeConnection();
        }

        switch (dbCheck)
        {
            case 0:
                saveMessage.Text = "";
                break;
            case 1:
                saveMessage.Text = "<div class='db-alert db-success hideMe'>Data updated successfully</div>";
                break;
            case 2:
                saveMessage.Text = "<div class='db-alert db-error hideMe'>Warning! Data not updated.</div>";
                break;
        }
        loadIndicators();
    }

    public static string[] RemoveDuplicates(string[] s)
    {
        HashSet<string> set = new HashSet<string>(s);
        string[] result = new string[set.Count];
        set.CopyTo(result);
        return result;
    }


    public Department getDepartment(string id)
    {
        if (departments.ContainsKey(id))
        {
            return departments[id];
        }
        return null;
    }

}