using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class UploadTempViewModel
    {
        public int RowNumber { get; set; }
        public string ClientUniqueId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MobileNo { get; set; }
        public string Location { get; set; }
        public string Issue { get; set; }
    }
}