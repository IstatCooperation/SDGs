
public class Indicator
{
    private string code;
    private string codeValue;
    private string indicatorNL;
    private string descEn;
    private string targetID;
    private string goalID;
    private string goalCode;
    private string goalType;
    private string goalTypeLabel;
    private string goalDescEn;

    public string CodeValue
    {
        get
        {
            return codeValue;
        }

        set
        {
            codeValue = value;
        }
    }

    public string GoalType
    {
        get
        {
            return goalType;
        }

        set
        {
            goalType = value;
        }
    }

    public Indicator(string code, string indicatorNL, string descEn)
    {
        this.code = code;
        this.indicatorNL = indicatorNL;
        this.descEn = descEn;
    }
    public Indicator(string code, string indicatorNL, string descEn, string targetID, string goalID, string goalCode, string goalDescEn, string goalType, string goalTypeLabel, string codeValue)
    {
        this.code = code;
        this.indicatorNL = indicatorNL;
        this.descEn = descEn;
        this.targetID = targetID;
        this.goalID = goalID;
        this.goalCode = goalCode;
        this.goalType = goalType;
        this.goalTypeLabel = goalTypeLabel;
        this.goalDescEn = goalDescEn;
        this.CodeValue = codeValue;
    }

    public string getCode()
    {
        return code;
    }

    public string getIndicatorNL()
    {
        return indicatorNL;
    }

    public string getDescEn()
    {
        return descEn;
    }
    public string getTargetID()
    {
        return targetID;
    }
    public string getGoalID()
    {
        return goalID;
    }

    public string getGoalCode()
    {
        return goalCode;
    }
    public string getGoalDescEn()
    {
        return goalDescEn;
    }

    public string getGoalTypeLabel()
    {
        return goalTypeLabel;
    }


    public string getGoalType()
    {
        return goalType;
    }
}

