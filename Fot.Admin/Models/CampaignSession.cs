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
    
    public partial class CampaignSession
    {
        public int EntryId { get; set; }
        public int CampaignId { get; set; }
        public int SessionId { get; set; }
    
        public virtual TestSession TestSession { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}
