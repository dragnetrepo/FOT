using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class TestScheduleViewModel
    {
        public int CandidateId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MobileNo { get; set; }
        public int BundleId { get; set; }
        public int CampaignId { get; set; }
        public int SessionId { get; set; }

        public bool ShowFeedback { get; set; }


    }
}