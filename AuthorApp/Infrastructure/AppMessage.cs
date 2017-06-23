using System;
using System.Collections.Generic;
using System.Linq;


namespace AuthorApp.Infrastructure
{
    public struct AppMessage
    {
        public bool IsDone { get; set; }
        public string Message { get; set; }
        public MessageStatus Status { get; set; }
        public object Data { get; set; }
    }



}