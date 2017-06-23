using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fot.Admin.ext.Models
{
   public class EndOfDayInfo
    {
       public int TotalCandidates { get; set; }
       public int TotalTested { get; set; }
       public bool DownloadedSchedule { get; set; }
       public bool TriggeredEndOfDay { get; set; }
       public int TotalSessions { get; set; }
       public int TotalPhotoCaptured { get; set; }
       public string StartTime { get; set; }
       public string EndTime { get; set; }
    }
}
