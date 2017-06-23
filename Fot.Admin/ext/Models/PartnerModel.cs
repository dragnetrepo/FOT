using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fot.Admin.ext.Models
{
    public class PartnerModel
    {
        public int PartnerId { get; set; }

        public string PartnerName { get; set; }

        public string Address { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
