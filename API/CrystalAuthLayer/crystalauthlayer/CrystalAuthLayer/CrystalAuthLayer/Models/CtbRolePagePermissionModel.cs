using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbRolePagePermission")]
    public class CtbRolePagePermissionModel
    {
        public int Id { get; set; }
        public Guid GUIDRole { get; set; }
        public int PageId { get; set; }
        public bool Allow { get; set; }
    }
}

