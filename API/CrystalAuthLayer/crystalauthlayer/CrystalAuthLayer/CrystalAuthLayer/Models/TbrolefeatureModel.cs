using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbRoleFeature")]
    public class TbrolefeatureModel
    {
        public int RoleFeatureId { get; set; }
        [Required]
        public string RoleFeatureName { get; set; }
    }
}

