using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbRole")]
    public class TbroleModel
    {
        public Guid GUIDRole { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}

