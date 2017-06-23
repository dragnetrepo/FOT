using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fot.Admin.ext.Models
{
    public class CenterAdminModel
    {
        public int AdminId { get; set; }

        public int? CenterId { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string CenterLocation { get; set; }

        public bool Active { get; set; }

        public string CenterName { get; set; }
    }
}
