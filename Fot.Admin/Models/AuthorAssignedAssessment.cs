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
    
    public partial class AuthorAssignedAssessment
    {
        public int EntryId { get; set; }
        public Nullable<int> AssessmentId { get; set; }
        public Nullable<int> AdminId { get; set; }
    
        public virtual AdminUser AdminUser { get; set; }
        public virtual Assessment Assessment { get; set; }
    }
}
