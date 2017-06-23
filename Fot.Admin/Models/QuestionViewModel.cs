using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string AnswerType { get; set; }
        public int OptionCount { get; set; }

        public byte[] QuestionImage { get; set; }

        public bool ValidQuestion { get; set; }

        public int TotalServed { get; set; }

        public int TotalRight { get; set; }

        public int TotalWrong { get; set; }

        public bool Retired { get; set; }
    }
}