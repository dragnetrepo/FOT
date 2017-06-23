using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fot.Admin.ext.Models
{
    public class Status
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public int? OptionalId { get; set; }
    }
}
