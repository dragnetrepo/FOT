using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class EmailBatchService : ServiceBase
    {

        public void QueueEmailBatch(EmailBatch batch)
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            Context.Configuration.ValidateOnSaveEnabled = false;

            Context.EmailBatches.Add(batch);

            Context.SaveChanges();

            

        }


        public int AddBatch(EmailBatch batch)
        {
            Context.EmailBatches.Add(batch);

            Context.SaveChanges();

            return batch.BatchId;
        }
    }
}