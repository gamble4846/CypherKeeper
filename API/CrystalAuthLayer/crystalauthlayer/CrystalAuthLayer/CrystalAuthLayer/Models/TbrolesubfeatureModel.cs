using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbRoleSubFeature")]
    public class TbrolesubfeatureModel
    {
        public int RoleSubFeatureId { get; set; }
        [Range(int.MinValue, int.MaxValue)]
        public int RoleFeatureId { get; set; }
        [Required]
        public string RoleSubFeatureName { get; set; }
    }
}

