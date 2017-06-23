using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Lan.Models
{
    public class AdminUserViewModel
    {
        public int ActualUserId { get; set; }
        public bool IsSupportStaff { get; set; }
        public DateTime DownloadDate { get; set; }
        public byte[] PreTestPhoto { get; set; }
        public byte[] PostTestPhoto { get; set; }
        public bool Synchronized { get; set; }
        public Nullable<int> PreTestCapturedByAdminId { get; set; }
        public Nullable<int> PostTestCapturedByAdminId { get; set; }
    }
}