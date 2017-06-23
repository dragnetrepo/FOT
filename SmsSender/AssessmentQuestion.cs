//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmsSender
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssessmentQuestion
    {
        public AssessmentQuestion()
        {
            this.AssessmentAnswers = new HashSet<AssessmentAnswer>();
        }
    
        public int QuestionId { get; set; }
        public int AssessmentId { get; set; }
        public Nullable<int> TopicId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<int> DifficultyLevel { get; set; }
        public string QuestionText { get; set; }
        public byte[] QuestionImage { get; set; }
        public string AdditionalText { get; set; }
        public string AnswerType { get; set; }
        public bool OptionsLayoutIsVertical { get; set; }
    
        public virtual Assessment Assessment { get; set; }
        public virtual ICollection<AssessmentAnswer> AssessmentAnswers { get; set; }
        public virtual AssessmentTopic AssessmentTopic { get; set; }
        public virtual QuestionDifficultyLevel QuestionDifficultyLevel { get; set; }
        public virtual QuestionGroup QuestionGroup { get; set; }
    }
}
