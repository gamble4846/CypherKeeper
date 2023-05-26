using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbSubfeatureLimits")]
    public class CtbsubfeaturelimitsModel
    {
        public int Id { get; set; }
        [Required]
        public string VaraibleName { get; set; }
        [Required]
        public string Operator { get; set; }
        [Required]
        public string InputType { get; set; }
        public int? SubfeatureId { get; set; }
        public int? DefaultsId { get; set; }
        public string LimitName { get; set; }
    }
}

