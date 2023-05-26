using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLayer.Models
{
    public class FullLimitModel
    {
        public CtbsubfeaturelimitsModel SubfeatureLimit { get; set; }
        public dynamic DefaultsData { get; set; }
        public List<CtbrolelimitvalueModel> Values { get; set; }
        public FullLimitModel() { }

        public FullLimitModel(CtbsubfeaturelimitsModel subfeatureLimit, dynamic defaultsData, List<CtbrolelimitvalueModel> values)
        {
            SubfeatureLimit = subfeatureLimit;
            DefaultsData = defaultsData;
            Values = values;
        }
    }
}
