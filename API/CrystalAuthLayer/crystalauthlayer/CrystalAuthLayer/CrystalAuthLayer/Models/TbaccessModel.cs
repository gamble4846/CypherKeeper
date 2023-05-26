using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("tbAccess")]
    public class TbaccessModel
    {
        public Guid? GUIDAccess { get; set; }
        [Required]
        public string ID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string EmailSignature { get; set; }
        public string BCCAddress { get; set; }
        public string MessageAddr { get; set; }
        public bool? Active { get; set; }
        public Guid? GUIDSalesperson { get; set; }
        public bool? AutoOpenAlerts { get; set; }
        public bool? AutoOpenActivities { get; set; }
        public bool? AutoOpenSchedule { get; set; }
        public string DefaultActivityType { get; set; }
        public string DefaultScheduleClass { get; set; }
        public bool? AutoOpenDashboard { get; set; }
        public bool? AutoOpenOrderManager { get; set; }
        public bool? RestrictBySalesperson { get; set; }
        public bool? EmailSettingsOverride { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public int? EmailType { get; set; }
        public int? SMTPEncryption { get; set; }
        public bool? SMTPAuthRequired { get; set; }
    }
}

