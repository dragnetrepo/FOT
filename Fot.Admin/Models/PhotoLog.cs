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
    
    public partial class PhotoLog
    {
        public int EntryId { get; set; }
        public int CandidateId { get; set; }
        public int AdminUserId { get; set; }
        public System.DateTime ExpungeDate { get; set; }
    
        public virtual AdminUser AdminUser { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}
