using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLayer.Models
{
    public class LimitsWithValues
    {
        public CtbsubfeaturelimitsModel Limit { get; set; }
        public List<CtbrolelimitvalueModel> Values { get; set; }
        public bool IsValid { get; set; }

        public LimitsWithValues()
        {
            Limit = null;
            Values = new List<CtbrolelimitvalueModel>();
            IsValid = false;
        }

        public LimitsWithValues(CtbsubfeaturelimitsModel limit, List<CtbrolelimitvalueModel> values)
        {
            Limit = limit;
            Values = values;
            IsValid = false;
        }
    }
}
