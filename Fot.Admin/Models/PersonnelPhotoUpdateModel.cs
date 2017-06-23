using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fot.Admin.Models
{
    public class PersonnelPhotoUpdateModel
    {
        public int UserId { get; set; }

        public bool IsSupportStaff { get; set; }

        public byte[] PreTestPhoto { get; set; }

        public byte[] PostTestPhoto { get; set; }

        public int PreTestCapturedByAdminId { get; set; }

        public int PostTestCapturedByAdminId { get; set; }

        public DateTime TestDate { get; set; }

    }
}
