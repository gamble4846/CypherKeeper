using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthLayer.Models
{
    [Table("ctbMenuPages")]
    public class CtbMenuPageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string RelativeRoute { get; set; }
        public string Icon { get; set; }
        public bool isCustom { get; set; }
        public bool showPageInMenu { get; set; }
        public string IconLibrary { get; set; }
    }
}

