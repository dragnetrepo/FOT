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
    
    public partial class AssessmentAuthor
    {
        public AssessmentAuthor()
        {
            this.Assessments = new HashSet<Assessment>();
        }
    
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    
        public virtual ICollection<Assessment> Assessments { get; set; }
    }
}
