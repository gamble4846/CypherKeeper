using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLayer.Models
{
    public class RoleWithAllow
    {
        public Guid GUIDRole { get; set; }
        public string RoleName { get; set; }
        public bool Allow { get; set; }
    }
}
