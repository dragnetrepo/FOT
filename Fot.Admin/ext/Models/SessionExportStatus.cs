using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.ext.Models
{
    public class SessionExportStatus
    {
        public int Succeeded { get; set; }
        public int Failed { get; set; }

        public List<Status> IssueList { get; set; } 
    }
}