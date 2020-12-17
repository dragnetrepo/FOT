using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class OnlineTestViewModel
    {
        public int CampaignEntryId { get; set; }
        public string Username { get; set; }
        public int SaveCount { get; set; }
        public DateTime FirstUpdated { get; set; }
        public DateTime LastUpdated { get; set; }
        public string CurrentAssessment { get; set; }
        public int? TimeRemainingMinutes { get; set; }

        public bool Online
        {
            get
            {
                var ts = DateTime.Now.Subtract(LastUpdated);

                return ts.TotalMinutes < 3;
            }
        }

    }
}