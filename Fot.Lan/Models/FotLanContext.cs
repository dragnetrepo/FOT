using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Fot.Lan.Models
{
    public partial class LanContext : DbContext
    {

        public LanContext(string str)
          : base(str)
        {
        }

    }
}