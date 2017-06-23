using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Fot.Lan.Models
{
    public struct AppMessage
    {
        public bool IsDone { get; set; }
        public string Message { get; set; }
        public MessageStatus Status { get; set; }
        public object Data { get; set; }
    }


    public static class Messages
    {
        
         public static void ShowMessage(this Literal label, AppMessage appMessage )
         {
             var messageType = string.Empty;
             switch (appMessage.Status)
             {
                 case MessageStatus.Error:
                     {
                         messageType = "error";
                         break;
                         
                     }
                 case  MessageStatus.Info:
                     {
                         messageType = "notice";
                         break;
                     }
                 case MessageStatus.Success:
                     {
                         messageType = "success";
                         break;
                     }
             }
             const string str = "<div class='{0}'>{1}</div>";

             label.Text = string.Format(str, messageType, appMessage.Message);

         }
    }
}