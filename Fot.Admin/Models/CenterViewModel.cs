using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CenterViewModel
    {
        public int CenterId { get; set; }
        public string CenterName { get; set; }
        public string LocationName { get; set; }
        public int CapacityPerSession { get; set; }
        public bool Active { get; set; }
    }
}