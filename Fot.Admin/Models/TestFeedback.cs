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
    
    public partial class TestFeedback
    {
        public int CampaignEntryId { get; set; }
        public Nullable<int> Directions { get; set; }
        public Nullable<int> WaitTime { get; set; }
        public Nullable<int> Professionalism { get; set; }
        public Nullable<int> StartTime { get; set; }
        public Nullable<int> Briefing { get; set; }
        public Nullable<int> Registration { get; set; }
        public Nullable<int> Overall { get; set; }
        public Nullable<int> UnsatisfactoryArea { get; set; }
        public Nullable<int> SatisfactoryArea { get; set; }
        public string Comments { get; set; }
    
        public virtual CampaignEntry CampaignEntry { get; set; }
    }
}
