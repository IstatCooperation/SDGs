namespace SDGsWA.Bean
{
    public class IndicatorCode
    {
        public Target target;
        public string indicatorNL;
        public Indicator indicator;


        public IndicatorCode(Target target, string indicatorNL, Indicator indicator)
        {
            this.target = target;
            this.indicatorNL = indicatorNL;
            this.indicator = indicator;
        }

        public IndicatorCode(string targetId, string indicatorNL, Indicator indicator)
        {
            this.target = new Target(targetId);
            this.indicatorNL = indicatorNL;
            this.indicator = indicator;
        }

        public IndicatorCode()
        {

        }

        public IndicatorCode(string targetID, string indicatorCodeID, string indicatorCodeValue, string indicatorNL, string indicatorDescEn, string indicatorDescAr, string goalTypeID)
        {

            this.target = new Target(targetID);
            this.indicatorNL = indicatorNL;
            this.indicator = new Indicator(indicatorCodeID, indicatorCodeValue, indicatorDescEn, indicatorDescAr, goalTypeID);
        }
    }
}