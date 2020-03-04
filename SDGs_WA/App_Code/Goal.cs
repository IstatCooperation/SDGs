using System.Collections.Generic;

public class Goal
{
    private int id;
    private string descEn;
    private SortedDictionary<string, Target> targets = new SortedDictionary<string, Target>(new SDGCodeSorter());

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

    public SortedDictionary<string, Target>.ValueCollection getTargets()
    {
        return targets.Values;
    }

    public Target createTarget(string id, string descEn)
    {
        if (targets.ContainsKey(id))
        {
            return targets[id];
        }

        Target target = new Target(id, descEn);
        targets.Add(id, target);
        return target;
    }

    public Target getTarget(string id)
    {
        if (targets.ContainsKey(id))
        {
            return targets[id];
        }

        return null;
    }

}
