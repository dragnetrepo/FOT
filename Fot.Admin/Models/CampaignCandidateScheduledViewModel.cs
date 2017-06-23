using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CampaignCandidateScheduledViewModel
    {
        public int EntryId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string LocationName { get; set; }
        public string CenterName { get; set; }
        public DateTime TestDate { get; set; }
        public string TestTime { get; set; }

        public string Email { get; set; }
    }
}