using System.Collections.Generic;

/// <summary>
/// Descrizione di riepilogo per Department
/// </summary>
public class Department
{
    private string id;
    private string descEn;
    private SortedDictionary<string, Indicator> indicators = new SortedDictionary<string, Indicator>(new SDGCodeSorter());

    public Department(string id, string descEn)
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
    public Indicator createIndicator(string code, string indicatorNL, string descEn, string targetID, string goalID, string goalCode, string goalDescr, string goalType, string goalTypeLabel, string codeValue)
    {
        if (indicators.ContainsKey(indicatorNL))
        {
            return indicators[indicatorNL];
        }

        Indicator ind = new Indicator(code, indicatorNL, descEn, targetID, goalID, goalCode, goalDescr, goalType, goalTypeLabel, codeValue);
        indicators.Add(indicatorNL, ind);
        return ind;
    }

}