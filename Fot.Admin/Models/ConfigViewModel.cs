using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class ConfigViewModel
    {
        public int ConfigId { get; set; }
        public string Topic { get; set; }
        public string DifficultyLevel { get; set; }
        public int NumQuestions { get; set; }
    }
}