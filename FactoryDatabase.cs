using System;
namespace POS.DataAccessLayer
{
    using POS.DataAccessLayer.SqlAccessLayer;
    using Provider;
    using Properties;
    class FactoryDatabase
    {
        private Enum Provide { get; set; }
        //connection here
      
        private readonly string port = "";
        private readonly string database =Settings.Default.Database;
        private readonly string username = Settings.Default.username;
        private readonly string password = Settings.Default.password;
        private readonly string server = Settings.Default.ServerIP;
        private readonly string security = Settings.Default.Security.ToString();
        public FactoryDatabase(Enum ProvideServer)
        {
            this.Provide = ProvideServer;
        }
        public IDatabaseHandler Database()
        {
            switch (Provide)
            {
                case provideServer.MNSQL:
                    return new SQLDataAccess(connectionManger());
                default:
                    return null;
            }
        }
        private string connectionManger()
        {
            switch (Provide)
            {
                case provideServer.MNSQL://LAPTOP-V1AN374A
                    //me DESKTOP-9VBP0KO\SQLEXPRESS
                    //Data Source=hardyosman1998150.database.windows.net;Initial Catalog=POS_mac;User ID=hardy;Password=Yaran1998150@;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
                    return string.Format(@"Data Source=DESKTOP-5RRUACT;Initial Catalog={1};Integrated Security=True",
                        "DESKTOP-5RRUACT", database,security
                        );
                                                                          //;Initial Catalog=POS_Market;Integrated Security=True";
                default:
                    return null;
            }
        }
    }
}
