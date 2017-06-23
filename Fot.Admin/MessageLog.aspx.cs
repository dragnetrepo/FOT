using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Services;

namespace Fot.Admin
{
    public partial class MessageLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }




        public List<MessageLogViewModel> GetMessageLog(int startRow, int maxRows)
        {
            var ctx = new ServiceBase().Context;

            IEnumerable<MessageLogViewModel> query =
                ctx.EmailBatches.OrderByDescending(x => x.BatchDate).Select(x => new MessageLogViewModel
                {
                    BatchId = x.BatchId,
                    BatchDate = x.BatchDate.Value,
                    Subject = x.EmailSubject,
                    Queued = x.EmailQueues.Count(),
                    Sent= x.EmailQueues.Count(y => y.Sent)
                });

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }


        public int GetMessageLogCount()
        {
            var ctx = new ServiceBase().Context;

            return ctx.EmailBatches.Count();
        }
    }


    public class MessageLogViewModel
    {
        public int BatchId { get; set; }

        public DateTime BatchDate { get; set; }

        public string Subject { get; set; }

        public int Queued { get; set; }

        public int Sent { get; set; }
        
    }
}