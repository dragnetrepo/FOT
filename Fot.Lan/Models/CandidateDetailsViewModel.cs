namespace Fot.Lan.Models
{
    public class CandidateDetailsViewModel
    {
        public int CandidateEntryId { get; set; }

        public int CandidateId { get; set; }

        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public System.DateTime? DateTimeStarted { get; set; }

        public System.DateTime? DateTimeCompleted { get; set; }

        public bool AssessmentStarted { get; set; }

        public bool AssessmentCompleted { get; set; }

        public bool Synchronized { get; set; }
    }
}
