using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbLimitDefaults")]
    public class CtblimitdefaultsModel
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        public string Values { get; set; }
        public string TableName { get; set; }
        public string Condition { get; set; }
        public string ValueColumn { get; set; }
        public string DisplayColumns { get; set; }
    }
}

