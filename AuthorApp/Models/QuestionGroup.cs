//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AuthorApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuestionGroup
    {
        public QuestionGroup()
        {
            this.AssessmentQuestions = new HashSet<AssessmentQuestion>();
        }
    
        public int GroupId { get; set; }
        public int AssessmentId { get; set; }
        public string GroupName { get; set; }
    
        public virtual Assessment Assessment { get; set; }
        public virtual ICollection<AssessmentQuestion> AssessmentQuestions { get; set; }
    }
}
