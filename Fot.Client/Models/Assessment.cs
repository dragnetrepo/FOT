//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fot.Client.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Assessment
    {
        public Assessment()
        {
            this.AssessmentBundleEntries = new HashSet<AssessmentBundleEntry>();
            this.AssessmentOutputConfigs = new HashSet<AssessmentOutputConfig>();
            this.AssessmentQuestions = new HashSet<AssessmentQuestion>();
            this.AssessmentResults = new HashSet<AssessmentResult>();
            this.AssessmentTopics = new HashSet<AssessmentTopic>();
            this.EssayTopics = new HashSet<EssayTopic>();
            this.QuestionDifficultyLevels = new HashSet<QuestionDifficultyLevel>();
            this.QuestionGroups = new HashSet<QuestionGroup>();
            this.FixedQuestions = new HashSet<FixedQuestion>();
        }
    
        public int AssessmentId { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string InstructionText { get; set; }
        public byte[] InstructionImage { get; set; }
        public bool Timed { get; set; }
        public bool ShowCalculator { get; set; }
        public Fot.Client.Models.AssessmentType AssessmentType { get; set; }
        public bool RandomizeQuestions { get; set; }
        public bool RandomizeOptions { get; set; }
        public bool AdvancedOutputOptions { get; set; }
        public int QuestionsPerTest { get; set; }
        public System.DateTime DateAdded { get; set; }
        public Nullable<System.DateTime> DateLastUpdated { get; set; }
        public Nullable<int> OwnerPartnerId { get; set; }
        public bool HasFixedQuestions { get; set; }
        public Nullable<int> AuthorId { get; set; }
        public Nullable<bool> AllowGTDView { get; set; }
    
        public virtual ICollection<AssessmentBundleEntry> AssessmentBundleEntries { get; set; }
        public virtual ICollection<AssessmentOutputConfig> AssessmentOutputConfigs { get; set; }
        public virtual ICollection<AssessmentQuestion> AssessmentQuestions { get; set; }
        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }
        public virtual ICollection<AssessmentTopic> AssessmentTopics { get; set; }
        public virtual ICollection<EssayTopic> EssayTopics { get; set; }
        public virtual ICollection<QuestionDifficultyLevel> QuestionDifficultyLevels { get; set; }
        public virtual ICollection<QuestionGroup> QuestionGroups { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual ICollection<FixedQuestion> FixedQuestions { get; set; }
    }
}
