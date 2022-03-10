using System.Collections.Generic;

namespace MDT2PxWeb.Bean
{
    public class Indicator
    {
        public string code;
        public string codeValue;
        public string desc;
        public string descEn;
        public List<SubIndicator> subIndicators = new List<SubIndicator>();

        public string GetIndexDescription()
        {
            return codeValue;
        }
    }
}