using System.Collections.Generic;

namespace MDT2PxWeb.Bean
{
    class Goal
    {
        public string code;
        public string desc;
        public string descEn;
        public List<Target> targets = new List<Target>();

        public string GetIndexDescription()
        {
            string c = "" + code;
            if (c.Length == 2)
            {
                return c;
            }
            return "0" + c;
        }
    }
}
