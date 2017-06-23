using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class QuestionSummaryViewModel
    {
        public int QuestionId { get; set; }

        public string AnswerType { get; set; }

        public string Level { get; set; }

        public string Topic { get; set; }

        public string Group { get; set; }

        public int OptionCount { get; set; }

        public int TotalServed { get; set; }

        public int TotalRight { get; set; }

        public int TotalWrong { get; set; }
    }
}