using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class AnswerViewModel
    {

        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public byte[] AnswerImage { get; set; }
        public bool IsImage { get; set; }
        public bool IsCorrect { get; set; }
        public string AnswerType { get; set; }
    }
}