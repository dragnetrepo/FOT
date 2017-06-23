using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fot.Admin.ext.Models
{
    public class CenterModel
    {
        public int CenterId { get; set; }

        public string CenterName { get; set; }

        public string Address { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public int CapacityPerSession { get; set; }

        public decimal? RatePerTested { get; set; }

        public bool Active { get; set; }
    }
}
