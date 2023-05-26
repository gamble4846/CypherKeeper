using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLayer.Models
{
    [Table("ctbControllerAction")]
    public class ctbControllerActionModel
    {
        public Guid GUIDControllerAction { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
