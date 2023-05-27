using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Model
{
    [Table("tbIcons")]
    public class tbIconsModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String Link { get; set; }
        public Boolean isDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
