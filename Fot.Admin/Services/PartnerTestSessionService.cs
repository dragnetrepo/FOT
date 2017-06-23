using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class PartnerTestSessionService : ServiceBase
    {
        public IQueryable<TestSession> Sessions
        {
            get { return Context.TestSessions; }
        }




        public List<TestSessionViewModel> GetSessions(int startRow, int maxRows)
        {

            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            IEnumerable<TestSessionViewModel> query = 
                Sessions.Where(x => x.TestDate >= DateTime.Today && x.Center.IsPrivateCenter && x.Center.OwnerPartnerId == currentAdmin.PartnerId).OrderBy(x => x.TestDate).ThenBy(x => x.TimeIndex).Select(
                    x => new TestSessionViewModel
                    {
                        SessionId = x.SessionId,
                        CenterName = x.Center.CenterName,
                        TestDate = x.TestDate,
                        TimeText = x.TimeText,
                        Capacity = x.Center.CapacityPerSession,
                        Scheduled = x.CampaignEntries.Count
                    });

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }

        public List<TestSessionViewModel> GetAvailableSessions(int CenterId)
        {
            return Sessions.Where(x => x.TestDate >= DateTime.Today && x.CampaignEntries.Count < x.Center.CapacityPerSession && x.CenterId == CenterId).OrderBy(x => x.TestDate).ThenBy(x => x.TimeIndex).Select(x => new TestSessionViewModel
            {
                SessionId = x.SessionId,
                CenterName = x.Center.CenterName,
                TestDate = x.TestDate,
                TimeText = x.TimeText,
                Capacity = x.Center.CapacityPerSession,
                Scheduled = x.CampaignEntries.Count

            }).ToList();
        }


        public int Count()
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            return Sessions.Count(x => x.TestDate >= DateTime.Today && x.Center.IsPrivateCenter && x.Center.OwnerPartnerId == currentAdmin.PartnerId);
        }



        public List<TestSessionViewModel> GetAvailableSessionsForUnscheduleView(int CenterId)
        {
            return Sessions.Where(x => x.TestDate >= DateTime.Today && x.CampaignEntries.Count > 0 && x.CenterId == CenterId).OrderBy(x => x.TestDate).ThenBy(x => x.TimeIndex).Select(x => new TestSessionViewModel
            {
                SessionId = x.SessionId,
                CenterName = x.Center.CenterName,
                TestDate = x.TestDate,
                TimeText = x.TimeText,
                Capacity = x.Center.CapacityPerSession,
                Scheduled = x.CampaignEntries.Count

            }).ToList();
        }








     




        public TestSession GetSession(int SessionId)
        {
            return Sessions.FirstOrDefault(x => x.SessionId == SessionId);
        }

        public TestSessionViewModel GetSessionDetails(int SessionId)
        {
            return Sessions.Where(x => x.SessionId == SessionId).Select(
                x => new TestSessionViewModel
                {
                    SessionId = x.SessionId,
                    CenterName = x.Center.CenterName,
                    TestDate = x.TestDate,
                    TimeText = x.TimeText,
                    Capacity = x.Center.CapacityPerSession,
                    Scheduled = x.CampaignEntries.Count
                }).FirstOrDefault();
        }


        public bool Exists(TestSession item)
        {
            return
                Sessions.Any(
                    x => x.CenterId == item.CenterId && x.TestDate == item.TestDate && x.TimeIndex == item.TimeIndex);
        }

        public bool ExistsExcept(TestSession item)
        {
            return
                Sessions.Any(
                    x =>
                    x.SessionId != item.SessionId && x.CenterId == item.CenterId && x.TestDate == item.TestDate &&
                    x.TimeIndex == item.TimeIndex);
        }

        public AppMessage Add(TestSession item)
        {
            if (Exists(item))
            {
                return new AppMessage { IsDone = false, Message = "An session already exists with specified center, date and time.", Status = MessageStatus.Error };
            }

            try
            {
                Context.TestSessions.Add(item);
                Context.SaveChanges();


                return new AppMessage
                {
                    IsDone = true,
                    Message = "Added session successfully.",
                    Status = MessageStatus.Success,
                    Data = item.SessionId
                };
            }

            catch (Exception ex)
            {
                return new AppMessage { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }


        public AppMessage Update(TestSession item)
        {
            if (ExistsExcept(item))
            {
                return new AppMessage { IsDone = false, Message = "An session already exists with specified center, date and time.", Status = MessageStatus.Error };
            }

            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Updated session successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }


        public void Delete(int SessionId)
        {
            TestSession item = Context.TestSessions.Find(SessionId);

            if (item != null)
            {
                Context.TestSessions.Remove(item);
                Context.SaveChanges();
            }
        }

        public List<TestSessionViewModel> GetCurrentSessionsForCenter(int CenterId)
        {

            return
                Sessions.Where(x => x.TestDate >= DateTime.Today && x.CenterId == CenterId)
                        .OrderBy(x => x.TestDate)
                        .ThenBy(x => x.TimeIndex)
                        .Select(x => new TestSessionViewModel
                        {
                            SessionId = x.SessionId,
                            CenterName = x.Center.CenterName,
                            TestDate = x.TestDate,
                            TimeText = x.TimeText,
                            Capacity = x.Center.CapacityPerSession,
                            Scheduled = x.CampaignEntries.Count
                        }).ToList();
        }
    }
}