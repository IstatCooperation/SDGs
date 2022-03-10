using System.Collections.Generic;

namespace MDT2PxWeb.Bean
{
    public class Target
    {
        public string id;
        public string code;
        public string desc;
        public string descEn;
        public List<Indicator> indicators = new List<Indicator>();

        public string GetIndexDescription()
        {
            return id.Replace(".", "_");
        }
    }
}