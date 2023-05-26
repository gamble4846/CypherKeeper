using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbLimitsControllerActions")]
    public class CtblimitscontrolleractionsModel
    {
        public int Id { get; set; }
        [Range(int.MinValue, int.MaxValue)]
        public int LimitsId { get; set; }
        [Required]
        public Guid GUIDControllerAction { get; set; }
    }
}

