using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.ext.Models
{
    public class ResultResponse
    {
        public bool Succeeded { get; set; }

        public string ErrorMessage { get; set; }

        public CandidateResult Result { get; set; }
    }
}