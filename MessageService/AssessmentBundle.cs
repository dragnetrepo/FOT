//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MessageService
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssessmentBundle
    {
        public AssessmentBundle()
        {
            this.AssessmentBundleEntries = new HashSet<AssessmentBundleEntry>();
            this.Campaigns = new HashSet<Campaign>();
        }
    
        public int BundleId { get; set; }
        public Nullable<int> OwnerPartnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] DescriptionImage { get; set; }
        public bool ShowResultsOnSubmit { get; set; }
        public bool SaveAsYouGo { get; set; }
        public bool SendResultNotification { get; set; }
        public Nullable<int> MinAggregatePassScore { get; set; }
        public bool AllowAssessmentSelection { get; set; }
    
        public virtual ICollection<AssessmentBundleEntry> AssessmentBundleEntries { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}