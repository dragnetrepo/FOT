using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CampaignSessionViewModel
    {
        public int EntryId { get; set; }
        public string LocationName { get; set; }
        public string CenterName { get; set; }
        public DateTime? TestDate { get; set; }
        public string TestTime { get; set; }
    }
}