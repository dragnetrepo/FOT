using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.ext.Models
{
    public class CandidateResult
    {
        public string CandidateUniqueId { get; set; }

        public int AggregateScore { get; set; }

        public DateTime DateTested { get; set; }

        public string TestCenter { get; set; }

        public List<AssessmentInfo> Assessments { get; set; } 
    }
}