namespace SDGsWA.Bean
{
    public class GoalType
    {
        public string Type_ID;
        public string Label_En;
        public string Label_Ar;
        public string Descr_En;
        public string Descr_Ar;
        public string url_img;
        public string Descr_Short;
        public string Subindicator_Separator;
        public string order_code;

        public GoalType()
        {

        }

        public GoalType(string type_ID, string label_En, string label_Ar, string descr_En, string descr_Ar, string url_img, string descr_Short, string subindicator_Separator, string order_code)
        {
            Type_ID = type_ID;
            Label_En = label_En;
            Label_Ar = label_Ar;
            Descr_En = descr_En;
            Descr_Ar = descr_Ar;
            this.url_img = url_img;
            Descr_Short = descr_Short;
            Subindicator_Separator = subindicator_Separator;
            this.order_code = order_code;
        }
    }
}