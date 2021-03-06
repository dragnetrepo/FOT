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
    
    public partial class Assessment
    {
        public Assessment()
        {
            this.AssessmentTopics = new HashSet<AssessmentTopic>();
            this.QuestionDifficultyLevels = new HashSet<QuestionDifficultyLevel>();
            this.QuestionGroups = new HashSet<QuestionGroup>();
            this.AssessmentQuestions = new HashSet<AssessmentQuestion>();
        }
    
        public int AssessmentId { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string InstructionText { get; set; }
        public byte[] InstructionImage { get; set; }
        public bool Timed { get; set; }
        public AuthorApp.Models.AssessmentType AssessmentType { get; set; }
        public bool RandomizeQuestions { get; set; }
        public bool RandomizeOptions { get; set; }
        public bool AdvancedOutputOptions { get; set; }
        public int QuestionsPerTest { get; set; }
        public System.DateTime DateAdded { get; set; }
        public Nullable<System.DateTime> DateLastUpdated { get; set; }
        public Nullable<int> AuthorAdminId { get; set; }
    
        public virtual ICollection<AssessmentTopic> AssessmentTopics { get; set; }
        public virtual ICollection<QuestionDifficultyLevel> QuestionDifficultyLevels { get; set; }
        public virtual ICollection<QuestionGroup> QuestionGroups { get; set; }
        public virtual AdminUser AdminUser { get; set; }
        public virtual ICollection<AssessmentQuestion> AssessmentQuestions { get; set; }
    }
}
