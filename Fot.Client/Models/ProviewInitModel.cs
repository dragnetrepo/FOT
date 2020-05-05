using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Client.Models
{
    public class ProviewInitModel
    {
        public string Username { get; set; }

        public string ProviewToken { get; set; }

        public string SessionToken { get; set; }

        public bool ProctoringEnabled { get; set; }

        public string TestTitle { get; set; }
    }
}