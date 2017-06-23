using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fot.Admin.Models
{
    public class CandidateFeedbackViewModel
    {
        public int EntryId { get; set; }
        public string Subject { get; set; }
        public string CandidateName { get; set; }
        public DateTime DateSent { get; set; }
        public string Message { get; set; }
    }
}
