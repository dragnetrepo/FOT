//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmailSender
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssessmentBundleEntry
    {
        public int EntryId { get; set; }
        public int BundleId { get; set; }
        public int AssessmentId { get; set; }
    
        public virtual Assessment Assessment { get; set; }
        public virtual AssessmentBundle AssessmentBundle { get; set; }
    }
}
