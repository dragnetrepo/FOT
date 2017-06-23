using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fot.Admin.ext.Models
{
    public class AdminModel
    {
        public int AdminId { get; set; }

        public int? PartnerId { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
