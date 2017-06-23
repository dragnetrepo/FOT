using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CampaignViewModel
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string PartnerName { get; set; }
        public int CandidateCount { get; set; }
        public string CampaignType { get; set; }
    }
}