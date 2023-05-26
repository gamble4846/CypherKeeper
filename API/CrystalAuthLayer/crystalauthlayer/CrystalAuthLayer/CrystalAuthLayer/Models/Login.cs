using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLayer.Models
{
    public class Login
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int CompanyIndex { get; set; }
    }

    public class UserData
    {
        public Guid GUIDAccess { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }

        public UserData(Guid GUIDAccess, string ID, string Name, string EMail)
        {
            this.GUIDAccess = GUIDAccess;
            this.ID = ID;
            this.Name = Name;
            this.EMail = EMail;
        }

        public UserData() { }
    }
}
