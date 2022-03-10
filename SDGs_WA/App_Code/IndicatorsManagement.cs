using SDGsWA.Bean;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class IndicatorsManagement
{
    public IndicatorsManagement()
    {

    }

    public Boolean addType(GoalType goalType)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Descr_Ar",goalType.Descr_Ar },
                { "@Descr_En",goalType.Descr_En },
                { "@Descr_Short",goalType.Descr_Short },
                { "@Label_Ar",goalType.Label_Ar },
                { "@Label_En",goalType.Label_En},
                { "@order_code",goalType.order_code },
                    { "@Subindicator_Separator",goalType.Subindicator_Separator },
                { "@url_img",goalType.url_img }

            };
        string insertCmd = "INSERT INTO Goal_Type (Label_En,Label_Ar,Descr_En,Descr_Ar,url_img,Descr_Short,Subindicator_Separator,order_code)     VALUES (@Label_En, @Label_Ar, @Descr_En, @Descr_Ar, @url_img, @Descr_Short, @Subindicator_Separator,@order_code);SELECT SCOPE_IDENTITY()";
        try
        {
            qm.executeScalar(insertCmd, parameters);

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

    public string GetDepartmentDescriptionByID(string depID)
    {
        string ret = "";
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@depID",depID }
            };
        string selectCmd = "SELECT  description  description FROM department WHERE dep_id=@depID";
        SqlDataReader reader = qm.executeReader(selectCmd, parameters);
        if (reader.Read())
        {
            ret = reader.GetString(0);
        }
        qm.closeConnection();
        return ret;
    }

    public Boolean removeTimePeriod(string timePeriod)
    {
        QueryManager qm = new QueryManager();

        Boolean ret = false;
        string removeInd = "DELETE FROM  TIME_PERIOD WHERE TIME_PERIOD=@timePeriod  ";
        Dictionary<string, object> parameters = new Dictionary<string, object> { { "@timePeriod", timePeriod } };
        try
        {

            qm.executeNonQuery(removeInd, parameters);

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

    public string getGoalDescr(string goalID)
    {
        string ret = "";
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Goal_ID",goalID }
            };
        string selectCmd = "SELECT Goal_Code, Goal_DescEn FROM Goal  WHERE Goal_ID = @Goal_ID";

        SqlDataReader reader = null;
        try
        {
            reader = qm.executeReader(selectCmd, parameters);

            while (reader.Read())
            {
                ret = reader["Goal_Code"].ToString() + " - " + reader["Goal_DescEn"].ToString();

            }
        }
        finally
        {
            reader.Close();
        }

        qm.closeConnection();
        return ret;
    }

    public Boolean addTimePeriod(string timePeriod)
    {
        QueryManager qm = new QueryManager();

        Boolean ret = false;
        string insertInd = "INSERT INTO TIME_PERIOD(TIME_PERIOD) VALUES (@timePeriod)";
        Dictionary<string, object> parameters = new Dictionary<string, object> { { "@timePeriod", timePeriod } };
        try
        {

            qm.executeNonQuery(insertInd, parameters);

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

    public SDGsWA.Bean.Target getTargetById(string targetID)
    {
        SDGsWA.Bean.Target ret = null;
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@target_ID",targetID }
            };
        string selectCmd = "SELECT Goal_ID ,Target_DescEn,Target_DescAr,Target_Code,Target_ID  FROM Target where target_ID = @target_ID";

        SqlDataReader reader = null;
        try
        {
            reader = qm.executeReader(selectCmd, parameters);

            while (reader.Read())
            {

                ret = new SDGsWA.Bean.Target(reader["Target_ID"].ToString(), reader["Target_Code"].ToString(), reader["Target_DescEn"].ToString(), reader["Target_DescAr"].ToString(), reader["Goal_ID"].ToString());
            }
        }
        finally
        {
            reader.Close();
        }

        qm.closeConnection();
        return ret;
    }

    public Boolean setActive(string tableName, string fieldID, string idRecord, Boolean flagActive)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                 { "@fieldID",idRecord},
                { "@flagActive",(flagActive)?1:0 }

            };
        string cmd = "UPDATE "+ tableName + " SET IS_ACTIVE = @flagActive  WHERE " + fieldID + " = @fieldID";
        try
        {
            qm.executeNonQuery(cmd, parameters);

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

    public object GetNumberIndicatorsByDep(string depID)
    {
        int ret = 0;
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@depID",depID }
            };
        string selectCmd = "SELECT count(*) as numb FROM dep_indicator WHERE DEP_ID  = @depID";
        ret = (Int32)qm.executeScalar(selectCmd, parameters);

        qm.closeConnection();
        return ret;
    }

    public Boolean saveDepartment(SDGsWA.Bean.Department bean)
    {

        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                 { "@Dep_ID",bean.Id},
                { "@Dep_Descr",bean.Descr }

            };
        string cmd = "UPDATE DEPARTMENT  SET description = @Dep_Descr WHERE dep_id = @Dep_ID";
        try
        {
            qm.executeNonQuery(cmd, parameters);

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

    public Boolean addDepIndicators(string depID, string[] indicators)
    {
        QueryManager qm = new QueryManager();
        SqlTransaction trans = null;
        Boolean ret = false;
        string insertInd = "INSERT INTO dep_indicator(DEP_ID, IND_CODE) VALUES (@depInd,@IndCode)";
        Dictionary<string, object> parameters = new Dictionary<string, object> { { "@depInd", depID } };
        try
        {
            trans = qm.getTransaction();
            foreach (string indicator in indicators)
            {
                parameters["@IndCode"] = indicator;
                qm.executeNonQuery(insertInd, trans, parameters);
            }
            trans.Commit();
            ret = true;
        }
        catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);

            throw ex;

        }
        finally
        {
            qm.closeConnection();

        }
        return ret;
    }

    public Boolean removeDepIndicators(string depID, string indicator)
    {
        QueryManager qm = new QueryManager();

        Boolean ret = false;
        string removeInd = "DELETE FROM  dep_indicator WHERE DEP_ID=@depInd AND  IND_CODE=@IndCode";
        Dictionary<string, object> parameters = new Dictionary<string, object> { { "@depInd", depID }, { "@IndCode", indicator } };
        try
        {

            qm.executeNonQuery(removeInd, parameters);

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
    public Boolean addDepartment(SDGsWA.Bean.Department bean)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
               { "@Dep_Descr",bean.Descr }

            };
        string insertCmd = "INSERT INTO DEPARTMENT (description)  VALUES (@Dep_Descr);SELECT SCOPE_IDENTITY()";
        try
        {
            qm.executeScalar(insertCmd, parameters);

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

    public string getTargetDescr(string targetID)
    {
        string ret = "";
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@target_ID",targetID }
            };
        string selectCmd = "SELECT  Target_Code, Target_DescEn   FROM Target where target_ID = @target_ID";

        SqlDataReader reader = null;
        try
        {
            reader = qm.executeReader(selectCmd, parameters);

            while (reader.Read())
            {
                ret = reader["Target_Code"].ToString() + " - " + reader["Target_DescEn"].ToString();

            }
        }
        finally
        {
            reader.Close();
        }

        qm.closeConnection();
        return ret;
    }

    public string getGoalTypeDescr(string goalTypeID)
    {
        string ret = "";
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Type_ID",goalTypeID }
            };
        string selectCmd = "SELECT Descr_Short FROM Goal_Type WHERE Type_ID = @Type_ID";

        SqlDataReader reader = null;
        try
        {
            reader = qm.executeReader(selectCmd, parameters);

            while (reader.Read())
            {
                ret = reader["Descr_Short"].ToString();

            }
        }
        finally
        {
            reader.Close();
        }

        qm.closeConnection();
        return ret;
    }

    public Boolean editTarget(SDGsWA.Bean.Target target)
    {

        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
              { "@Goal_ID",target.goalID },
                { "@Target_ID",target.id},
                  { "@Target_DescEn",target.descEn },
                { "@Target_DescAr",target.descAr },
                { "@Target_Code",target.code }

            };
        string insertCmd = "UPDATE TARGET  SET Goal_ID = @Goal_ID ,Target_DescEn=@Target_DescEn, Target_DescAr = @Target_DescAr ,Target_Code = @Target_Code  WHERE Target_ID = @Target_ID";
        try
        {
            qm.executeNonQuery(insertCmd, parameters);

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

    public Boolean addTarget(SDGsWA.Bean.Target target)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
              { "@Goal_ID",target.goalID },
               { "@Target_DescEn",target.descEn },
                { "@Target_DescAr",target.descAr },
                { "@Target_Code",target.code }

            };
        string insertCmd = "INSERT INTO TARGET (Target_Code ,Target_DescEn ,Target_DescAr ,Goal_ID) VALUES (@Target_Code, @Target_DescEn, @Target_DescAr, @Goal_ID);SELECT SCOPE_IDENTITY()";
        try
        {
            qm.executeNonQuery(insertCmd, parameters);

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

    public Boolean editGoal(SDGsWA.Bean.Goal goal)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
              { "@Goal_ID",goal.id },
                { "@Type_ID",goal.typeId},
                  { "@Goal_DescEn",goal.descEn },
                { "@Goal_DescAr",goal.descAr },
                { "@GoalImageEn",goal.imageEn },
                { "@GoalImageAr",goal.imageAr }

            };
        string insertCmd = "UPDATE Goal  SET Goal_DescEn = @Goal_DescEn ,Type_ID=@Type_ID, Goal_DescAr = @Goal_DescAr ,GoalImageEn = @GoalImageEn ,GoalImageAr = @GoalImageAr  WHERE Goal_ID = @Goal_ID";
        try
        {
            qm.executeNonQuery(insertCmd, parameters);

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

    public Boolean addGoal(SDGsWA.Bean.Goal goal)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {

                { "@Type_ID",goal.typeId},
                  { "@Goal_DescEn",goal.descEn },
                { "@Goal_DescAr",goal.descAr },
                { "@GoalImageEn",goal.imageEn },
                { "@GoalImageAr",goal.imageAr },
                   { "@Goal_Code",goal.code}

            };
        string insertCmd = "INSERT INTO Goal (Goal_DescEn ,Goal_DescAr ,GoalImageEn ,GoalImageAr ,Type_ID ,Goal_Code) VALUES (@Goal_DescEn, @Goal_DescAr, @GoalImageEn, @GoalImageAr, @Type_ID,@Goal_Code);SELECT SCOPE_IDENTITY()";
        try
        {
            qm.executeNonQuery(insertCmd, parameters);

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

    public Boolean editType(GoalType goalType)
    {
        QueryManager qm = new QueryManager();
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
              { "@Type_ID",goalType.Type_ID },
                { "@Descr_Ar",goalType.Descr_Ar },
                { "@Descr_En",goalType.Descr_En },
                { "@Descr_Short",goalType.Descr_Short },
                { "@Label_Ar",goalType.Label_Ar },
                { "@Label_En",goalType.Label_En},
                { "@order_code",goalType.order_code },
                    { "@Subindicator_Separator",goalType.Subindicator_Separator },
                { "@url_img",goalType.url_img }

            };
        string insertCmd = "UPDATE Goal_Type SET Label_En = @Label_En ,Label_Ar = @Label_Ar ,Descr_En = @Descr_En ,Descr_Ar = @Descr_Ar ,url_img = @url_img ,Descr_Short = @Descr_Short ,Subindicator_Separator = @Subindicator_Separator ,order_code = @order_code WHERE Type_ID = @Type_ID";
        try
        {
            qm.executeNonQuery(insertCmd, parameters);

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

    public Boolean addNewIndicator(IndicatorCode ic)
    {
        QueryManager qm = new QueryManager();
        SqlTransaction trans = null;
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {

                { "@Indicator_Code",ic.indicator.codeId},
                  { "@Indicator_descEn",ic.indicator.descEn },
                { "@Indicator_descAr",ic.indicator.descAr },
                { "@Code",ic.indicator.codeId },
                { "@Goal_Type",ic.indicator.goalType },
                   { "@Value",ic.indicator.codeValue},
                         { "@Indicator_NL",ic.indicatorNL },
                             { "@Target_ID",ic.target.id}

            };
        string insertInd = "INSERT INTO Indicator (Indicator_Code ,Indicator_descEn ,Indicator_descAr) VALUES (@Indicator_Code, @Indicator_descEn, @Indicator_descAr)";
        string insertCM = "INSERT INTO Code_Mapping (Code ,Goal_Type ,Value) VALUES (@Code, @Goal_Type, @Value)";
        string insertIC = "INSERT INTO Ind_Code (Indicator_NL ,Indicator_Code ,Target_ID) VALUES (@Indicator_NL, @Indicator_Code, @Target_ID)";

        try
        {
            trans = qm.getTransaction();
            qm.executeNonQuery(insertInd, trans, parameters);
            qm.executeNonQuery(insertCM, trans, parameters);
            qm.executeNonQuery(insertIC, trans, parameters);
            trans.Commit();
            ret = true;
        }
        catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);

            throw ex;

        }
        finally
        {
            qm.closeConnection();

        }
        return ret;
    }

    public Boolean editIndicatorCode(IndicatorCode ic)
    {
        QueryManager qm = new QueryManager();
        SqlTransaction trans = null;
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {

                { "@Indicator_Code",ic.indicator.codeId},
                  { "@Indicator_descEn",ic.indicator.descEn },
                { "@Indicator_descAr",ic.indicator.descAr },
                { "@Code",ic.indicator.codeId },
                { "@Goal_Type",ic.indicator.goalType },
                   { "@Value",ic.indicator.codeValue},
                         { "@Indicator_NL",ic.indicatorNL },
                             { "@Target_ID",ic.target.id}

            };
        string updateInd = "UPDATE Indicator SET  Indicator_descEn=@Indicator_descEn , Indicator_descAr=@Indicator_descAr  WHERE Indicator_Code=@Indicator_Code";
        string updateCM = "UPDATE Code_Mapping SET Value=@Value WHERE Code=@Code AND  Goal_Type=@Goal_Type";
        string updateIC = "UPDATE  Ind_Code SET Indicator_NL=@Indicator_NL  WHERE Indicator_Code= @Indicator_Code AND Target_ID=@Target_ID";

        try
        {
            trans = qm.getTransaction();
            qm.executeNonQuery(updateInd, trans, parameters);
            qm.executeNonQuery(updateCM, trans, parameters);
            qm.executeNonQuery(updateIC, trans, parameters);
            trans.Commit();
            ret = true;
        }
        catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);

            throw ex;

        }
        finally
        {
            qm.closeConnection();

        }
        return ret;
    }



    public Boolean addExistingIndicator(IndicatorCode ic, Boolean isSameGoalType)
    {
        QueryManager qm = new QueryManager();
        SqlTransaction trans = null;
        Boolean ret = false;
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {

                { "@Indicator_Code",ic.indicator.codeId},
                 { "@Code",ic.indicator.codeId },
                { "@Goal_Type",ic.indicator.goalType },
                { "@Value",ic.indicator.codeValue},
                { "@Indicator_NL",ic.indicatorNL },
                { "@Target_ID",ic.target.id}

            };



        try
        {
            trans = qm.getTransaction();
            if (!isSameGoalType)
            {
                string insertCM = "INSERT INTO Code_Mapping (Code ,Goal_Type ,Value) VALUES (@Code, @Goal_Type, @Value)";
                qm.executeNonQuery(insertCM, trans, parameters);
            }
            string insertIC = "INSERT INTO Ind_Code (Indicator_NL ,Indicator_Code ,Target_ID) VALUES (@Indicator_NL, @Indicator_Code, @Target_ID)";
            qm.executeNonQuery(insertIC, trans, parameters);
            trans.Commit();
            ret = true;
        }
        catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);

            throw ex;

        }
        finally
        {
            qm.closeConnection();

        }
        return ret;
    }

    public int GetNumberGoals(string typeID)
    {
        int ret = 0;
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Type_ID",typeID }
            };
        string selectCmd = "SELECT count(*) as numb FROM GOAl WHERE Type_ID = @Type_ID and IS_ACTIVE=1";
        ret = (Int32)qm.executeScalar(selectCmd, parameters);

        qm.closeConnection();
        return ret;
    }

    public int GetNumberTargets(string typeID, string goalID)
    {
        int ret = 0;
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Type_ID",typeID }
            };
        string selectCmd = "SELECT count(*)  FROM GOAl g, Target t WHERE t.Goal_ID=g.Goal_ID and g.Type_ID = @Type_ID  and t.IS_ACTIVE=1  and g.IS_ACTIVE=1";
        if (goalID != null)
        {
            parameters.Add("@Goal_ID", goalID);
            selectCmd += " and (@Goal_ID is null or g.Goal_ID=@Goal_ID)";
        }
        ret = (Int32)qm.executeScalar(selectCmd, parameters);

        qm.closeConnection();
        return ret;
    }


    public int GetNumberIndicators(string typeID, string goalID, string targetID)
    {
        int ret = 0;
        QueryManager qm = new QueryManager();
        string selectCmd = "SELECT count(I.Indicator_Code) FROM Goal_type gt  " +
                       "inner join GOAL G on g.Type_ID = gt.Type_ID  " +
                       "INNER JOIN Target T ON G.Goal_ID = T.Goal_ID " +
                       "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                       "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                       "INNER JOIN Code_Mapping CM ON(CM.CODE = I.Indicator_Code AND CM.GOAL_TYPE = gt.Type_ID) " +
                       "where gt.Type_ID = @Type_ID and G.IS_ACTIVE=1 and T.IS_ACTIVE=1 and I.IS_ACTIVE=1 ";
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Type_ID",typeID }
            };
        if (goalID != null)
        {
            parameters.Add("@Goal_ID", goalID);
            selectCmd += " and (@Goal_ID is null or g.Goal_ID=@Goal_ID)";
        }
        if (targetID != null)
        {
            parameters.Add("@Target_ID", targetID);
            selectCmd += " and (@Target_ID is null or T.Target_ID=@Target_ID)";
        }

        ret = (Int32)qm.executeScalar(selectCmd, parameters);

        qm.closeConnection();
        return ret;
    }


    public int GetNumberSubIndicators(string typeID, string goalID, string targetID, string indicatorCode)
    {
        int ret = 0;
        QueryManager qm = new QueryManager();
        string selectCmd = "SELECT count( distinct  s.Subindicator_Code)   " +
           "FROM Goal_type gt  " +
           "inner join GOAL G on g.Type_ID = gt.Type_ID  " +
           "INNER JOIN Target T ON G.Goal_ID = T.Goal_ID  " +
           "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID  " +
           "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code  " +
           "inner join Subindicator s on s.Indicator_Code = i.Indicator_Code  " +
           "INNER JOIN Code_Mapping CM ON(CM.CODE = I.Indicator_Code AND CM.GOAL_TYPE = gt.Type_ID)  " +
           "INNER JOIN Code_Mapping CM2 ON(CM2.CODE = s.Subindicator_Code AND CM.GOAL_TYPE = gt.Type_ID)  " +
           "where gt.Type_ID = @Type_ID and s.Is_Valid = 1 and G.IS_ACTIVE=1 and T.IS_ACTIVE=1 and I.IS_ACTIVE=1";

        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                  { "@Type_ID",typeID }
            };
        if (goalID != null)
        {
            parameters.Add("@Goal_ID", goalID);
            selectCmd += " and (@Goal_ID is null or g.Goal_ID=@Goal_ID)";
        }
        if (targetID != null)
        {
            parameters.Add("@Target_ID", targetID);
            selectCmd += " and (@Target_ID is null or T.Target_ID=@Target_ID)";
        }
        if (indicatorCode != null)
        {
            parameters.Add("@Indicator_Code", indicatorCode);
            selectCmd += " and (@Indicator_Code is null or I.Indicator_Code=@Indicator_Code)";
        }

        ret = (Int32)qm.executeScalar(selectCmd, parameters);

        qm.closeConnection();
        return ret;
    }

    public SDGsWA.Bean.Indicator getIndicatorByCodeAndType(string indicatorCodeSel, string goalType)
    {
        SDGsWA.Bean.Indicator ret = new SDGsWA.Bean.Indicator();
        QueryManager qm = new QueryManager();
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Indicator_Code",indicatorCodeSel },
               { "@GOAL_TYPE",goalType }
            };
        string selectCmd = "SELECT I.Indicator_Code, CM.Value as Indicator_Code_Value, " +
             "I.Indicator_descEn, I.Indicator_descAr  FROM Indicator I  " +
                 "INNER JOIN Code_Mapping CM ON(CM.CODE = I.Indicator_Code AND CM.GOAL_TYPE = @GOAL_TYPE) " +
                 "where  I.Indicator_Code=@Indicator_Code ";
        SqlDataReader reader = null;
        try
        {
            reader = qm.executeReader(selectCmd, parameters);

            while (reader.Read())
            {
                ret.codeValue = reader["Indicator_Code_Value"].ToString();
                ret.codeId = reader["Indicator_Code"].ToString();
                ret.descAr = reader["Indicator_descAr"].ToString();
                ret.descEn = reader["Indicator_descEn"].ToString();
            }
        }
        finally
        {
            reader.Close();
        }

        qm.closeConnection();
        return ret;
    }

    public Boolean deleteType(string typeID)
    {
        Boolean ret = false;
        QueryManager qm = new QueryManager();
        try
        {
            Dictionary<string, object> actionParam = new Dictionary<string, object>
            {
                { "@Typer_ID", typeID }
            };
            qm.executeNonQuery(
                "DELETE FROM Goal_Type WHERE" +
                " Typer_ID = @Typer_ID", actionParam);

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


    public string getOtherCodeValuesIndicator(string goalTypeID, string goalID, string targetID, string indicatorCode)
    {
        string selectCmd = "SELECT I.Indicator_Code, CM.Value as Code, " +
            "I.Indicator_descEn, I.Indicator_descAr, IC.Indicator_NL, T.Target_ID,T.Target_Code, T.Target_DescEn, gt.Descr_Short as GoalType,g.Goal_code,g.Goal_DescEn, " +
            "T.Target_DescAr FROM  Target T  " +
                "INNER JOIN Ind_Code IC ON T.Target_ID = IC.Target_ID " +
                "INNER JOIN Indicator I ON IC.Indicator_Code = I.Indicator_Code " +
                "INNER JOIN Code_Mapping CM ON CM.CODE = I.Indicator_Code  " +
                "inner join GOAL G on   G.Goal_ID = T.Goal_ID   " +
             "INNER JOIN Goal_type gt ON  g.Type_ID = gt.Type_ID  " +
                " where I.Indicator_Code =@CODE and IC.Target_ID <> @Target_ID AND CM.GOAL_TYPE = gt.Type_ID and gt.IS_ACTIVE=1 and G.IS_ACTIVE=1 and T.IS_ACTIVE=1 and I.IS_ACTIVE=1";
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@CODE", indicatorCode } ,
                { "@Target_ID",targetID }
            };
        string result = "<ul>";
        QueryManager qm = new QueryManager();
        SqlDataReader reader = null;
        try
        {

            //            reader = qm.executeReader("Select cm.Value as Code,gt.Descr_Short as GoalType from Code_Mapping cm ,Goal_Type gt " +
            //                               " WHERE gt.Type_ID = cm.Goal_Type and cm.code =@CODE and gt.Type_ID <> @GOAL_TYPE", parameters);

            reader = qm.executeReader(selectCmd, parameters);

            while (reader.Read())
            {
                String goaltype2 = Convert.ToString(reader["GoalType"]).Trim();
                String title = goaltype2 + "-" + Convert.ToString(reader["Goal_Code"]).Trim() + " " + Convert.ToString(reader["Goal_DescEn"]).Trim() + "-" + Convert.ToString(reader["Target_Code"]).Trim() + " " + Convert.ToString(reader["Target_DescEn"]).Trim();
                String text = goaltype2 + ": " + Convert.ToString(reader["Code"]).Trim();
                result += "<li><a   title='" + title + "'> " + text + " </a></li>";

            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
        }
        finally
        {
            reader.Close();
        }
        result += "</ul>";
        return result;
    }


    public string getAllCodeValuesIndicator(string indicatorCode)
    {
        string selectCmd = "SELECT CM.Value as Code,  gt.Descr_Short as GoalType  FROM  CODE_MAPPING CM" +
             " INNER JOIN Goal_type gt ON  CM.GOAL_TYPE = gt.Type_ID  " +
                " where CM.Code =@CODE";
        Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@CODE", indicatorCode }
            };
        string result = "<ul>";
        QueryManager qm = new QueryManager();
        SqlDataReader reader = null;
        try
        {
            reader = qm.executeReader(selectCmd, parameters);

            while (reader.Read())
            {
                String goaltype2 = Convert.ToString(reader["GoalType"]).Trim();
                String text = goaltype2 + ": " + Convert.ToString(reader["Code"]).Trim();
                result += "<li>" + text + "</li>";

            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception caught. " + ex.Message);
        }
        finally
        {
            reader.Close();
        }
        result += "</ul>";
        return result;
    }

}