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
    
    public partial class Partner
    {
        public Partner()
        {
            this.AdminUsers = new HashSet<AdminUser>();
            this.Assessments = new HashSet<Assessment>();
            this.AssessmentBundles = new HashSet<AssessmentBundle>();
            this.Centers = new HashSet<Center>();
            this.PartnerAssignedAssessments = new HashSet<PartnerAssignedAssessment>();
            this.PartnerWalletDebits = new HashSet<PartnerWalletDebit>();
            this.PartnerWalletEntries = new HashSet<PartnerWalletEntry>();
            this.PartnerAssignedCenters = new HashSet<PartnerAssignedCenter>();
            this.Campaigns = new HashSet<Campaign>();
        }
    
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public bool IsSelfManaged { get; set; }
        public Nullable<decimal> CostPerTestPublic { get; set; }
        public Nullable<decimal> CostPerTestPrivate { get; set; }
        public decimal WalletBalance { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<bool> IsIntegrationPartner { get; set; }
        public string ResultsPostUrl { get; set; }
        public string APIKey { get; set; }
    
        public virtual ICollection<AdminUser> AdminUsers { get; set; }
        public virtual ICollection<Assessment> Assessments { get; set; }
        public virtual ICollection<AssessmentBundle> AssessmentBundles { get; set; }
        public virtual ICollection<Center> Centers { get; set; }
        public virtual ICollection<PartnerAssignedAssessment> PartnerAssignedAssessments { get; set; }
        public virtual ICollection<PartnerWalletDebit> PartnerWalletDebits { get; set; }
        public virtual ICollection<PartnerWalletEntry> PartnerWalletEntries { get; set; }
        public virtual ICollection<PartnerAssignedCenter> PartnerAssignedCenters { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}