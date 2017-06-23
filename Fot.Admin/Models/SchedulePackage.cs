using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class SchedulePackage
    {
        public List<TestScheduleViewModel> ScheduleList { get; set; }
        public List<AssessmentBundleViewModel> AssessmentList { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime DownloadDate { get; set; }
        public List<SupportStaffViewModel> StaffList { get; set; }
    }
}