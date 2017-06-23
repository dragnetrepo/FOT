using System;

namespace AuthorApp.Models
{
    public class AssessmentViewModel
    {

        public int AssessmentId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public AssessmentType AssessmentType { get; set; }
        public int QuestionCount { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Added { get { return DateAdded.ToString("dd-MMM-yyyy"); } }
        public string Updated { get { return LastUpdated.ToString("dd-MMM-yyyy"); } }
    }
}
