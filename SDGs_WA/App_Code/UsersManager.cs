using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

/// <summary>
/// Descrizione di riepilogo per UsersManager
/// </summary>
public class UsersManager
{
    public UsersManager()
    {

    }


    public HttpCookie doLogin(string username, string password)
    {
        HttpCookie ret = null;
        // Initialize FormsAuthentication, for what it's worth
        FormsAuthentication.Initialize();

        // Create our connection and command objects
        string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        SqlConnection conn = null;
        SqlDataReader reader = null;
        try
        {
            conn = new SqlConnection(constr);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT r.Role_ID [RoleId],r.Role_Name [RoleName]  FROM Roles r, Users u WHERE u.username=@username " +
             "AND u.password=@password AND r.Role_ID=u.Role_ID";

            // Fill our parameters
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5"); // Or "sha1"

            // Execute the command
            conn.Open();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // Create a new ticket used for authentication
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                   1, // Ticket version
                   username, // Username associated with ticket
                   DateTime.Now, // Date/time issued
                   DateTime.Now.AddMinutes(30), // Date/time to expire
                   true, // "true" for a persistent user cookie
                   reader.GetString(1), // User-data, in this case the roles
                   FormsAuthentication.FormsCookiePath);// Path cookie valid for

                // Encrypt the cookie using the machine key for secure transport
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                   FormsAuthentication.FormsCookieName, // Name of auth cookie
                   hash); // Hashed ticket

                // Set the cookie's expiration time to the tickets expiration time
                if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

                // Add the cookie to the list for outgoing response
                ret = cookie;

            }

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
        }
        finally

        {
            if (reader != null) reader.Close();
            if (conn != null) conn.Close();
        }

        return ret;
    }

    public bool checkAccessIndicator(IPrincipal user, string indId)
    {
        if (user.IsInRole("Admin")) return true;
        bool ret = false;
        string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        SqlConnection conn =
         new SqlConnection(constr);
        SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM User_INDICATOR ui,Users u WHERE  ui.User_ID=u.User_ID  AND ui.IND_CODE=@indId AND u.username=@username";

        // Fill our parameters
        cmd.Parameters.Add("@indId", SqlDbType.VarChar).Value = indId;
        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = user.Identity.Name;
        // Execute the command
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        try
        {

            if (reader.Read())
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
        }
        finally

        {
            if (reader != null) reader.Close();
            if (conn != null) conn.Close();
        }

        return ret;
    }

    public bool IsAdmin(string userId)
    {
        bool ret = false;
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId",userId }
            };
        string selectUser = "SELECT * FROM Roles r, Users u WHERE u.user_Id=@userId   AND r.Role_ID=u.Role_ID AND r.ROLE_NAME='Admin'";

        SqlDataReader reader = qm.executeReader(selectUser, parameters);
        if (reader.HasRows)
        {
            ret = true;
        }
        qm.closeConnection();
        return ret;
    }

    public string GetIDByUserName(string userName)
    {
        string ret = "";
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@username",userName }
            };
        string selectUser = "SELECT  User_ID FROM USERS WHERE username IN (@username)";
        SqlDataReader reader = qm.executeReader(selectUser, parameters);
        if (reader.Read())
        {
            ret = reader["User_ID"].ToString();
        }
        qm.closeConnection();
        return ret;
    }

    public Boolean ChangePassword(string userId, string newpassword)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@User_ID",userId },
                { "@password",FormsAuthentication.HashPasswordForStoringInConfigFile(newpassword, "md5") }
            };
        string insertUser = "UPDATE USERS SET PASSWORD=@password WHERE User_ID IN (@User_ID);";
        try
        {
            qm.executeNonQuery(insertUser, parameters);
            ret = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);

            throw ex;

        }
        finally
        {
            qm.closeConnection();

        }

        return ret;
    }

    public string GetUserNameByID(string userId)
    {
        string ret = "";
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId",userId }
            };
        string selectUser = "SELECT  USERNAME [usernme] FROM USERS WHERE USER_ID=@userId";
        SqlDataReader reader = qm.executeReader(selectUser, parameters);
        if (reader.Read())
        {
            ret = reader.GetString(0);
        }
        qm.closeConnection();
        return ret;
    }

    public string GetRoleIDByUserID(string userId)
    {
        string ret = "";
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId",userId }
            };
        string selectUser = "SELECT  ROLE_ID [roleId] FROM USERS WHERE USER_ID=@userId";
        SqlDataReader reader = qm.executeReader(selectUser, parameters);
        if (reader.Read())
        {
            ret =Convert.ToString( reader.GetInt32(0));
        }
        qm.closeConnection();
        return ret;
    }

    public Boolean AddUser(string user, string pass, string roleId, string roleName)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@username",user },
                { "@password",FormsAuthentication.HashPasswordForStoringInConfigFile(pass, "md5") },
                { "@roleID",Int32.Parse(roleId) }
            };
        string insertUser = "INSERT INTO USERS (USERNAME,PASSWORD,ROLE_ID) VALUES(@username,@password,@roleID);SELECT SCOPE_IDENTITY()";
        try
        {
            int newID = qm.executeScalar(insertUser, parameters);
            if (roleName.Equals("Admin")) {
                SqlDataReader reader = qm.executeReader("SELECT distinct indicator_code from indicator");
                String insertUsersIndCmd = "INSERT INTO User_Indicator(USER_ID,IND_CODE) VALUES ";
                while (reader.Read())
                {
                    insertUsersIndCmd += "(" + newID + ",'" + Convert.ToString(reader["indicator_code"]).Trim() + "'),";
                  
                }
                if (reader != null) reader.Close();
                qm.executeNonQuery(insertUsersIndCmd.Substring(0, insertUsersIndCmd.Length - 1));
              
            }
            ret = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);

            throw ex;

        }
        finally
        {
            qm.closeConnection();

        }
        return ret;

    }

   

    public Boolean deleteUser(string userId)
    {
        Boolean ret = false;
        QueryManager qm = new QueryManager();
        try
        {
            Dictionary<string, object> actionParam = new Dictionary<string, object>
            {
                { "@User_ID", userId }
            };
            qm.executeNonQuery(
                "DELETE FROM User_INDICATOR WHERE" +
                " User_ID = @User_ID", actionParam);
            qm.executeNonQuery(
                "DELETE FROM Users WHERE" +
                " User_ID = @User_ID", actionParam);
            ret = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
        }
        finally
        {
            qm.closeConnection();
        }
        return ret;
    }

    public bool checkAccessGoal(IPrincipal user, string goalID)
    {
        if (user.IsInRole("Admin")) return true;
        bool ret = false;
        string constr = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        SqlConnection conn =
         new SqlConnection(constr);
        SqlCommand cmd = conn.CreateCommand();
       // cmd.CommandText = "SELECT *  FROM User_Goal ug,Users u WHERE  ug.User_ID=u.User_ID  AND ug.Goal_ID=@goalID AND u.username=@username";
        cmd.CommandText = "SELECT g.* from GOAL g, Target t, Ind_code ic,User_indicator ui,Users u " +
                " where  g.Goal_ID=t.Goal_ID AND g.Goal_ID=@goalID and t.target_id=ic.target_ID and ui.ind_code=ic.indicator_code and ui.User_ID=u.User_Id and u.username=@username";
        // Fill our parameters
        cmd.Parameters.Add("@goalID", SqlDbType.Int).Value = goalID;
        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = user.Identity.Name;
        // Execute the command
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        try
        {

            if (reader.Read())
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
        }
        finally

        {
            if (reader != null) reader.Close();
            if (conn != null) conn.Close();
        }

        return ret;
    }
}