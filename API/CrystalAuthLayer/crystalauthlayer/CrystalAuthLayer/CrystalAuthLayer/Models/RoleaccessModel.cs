using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("cvRoleAccess")]
    public class RoleaccessModel
    {
        [Required]
        public Guid GUIDAccess { get; set; }
        [Required]
        public string ID { get; set; }
        [Required]
        public Guid GUIDRole { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}

