using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CandidateCampaignViewModel
    {
        public int EntryId { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public bool Scheduled { get; set; }
        public bool Tested { get; set; }
        public DateTime? DateTested { get; set; }

    }
}