using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Admin.Models
{
    public class WalletScheduledViewModel
    {
        public int EntryId { get; set; }
        public bool IsPrivateCenter { get; set; }
        public bool IsUnProctored { get; set; }
    }
}