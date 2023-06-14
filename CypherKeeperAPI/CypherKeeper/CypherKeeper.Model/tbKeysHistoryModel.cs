using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Model
{
    [Table("tbKeysHistory")]
    public class tbKeysHistoryModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid KeyId { get; set; }
        public String KeysJSON { get; set; }
        public String Type { get; set; }
        public Boolean isDeleted { get; set; } = false;
        public DateTime Date { get; set; }
    }
}
