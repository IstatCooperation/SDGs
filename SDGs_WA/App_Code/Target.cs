using System.Collections.Generic;

public class Target
{

    private string id;
    private string descEn;
    private SortedDictionary<string, Indicator> indicators = new SortedDictionary<string, Indicator>(new SDGCodeSorter());

    public Target(string id, string descEn)
    {
        this.id = id;
        this.descEn = descEn;
    }

    public string getId()
    {
        return id;
    }

    public string getDescEn()
    {
        return descEn;
    }

    public SortedDictionary<string, Indicator>.ValueCollection getIndicators()
    {
        return indicators.Values;
    }

    public Indicator createIndicator(string code, string indicatorNL, string descEn)
    {
        if (indicators.ContainsKey(indicatorNL))
        {
            return indicators[indicatorNL];
        }

        Indicator ind = new Indicator(code, indicatorNL, descEn);
        indicators.Add(indicatorNL, ind);
        return ind;
    }

}

