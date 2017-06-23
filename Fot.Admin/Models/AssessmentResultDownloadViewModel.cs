using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Fot.Admin.Models
{
    public class AssessmentResultDownloadViewModel
    {
        public int CampaignEntryId { get; set; }
        public int CampaignId { get; set; }
        public int CandidateId { get; set; }

        public string ClientUniqueId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateTime DateTested { get; set; }
        public string Locaton { get; set; }
        public string CenterName { get; set; }

        public int? OverallScore
        {
            get { return ResultList.Sum(x => x.TestScore); }
        }

        public int OverallTotalQuestions
        {
            get { return ResultList.Sum(x => x.TotalQuestions); }
        }

        public IEnumerable<ResultDownloadViewModel> ResultList { get; set; }

    }

    public class ResultDownloadViewModel
    {
        public int EntryId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public int? TestScore { get; set; }
        public string CandidateOptions { get; set; }

        public int TotalQuestions
        {
            get { return CandidateOptions.Split(';').Count(); }
        }

        public List<ResultTopicsViewModel> Topics { get; set; }


        public ResultDownloadViewModel()
        {
            Topics = new List<ResultTopicsViewModel>();
        }
        
    }



    public class ResultTopicsViewModel
    {

        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public int? TopicScore { get; set; }
    }
}