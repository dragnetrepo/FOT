//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmsSender
{
    using System;
    using System.Collections.Generic;
    
    public partial class TestSession
    {
        public TestSession()
        {
            this.CampaignEntries = new HashSet<CampaignEntry>();
        }
    
        public int SessionId { get; set; }
        public int CenterId { get; set; }
        public System.DateTime TestDate { get; set; }
        public int TimeIndex { get; set; }
        public string TimeText { get; set; }
    
        public virtual ICollection<CampaignEntry> CampaignEntries { get; set; }
    }
}
