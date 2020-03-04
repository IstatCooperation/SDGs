namespace MDT2PxWeb.Bean
{
    public class Database
    {
        public string type = "SQLSERVER";
        public string connectionString;
        public string username;
        public string password;
        public Table dimensionsTable;
        public Table dataTable;
        public Table goalTable;
        public Table targetTable;
        public Table indicatorTable;
        public Table subIndicatorTable;
    }
}
