using Fot.Admin.Infrastructure;

namespace Fot.Admin.Models
{
    public class AssessmentViewModel
    {
        public int AssessmentId { get; set; }
        public string Name { get; set; }
        public AssessmentType AssessmentType { get; set; }
        public int QuestionCount { get; set; }
        public bool IsValid { get; set; }
  
    }
}