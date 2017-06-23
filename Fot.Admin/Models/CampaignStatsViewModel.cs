using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CampaignStatsViewModel
    {
        public int Total { get; set; }
        public int Scheduled { get; set; }
        public int Unscheduled { get; set; }
    }
}