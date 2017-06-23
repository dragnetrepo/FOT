using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fot.Lan.Models
{
    public class FeedbackViewModel
    {
        public int? Directions { get; set; }
        public int? WaitTime { get; set; }
        public int? Professionalism { get; set; }
        public int? StartTime { get; set; }
        public int? Briefing { get; set; }
        public int? Registration { get; set; }
        public int? Overall { get; set; }
        public int? UnsatisfactoryArea { get; set; }
        public int? SatisfactoryArea { get; set; }
        public string Comments { get; set; }
    }
}
