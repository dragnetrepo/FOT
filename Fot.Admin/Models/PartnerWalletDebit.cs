//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fot.Admin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartnerWalletDebit
    {
        public int EntryId { get; set; }
        public Nullable<int> PartnerId { get; set; }
        public Nullable<int> CampaignEntryId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> DebitDate { get; set; }
    
        public virtual CampaignEntry CampaignEntry { get; set; }
        public virtual Partner Partner { get; set; }
    }
}
