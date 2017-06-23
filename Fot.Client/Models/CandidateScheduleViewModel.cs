using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Client.Models
{
    public class CandidateScheduleViewModel
    {
        public int CampaignEntryId { get; set; }
        public int CampaignId { get; set; }
        public int SessionId { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public DateTime TestDate { get; set; }
        public string TimeText { get; set; }
        public bool IsPrivateCenter { get; set; }
    }
}