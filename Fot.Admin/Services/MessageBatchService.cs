using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class MessageBatchService : ServiceBase
    {

        public void QueueMessageBatch(MessageBatch batch)
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            Context.Configuration.ValidateOnSaveEnabled = false;

            Context.MessageBatches.Add(batch);

            Context.SaveChanges();

        }

        public int AddBatch(MessageBatch batch)
        {
         

            Context.MessageBatches.Add(batch);

            Context.SaveChanges();

            return batch.BatchId;

        }
    }
}