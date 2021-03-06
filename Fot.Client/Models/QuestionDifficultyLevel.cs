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
    
    public partial class QuestionDifficultyLevel
    {
        public QuestionDifficultyLevel()
        {
            this.AssessmentOutputConfigs = new HashSet<AssessmentOutputConfig>();
            this.AssessmentQuestions = new HashSet<AssessmentQuestion>();
        }
    
        public int LevelId { get; set; }
        public int AssessmentId { get; set; }
        public string LevelName { get; set; }
        public int LevelWeight { get; set; }
    
        public virtual Assessment Assessment { get; set; }
        public virtual ICollection<AssessmentOutputConfig> AssessmentOutputConfigs { get; set; }
        public virtual ICollection<AssessmentQuestion> AssessmentQuestions { get; set; }
    }
}
