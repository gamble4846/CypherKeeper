using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLayer.Models
{
    [Table("ctbRoleAccess")]
    public class ctbRoleAccessModel
    {
        public Guid GUIDRoleAccess { get; set; }
        public Guid GUIDRole { get; set; }
        public Guid GUIDAccess { get; set; }
    }
}
