using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class AssessmentSummaryViewModel
    {
        public string AssessmentName { get; set; }

        public string Developer { get; set; }

        public string YearCreated { get; set; }

        public string AssessmentType { get; set; }

        public int Deployments { get; set; }

        public IEnumerable<PartnerCampaign> Campaigns { get; set; }
    }


    public class PartnerCampaign
    {
        public string PartnerName { get; set; }

        public string CampaignName { get; set; }
    }
}