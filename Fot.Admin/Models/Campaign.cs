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
    
    public partial class Campaign
    {
        public Campaign()
        {
            this.CampaignEntries = new HashSet<CampaignEntry>();
            this.CampaignSessions = new HashSet<CampaignSession>();
        }
    
        public int CampaignId { get; set; }
        public int PartnerId { get; set; }
        public string CampaignName { get; set; }
        public string Instructions { get; set; }
        public string InvitationLogo { get; set; }
        public Nullable<int> LogoPlacement { get; set; }
        public bool Active { get; set; }
        public bool ShowFeedback { get; set; }
        public int BundleId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public bool IsUnproctored { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool ViewResult { get; set; }
        public bool EnableProctoring { get; set; }
        public bool RequireSEB { get; set; }
    
        public virtual AssessmentBundle AssessmentBundle { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual ICollection<CampaignEntry> CampaignEntries { get; set; }
        public virtual ICollection<CampaignSession> CampaignSessions { get; set; }
    }
}
