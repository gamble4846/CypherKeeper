using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.AuthLayer.Models
{
    public class SettingsModel
    {
        public List<Server> Servers { get; set; }
    }

    public class Server
    {
        public Guid GUIDServer { get; set; }
        public string ServerName { get; set; }
        public string DatabaseType { get; set; }
        public string ConnectionString { get; set; }
        public string KeyVerify { get; set; }
        public string ImageLink { get; set; }
    }

    public class ServerViewModel
    {
        public string ServerName { get; set; }
        public string DatabaseType { get; set; }
        public string ConnectionString { get; set; }
        public string Key { get; set; }
        public string ImageLink { get; set; }
    }

    public class ServerDisplayModel
    {
        public Guid GUIDServer { get; set; }
        public string ServerName { get; set; }
        public string DatabaseType { get; set; }
        public string ImageLink { get; set; }
    }

    public class SelectedServerModel
    {
        public Guid GUIDServer { get; set; }
        public string ServerName { get; set; }
        public string DatabaseType { get; set; }
        public string ConnectionString { get; set; }
        public string Key { get; set; }
        public string ImageLink { get; set; }
    }

    public class SelectServerModel
    {
        public Guid GUIDServer { get; set; }
        public string Key { get; set; }
    }
}
