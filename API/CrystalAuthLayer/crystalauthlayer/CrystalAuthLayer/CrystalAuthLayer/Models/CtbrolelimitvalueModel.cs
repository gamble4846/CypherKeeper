using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbRoleLimitValue")]
    public class CtbrolelimitvalueModel
    {
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }
        [Range(int.MinValue, int.MaxValue)]
        public int SubfeatureLimitsId { get; set; }
        [Required]
        public Guid GUIDRole { get; set; }
    }
}

