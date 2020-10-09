using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Client.Models
{
    public class InvitationViewModel
    {
        public int CandidateId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public DateTime? TestDate { get; set; }
        public string TimeText { get; set; }
        public string Instructions { get; set; }

        public int? LogoPlacement { get; set; }

        public string LogoFileName { get; set; }

        public bool HasResponded { get; set; }

        public string PhotoFileName { get; set; }
    }
}