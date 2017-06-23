using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageService
{
    public class MessageViewModel
    {
        public int EntryId { get; set; }

        public string Message { get; set; }

        public string MessageFrom { get; set; }

        public string MobileNo { get; set; }
    }


    public class EmailViewModel
    {
        public int EntryId { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string Subject { get; set; }

    }
}
