using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Model
{
    [Table("tbKeys")]
    public class tbKeysModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ParentGroupId { get; set; }
        public String Name { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public Guid? WebsiteId { get; set; }
        public String Notes { get; set; }
        public Boolean isDeleted { get; set; } = false;
        public DateTime DeletedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
