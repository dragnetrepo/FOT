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
    
    public partial class Center
    {
        public Center()
        {
            this.TestSessions = new HashSet<TestSession>();
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
    
        public virtual ICollection<TestSession> TestSessions { get; set; }
        public virtual Location Location { get; set; }
    }
}
