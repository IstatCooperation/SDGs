using System.Collections.Generic;

namespace SDGsWA.Bean
{
    public class Indicator
    {
        public string codeId;

        public string codeValue;

        public string descEn;
        public string descAr;
        public string goalType;
        public List<SubIndicator> subIndicators = new List<SubIndicator>();

        public Indicator(string codeId, string codeValue, string descEn, string descAr, string goalType)
        {
            this.codeId = codeId;

            this.codeValue = codeValue;
            this.descEn = descEn;
            this.descAr = descAr;
            this.goalType = goalType;
        }

        public string GetIndexDescription()
        {
            return codeValue;
        }


        public Indicator()
        {

        }
    }
}