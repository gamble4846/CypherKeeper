using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Model
{
    [Table("tbGroups")]
    public class tbGroupsModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String Name { get; set; }
        public Guid? ParentGroupId { get; set; }
        public Guid? IconId { get; set; }
        public Boolean isDeleted { get; set; } = false;
    }
}
