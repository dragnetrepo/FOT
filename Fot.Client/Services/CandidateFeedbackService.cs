using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Client.Infrastructure;
using Fot.Client.Models;

namespace Fot.Client.Services
{
    public class CandidateFeedbackService : ServiceBase
    {
        public List<FeedbackType> GetFeedbackTypes()
        {
            return Context.FeedbackTypes.OrderByDescending(x => x.EntryId).ToList();
        }


        public AppMessage Add(CandidateFeedback item)
        {
            try
            {
                Context.CandidateFeedbacks.Add(item);
                Context.SaveChanges();


                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Sent feedback successfully.",
                        Status = MessageStatus.Success,
                        Data = item.EntryId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {
                        IsDone = false,
                        Message = "An error occured." + ex.Message,
                        Status = MessageStatus.Error
                    };
            }
        }
    }
}