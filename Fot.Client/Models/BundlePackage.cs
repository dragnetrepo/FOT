using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fot.Client.Models
{
    public class BundlePackage
    {
        public int BundleId { get; set; }
        public string BundleName { get; set; }
        public byte[] BundleData { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
    }
}