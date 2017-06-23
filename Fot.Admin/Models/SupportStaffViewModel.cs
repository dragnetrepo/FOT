using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class SupportStaffViewModel
    {
        public int UserId { get; set; }

        public bool IsSupportStaff { get; set; }

        public bool IsCaptureAdmin { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

    }
}