using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class PartnerWalletEntryViewModel
    {
        public int EntryId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryAdmin { get; set; }
    }
}