using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;

namespace Fot.Admin.ext.Models
{
    public class CandidateExportStatus
    {
        public Status ExportStatus { get; set; }
        public int TotalSucceeded { get; set; }
        public int TotalFailed { get; set; }

        public List<UploadTempViewModel> IssueList { get; set; } 
    }
}