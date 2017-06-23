using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class WalletDebitEntryViewModel
    {
        public int EntryId { get; set; }
        public string CampaignName { get; set; }
        public string CandidateName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DebitDate { get; set; }
    }
}