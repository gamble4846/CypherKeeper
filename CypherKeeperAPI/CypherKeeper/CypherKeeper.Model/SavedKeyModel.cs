using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Model
{
    public class SavedKeyModel
    {
        public ToSaveKey Key { get; set; }
        public List<ToSaveStringKeyField> StringKeyFields { get; set; }
    }

    public class ToSaveKey
    {
        public Guid? Id { get; set; }
        public Guid ParentGroupId { get; set; }
        public String Name { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public Guid? WebsiteId { get; set; }
        public String Notes { get; set; }
    }

    public class ToSaveStringKeyField
    {
        public Guid? Id { get; set; }
        public String Name { get; set; }
        public String Value { get; set; }
    }
}
