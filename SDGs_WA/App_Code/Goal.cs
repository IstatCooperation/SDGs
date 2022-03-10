using System.Collections.Generic;

public class Goal
{
    private int id;
    private string code;
    private string descEn;
    private SortedDictionary<string, Target> targets = new SortedDictionary<string, Target>(new SDGCodeSorter());

    public Goal(int id, string descEn, string code)
    {
        this.id = id;
        this.code = code;
        this.descEn = descEn;
    }


    public Goal(int id, string descEn)
    {
        this.id = id;
        this.descEn = descEn;
    }

    public Goal(int id)
    {
        this.id = id;
    }

    public int getId()
    {
        return id;
    }

    public string getDescEn()
    {
        return descEn;
    }


    public void setDescEn(string descEn)
    {
        this.descEn = descEn;
    }

    public void setCode(string code)
    {
        this.code = code;
    }

    public string getCode()
    {
        return code;
    }


    public SortedDictionary<string, Target>.ValueCollection getTargets()
    {
        return targets.Values;
    }

    public Target createTarget(string id, string descEn, string code)
    {
        if (targets.ContainsKey(code))
        {
            return targets[code];
        }

        Target target = new Target(id, descEn, code);
        targets.Add(code, target);
        return target;
    }

    public Target getTarget(string code)
    {
        if (targets.ContainsKey(code))
        {
            return targets[code];
        }

        return null;
    }

}
