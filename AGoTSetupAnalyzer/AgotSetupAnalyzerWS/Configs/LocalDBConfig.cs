using AgotSetupAnalyzerCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AgotSetupAnalyzerWS.Configs
{
    public class LocalDBConfig : ILocalDBConfig
    {
        public string Server
        {
            get { return ConfigurationManager.AppSettings["agotsetup.postgredb.server"]; }
        }

        public string Port
        {
            get { return ConfigurationManager.AppSettings["agotsetup.postgredb.port"]; }
        }

        public string User
        {
            get { return ConfigurationManager.AppSettings["agotsetup.postgredb.user"]; }
        }

        public string Password
        {
            get { return ConfigurationManager.AppSettings["agotsetup.postgredb.password"]; }
        }

        public string Database
        {
            get { return ConfigurationManager.AppSettings["agotsetup.postgredb.database"]; }
        }
    }
}