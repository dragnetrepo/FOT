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
    
    public partial class AssessmentResult
    {
        public int EntryId { get; set; }
        public int CampaignEntryId { get; set; }
        public int AssessmentId { get; set; }
        public Nullable<int> TestScore { get; set; }
        public string CandidateOptions { get; set; }
        public Nullable<int> SelectedEssayId { get; set; }
        public string EssayText { get; set; }
    
        public virtual Assessment Assessment { get; set; }
        public virtual CampaignEntry CampaignEntry { get; set; }
    }
}
