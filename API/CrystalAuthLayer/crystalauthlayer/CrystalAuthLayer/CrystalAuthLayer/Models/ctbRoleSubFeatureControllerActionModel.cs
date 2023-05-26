using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLayer.Models
{
    [Table("ctbRoleSubFeatureControllerAction")]
    public class ctbRoleSubFeatureControllerActionModel
    {
        public Int32 SubFeatureControllerActionID { get; set; }
        public Int32 RoleSubFeatureId { get; set; }
        public Guid GUIDControllerAction { get; set; }
    }
}
