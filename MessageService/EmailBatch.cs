//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MessageService
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailBatch
    {
        public EmailBatch()
        {
            this.EmailQueues = new HashSet<EmailQueue>();
        }
    
        public int BatchId { get; set; }
        public Nullable<System.DateTime> BatchDate { get; set; }
        public Nullable<int> MessageType { get; set; }
        public string EmailFrom { get; set; }
        public string EmailName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailText { get; set; }
    
        public virtual ICollection<EmailQueue> EmailQueues { get; set; }
    }
}
