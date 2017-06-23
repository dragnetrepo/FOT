using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class ResultUpdateModel
    {
        public ResultUpdateModel()
        {
            this.Results = new List<ResultEntryModel>();
        }
        public int CandidateId { get; set; }
        public int CampaignId { get; set; }
        public int SessionId { get; set; }
        public byte[] CandidatePhoto { get; set; }

        public int? PhotoCapturedBy { get; set; }

        public DateTime TestStartTime { get; set; }

        public DateTime TestEndTime { get; set; }

        public List<ResultEntryModel> Results { get; set; }

        public FeedbackModel Feedback { get; set; }

    }

    public class FeedbackModel
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


    public class ResultEntryModel
    {
        public int AssessmentId { get; set; }
        public int? TestScore { get; set; }
        public string CandidateOptions { get; set; }
        public int? SelectedEssayId { get; set; }
        public string EssayText { get; set; }
        
    }
}