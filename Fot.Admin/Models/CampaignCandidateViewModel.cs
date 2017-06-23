using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CampaignCandidateViewModel
    {
        public int EntryId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool Scheduled { get; set; }
        public bool Tested { get; set; }
        public string LocationName { get; set; }
    }
}