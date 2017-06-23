using System;

namespace Fot.Client.Models
{
    public class TestSessionViewModel
    {
        public int SessionId { get; set; }
        public string CenterName { get; set; }
        public int LocationId { get; set; }
        public DateTime TestDate { get; set; }
        public string TimeText { get; set; }
        public int Capacity { get; set; }
        public int Scheduled { get; set; }
        
        
        public int AvailableSlots
        {
            get { return Capacity - Scheduled; }
        }

        public string DisplayText
        {
            get
            {
                return TestDate.ToString("dd-MMM-yyyy") + " @ " + TimeText;
            }
        }

   
    }
}