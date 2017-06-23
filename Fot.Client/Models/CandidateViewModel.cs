using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Client.Models
{
    public class CandidateViewModel
    {
        public int CandidateId { get; set; }
        public string CandidateGuid { get; set; }
        public int BundleId { get; set; }
        public bool AssessmentCompleted { get; set; }
       
    }
}