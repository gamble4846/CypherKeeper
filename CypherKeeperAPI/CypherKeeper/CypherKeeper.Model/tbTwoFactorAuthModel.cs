using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.Model
{
    [Table("tbTwoFactorAuth")]
    public class tbTwoFactorAuthModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String Name { get; set; }
        public String SecretKey { get; set; }
        public String Mode { get; set; }
        public Int32 CodeSize { get; set; } = 6;
        public String Type { get; set; }
        public Guid KeyId { get; set; }
        public Boolean isDeleted { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Int32? ArrangePosition { get; set; }
    }

    public class tbTwoFactorAuthModel_ADD
    {
        public Guid? Id { get; set; }
        public String Name { get; set; }
        public String SecretKey { get; set; }
        public String Mode { get; set; } = "Sha256";
        public Int32 CodeSize { get; set; } = 6;
        public String Type { get; set; } = "TOTP";
        public Guid? KeyId { get; set; }
    }
}
