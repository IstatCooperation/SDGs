
public class Indicator
{
    private string code;
    private string indicatorNL;
    private string descEn;
    private string targetID;
    private string goalID;
    private string goalDescEn;

    public Indicator(string code, string indicatorNL, string descEn)
    {
        this.code = code;
        this.indicatorNL = indicatorNL;
        this.descEn = descEn;
    }
    public Indicator(string code, string indicatorNL, string descEn, string targetID, string goalID, string goalDescEn)
    {
        this.code = code;
        this.indicatorNL = indicatorNL;
        this.descEn = descEn;
        this.targetID = targetID;
        this.goalID = goalID;
        this.goalDescEn = goalDescEn;
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
    public string getGoalDescEn()
    {
        return goalDescEn;
    }

}

