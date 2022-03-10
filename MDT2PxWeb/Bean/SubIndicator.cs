using System.Collections.Generic;

namespace MDT2PxWeb.Bean
{
    public class SubIndicator
    {

        public string code;
        public string codeValue;
        public string desc;
        public string descEn;
        public List<Dimension> dimensions = new List<Dimension>();

        public string GetMainTableName(Target target)
        {
            return "S" + target.GetIndexDescription().Replace(" ", "").Replace("_", "") + code.Replace(" ", "").Replace("_", "");
            // return "SD" + code.Replace(" ", "").Replace("_", "");
        }
    }
}