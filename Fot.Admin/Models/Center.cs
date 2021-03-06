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
    
    public partial class Center
    {
        public Center()
        {
            this.AdminUsers = new HashSet<AdminUser>();
            this.ScheduleDownloads = new HashSet<ScheduleDownload>();
            this.TestSessions = new HashSet<TestSession>();
            this.PartnerAssignedCenters = new HashSet<PartnerAssignedCenter>();
            this.CenterUsers = new HashSet<CenterUser>();
        }
    
        public int CenterId { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public int LocationId { get; set; }
        public int CapacityPerSession { get; set; }
        public Nullable<decimal> RatePerTested { get; set; }
        public bool Active { get; set; }
        public bool IsPrivateCenter { get; set; }
        public Nullable<int> OwnerPartnerId { get; set; }
    
        public virtual ICollection<AdminUser> AdminUsers { get; set; }
        public virtual Location Location { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual ICollection<ScheduleDownload> ScheduleDownloads { get; set; }
        public virtual ICollection<TestSession> TestSessions { get; set; }
        public virtual ICollection<PartnerAssignedCenter> PartnerAssignedCenters { get; set; }
        public virtual ICollection<CenterUser> CenterUsers { get; set; }
    }
}
