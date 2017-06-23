using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Lan.Models;

namespace Fot.Lan.Services
{
    public class CandidateAssessmentService : ServiceBase
    {

        public CandidateAssessment GetCandidateAssessment(string CandidateGuid)
        {
            return Context.CandidateAssessments.FirstOrDefault(x => x.CandidateGuid.Equals(CandidateGuid));

        }


        public bool EntryExists(string CandidateGuid)
        {
            return Context.CandidateAssessments.Any(x => x.CandidateGuid.Equals(CandidateGuid));
        }



    }
}