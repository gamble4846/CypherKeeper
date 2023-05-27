using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Model
{
    [Table("tbWebsites")]
    public class tbWebsitesModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String Name { get; set; }
        public String Link { get; set; }
        public Guid IconId { get; set; }
        public Boolean isDeleted { get; set; } = false;
        public DateTime DeletedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
