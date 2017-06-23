using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class CandidateResponseViewModel
    {
        public int EntryId { get; set; }
        public string Username { get; set; }
        public string UniqueID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool Accepted { get; set; }

        public string RejectReason { get; set; }

        public DateTime DateResponded { get; set; }
        public string LocationName { get; set; }
    }
}