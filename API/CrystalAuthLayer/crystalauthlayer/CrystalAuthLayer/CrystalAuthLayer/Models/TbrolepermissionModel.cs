using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbRolePermission")]
    public class TbrolepermissionModel
    {
        public int RolePermissionId { get; set; }
        [Required]
        public Guid GUIDRole { get; set; }
        [Range(int.MinValue, int.MaxValue)]
        public int RoleSubFeatureId { get; set; }
        public bool? Allow { get; set; }
    }
}

