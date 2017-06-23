using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class DailyScheduleViewModel
    {
        public int CenterId { get; set; }
        public string CenterName { get; set; }
        public string LocationName { get; set; }
        public int TotalScheduled { get; set; }
        public int TotalTested { get; set; }
        public bool DownloadedSchedule { get; set; }
        public bool TriggeredEndOfDay { get; set; }
    }
}