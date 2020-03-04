using System.Collections.Generic;

namespace MDT2PxWeb.Bean
{
    public class Dimension
    {
        public string name;
        public string label;
        public string labelEn;
        public Table table;
        public bool isTime = false;
        public Dictionary<string, string> values = new Dictionary<string, string>();
        public Dictionary<string, string> valuesEn = new Dictionary<string, string>();

        public string GetValuePool()
        {
            return "VP" + name;
        }

        public string GetValueSet(SubIndicator subIndicator)
        {
            return "VS" + name + "_" + subIndicator.code;
        }

        public string GetDescription()
        {
            return "{name:" + name + ",label:" + label + ",isTime:" + isTime + ",table:" + table.GetDescription() + "}";
        }

        public string GetDescriptionEN()
        {
            return "{name:" + name + ",labelEn:" + labelEn + ",isTime:" + isTime + ",table:" + table.GetDescriptionEn() + "}";
        }
    }
}
