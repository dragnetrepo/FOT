using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class AccessLogViewModel
    {
        public int EntryId { get; set; }
        public string Username { get; set; }
        public string LogEntryType { get; set; }
        public string LogEntryDetails { get; set; }
        public DateTime LogDate { get; set; }
        public string IpAddress { get; set; }

    }
}