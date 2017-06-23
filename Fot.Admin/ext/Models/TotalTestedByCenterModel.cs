using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fot.Admin.ext.Models
{
    public class TotalTestedByCenterModel
    {
        public int CenterId { get; set; }

        public string CenterName { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public int TotalTested { get; set; }
    }
}
