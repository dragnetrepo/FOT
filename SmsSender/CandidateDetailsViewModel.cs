using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SmsSender
{
    public class CandidateDetailsViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MobileNo { get; set; }

        public int? OverallScore
        {
            get { return ResultList.Sum(x => x.TestScore); }
        }

        public int OverallTotalQuestions
        {
            get { return ResultList.Sum(x => x.TotalQuestions); }
        }


        public IEnumerable<ResultViewModel> ResultList { get; set; }


        public int BatchId { get; set; }

        public string Username { get; set; }

        public string UniqueId { get; set; }

        public string Password { get; set; }

        public int EntryId { get; set; }
    }

    public class ResultViewModel
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

        public ResultViewModel()
        {
            
        }
        
    }

}