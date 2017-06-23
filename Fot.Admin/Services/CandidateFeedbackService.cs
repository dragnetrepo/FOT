using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class CandidateFeedbackService : ServiceBase
    {
        public List<FeedbackType> GetFeedbackTypes()
        {
            return Context.FeedbackTypes.OrderByDescending(x => x.EntryId).ToList();
        }

        public List<FeedbackType> GetFeedbackTypes(int startRow, int maxRows)
        {
            return Context.FeedbackTypes.OrderByDescending(x => x.EntryId).Skip(startRow).Take(maxRows).ToList();
        }

        public int Count()
        {
            return Context.FeedbackTypes.Count();
        }

        public AppMessage Add(FeedbackType item)
        {
            try
            {
                Context.FeedbackTypes.Add(item);
                Context.SaveChanges();


                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added Feedback subject successfully.",
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

        public void Delete(int EntryId)
        {
            FeedbackType item = Context.FeedbackTypes.Find(EntryId);

            if (item != null)
            {
                Context.FeedbackTypes.Remove(item);
                Context.SaveChanges();
            }
        }


        //candidate actual feedbacks



        public List<CandidateFeedbackViewModel> GetCandidateFeedbacks(int FeedbackTypeId, int startRow, int maxRows)
        {
            IEnumerable<CandidateFeedbackViewModel> query = null;


            if (FeedbackTypeId == 0)
            {
                
                   query = Context.CandidateFeedbacks.Where(x => x.FeedBackTypeId == null)
                           .OrderByDescending(x => x.EntryId)
                           .Select(x => new CandidateFeedbackViewModel
                               {
                                   EntryId = x.EntryId,
                                   Subject = x.FeedbackOther,
                                   CandidateName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                                   DateSent = x.DateSent.Value
                               });
            }

            else if (FeedbackTypeId > 0)
            {
                
                  query =  Context.CandidateFeedbacks.Where(x => x.FeedBackTypeId == FeedbackTypeId)
                           .OrderByDescending(x => x.EntryId)
                           .Select(x => new CandidateFeedbackViewModel
                           {
                               EntryId = x.EntryId,
                               Subject =  x.FeedbackType.FeedbackReason,
                               CandidateName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                               DateSent = x.DateSent.Value
                           });
            }

            else
            {
                
                  query =  Context.CandidateFeedbacks.OrderByDescending(x => x.EntryId)
                           .Select(x => new CandidateFeedbackViewModel
                               {
                                   EntryId = x.EntryId,
                                   Subject = x.FeedBackTypeId.HasValue ? x.FeedbackType.FeedbackReason : x.FeedbackOther,
                                   CandidateName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                                   DateSent = x.DateSent.Value
                               });
            }

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }

        public int CountCandidateFeedbacks(int FeedbackTypeId)
        {
            return Context.CandidateFeedbacks.Count();
        }


        public CandidateFeedbackViewModel GetCandidateFeedback(int EntryId)
        {
            return
                Context.CandidateFeedbacks.Where(x => x.EntryId == EntryId).Select(x => new CandidateFeedbackViewModel
                    {
                        EntryId = x.EntryId,
                        Subject = x.FeedBackTypeId.HasValue ? x.FeedbackType.FeedbackReason : x.FeedbackOther,
                        CandidateName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                        DateSent = x.DateSent.Value,
                        Message = x.FeedbackMessage

                    }).FirstOrDefault();
        }
        public AppMessage AddFeedback(CandidateFeedback item)
        {
            try
            {
                Context.CandidateFeedbacks.Add(item);
                Context.SaveChanges();


                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added Feedback successfully.",
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