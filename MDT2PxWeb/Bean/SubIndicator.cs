using System.Collections.Generic;

namespace MDT2PxWeb.Bean
{
    public class SubIndicator
    {
        public string code;
        public string desc;
        public string descEn;
        public List<Dimension> dimensions = new List<Dimension>();

        public string GetMainTableName(Target target)
        {
            return "SD" + target.GetIndexDescription() + "_" + code;
        }
    }
}