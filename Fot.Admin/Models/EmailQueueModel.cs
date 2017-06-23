using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class EmailQueueModel
    {
        public int EntryId { get; set; }
        public int BatchId { get; set; }
        public string EmailTo { get; set; }

        public int? CampaignEntryId { get; set; }

        public Nullable<System.DateTime> DateTimeQueued { get; set; }
        public Nullable<System.DateTime> DateTimeSent { get; set; }
        public bool Sent { get; set; }

        
    }
}