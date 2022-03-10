namespace SDGsWA.Bean
{
    public class Department
    {
        private string id;
        private string descr;

        public Department(string id, string descr)
        {
            this.id = id;
            this.descr = descr;
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Descr
        {
            get
            {
                return descr;
            }

            set
            {
                descr = value;
            }
        }



    }
}