using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.ext.Models
{
    public class PartnerAdminModel
    {
        public int AdminId { get; set; }

        public int? PartnerId { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public bool Active { get; set; }

        public string PartnerName { get; set; }
    }
}