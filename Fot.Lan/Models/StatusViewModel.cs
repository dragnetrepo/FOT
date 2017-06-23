using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Humanizer;

namespace Fot.Lan.Models
{
    public class StatusViewModel
    {
        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime? TimeStarted { get; set; }

        public int SaveCount { get; set; }

        public DateTime? LastSaved { get; set; }

        public string LastSaveStr => LastSaved?.Humanize(false);
    }
}