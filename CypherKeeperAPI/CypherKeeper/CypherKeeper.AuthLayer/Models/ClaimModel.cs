using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.AuthLayer.Models
{
    public class ClaimModel
    {
        public string ClaimName { get; set; }
        public dynamic Data { get; set; }
    }
}
