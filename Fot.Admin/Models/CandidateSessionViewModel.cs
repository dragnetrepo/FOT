using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CandidateSessionViewModel
    {
        public int EntryId { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string SessionTime { get; set; }
        public int TimeIndex { get; set; }
    }
}