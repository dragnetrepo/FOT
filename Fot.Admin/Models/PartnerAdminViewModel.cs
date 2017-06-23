using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class PartnerAdminViewModel
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MobileNo { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool Active { get; set; }
        public string PartnerName { get; set; }
        
    }
}