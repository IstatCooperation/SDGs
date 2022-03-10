using System.Collections.Generic;

namespace SDGsWA.Bean
{
    public class Goal
    {
        public string id;
        public string typeId;
        public string code;
        public string descEn;
        public string descAr;
        public string imageEn;
        public string imageAr;
        public List<Target> targets = new List<Target>();

        public Goal(string id, string typeId, string code, string descEn, string descAr, string imageEn, string imageAr)
        {
            this.id = id;
            this.typeId = typeId;
            this.code = code;
            this.descAr = descAr;
            this.descEn = descEn;
            this.imageEn = imageEn;
            this.imageAr = imageAr;
        }

    }
}
