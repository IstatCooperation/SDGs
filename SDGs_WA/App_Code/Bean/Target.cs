using System.Collections.Generic;

namespace SDGsWA.Bean
{
    public class Target
    {
        public string id;
        public string goalID;
        public string code;
        public string descAr;
        public string descEn;
        private SortedDictionary<string, IndicatorCode> indicators = new SortedDictionary<string, IndicatorCode>(new SDGCodeSorter());

        public Target(string id, string code, string descEn, string descAr, string goalID)
        {
            this.id = id;
            this.code = code;
            this.descAr = descAr;
            this.descEn = descEn;
            this.goalID = goalID;
        }
        public Target(string id)
        {
            this.id = id;
        }

        public SortedDictionary<string, IndicatorCode>.ValueCollection getIndicatorsCode()
        {
            return indicators.Values;
        }
        public IndicatorCode createIndicatorCode(string targetID, string indicatorCodeID, string indicatorCodeValue, string indicatorNL, string indicatorDescEn, string indicatorDescAr, string goalTypeID)
        {
            if (indicators.ContainsKey(indicatorNL))
            {
                return indicators[indicatorNL];
            }

            IndicatorCode ind = new IndicatorCode(targetID, indicatorCodeID, indicatorCodeValue, indicatorNL, indicatorDescEn, indicatorDescAr, goalTypeID);
            indicators.Add(indicatorNL, ind);
            return ind;
        }
    }
}