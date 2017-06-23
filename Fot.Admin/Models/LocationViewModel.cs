using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class LocationViewModel
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string MappedToLocation { get; set; }
    }
}