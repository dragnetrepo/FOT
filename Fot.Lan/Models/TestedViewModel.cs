using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Lan.Models
{
    public class TestedViewModel
    {
        public int CandidateEntryId { get; set; }
        public int CandidateId { get; set; }
        public byte[] CandidatePhoto { get; set; }
        public int CampaignId { get; set; }
        public int SessionId { get; set; }
        public DateTime DateTimeStarted { get; set; }
        public bool AssessmentCompleted { get; set; }
        public DateTime DateTimeCompleted { get; set; }
        public int? PhotoCapturedByAdminId { get; set; }
        public TestFeedbackViewModel TestFeedback { get; set; }
        public List<AssessmentResultViewModel> AssessmentResults { get; set; }
    }

    public class TestFeedbackViewModel
    {
        public int? Directions { get; set; }
        public int? WaitTime { get; set; }
        public int? Professionalism { get; set; }
        public int? StartTime { get; set; }
        public int? Briefing { get; set; }
        public int? Registration { get; set; }
        public int? Overall { get; set; }
        public int? UnsatisfactoryArea { get; set; }
        public int? SatisfactoryArea { get; set; }
        public string Comments { get; set; }
    }
    
    public  class AssessmentResultViewModel
    {
        public int EntryId { get; set; }
        public int CandidateEntryId { get; set; }
        public int AssessmentId { get; set; }
        public string Score { get; set; }
        public string CandidateOptions { get; set; }
        public int? SelectedEssayId { get; set; }
        public string EssayText { get; set; }
    }
}