using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class PhotoLogModel
    {
        public int CandidateId { get; set; }
        public int AdminUserId { get; set; }
        public DateTime ExpungeDate { get; set; }
    }
}