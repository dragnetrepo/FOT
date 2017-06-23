using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Fot.Client.Infrastructure;
using Fot.Client.Models;

namespace Fot.Client.Services
{
    public class TestSessionService : ServiceBase
    {
        public IQueryable<TestSession> Sessions
        {
            get { return Context.TestSessions; }
        }


        public List<TestSessionViewModel> GetSessions()
        {
            return Sessions.Where(x => x.TestDate >= DateTime.Today).OrderBy(x => x.TestDate).ThenBy(x => x.TimeIndex).Select(x => new TestSessionViewModel
                {
                    SessionId = x.SessionId,
                    CenterName = x.Center.CenterName,
                    TestDate = x.TestDate,
                    TimeText = x.TimeText,
                    Capacity = x.Center.CapacityPerSession,
                    Scheduled = x.CampaignEntries.Count
                }).ToList();
        }


        public List<TestSessionViewModel> GetSessions(int startRow, int maxRows)
        {
            return
                Sessions.Where(x => x.TestDate >= DateTime.Today).OrderBy(x => x.TestDate).ThenBy(x => x.TimeIndex).Skip(startRow).Take(maxRows).Select(
                    x => new TestSessionViewModel
                        {
                            SessionId = x.SessionId,
                            CenterName = x.Center.CenterName,
                            TestDate = x.TestDate,
                            TimeText = x.TimeText,
                            Capacity = x.Center.CapacityPerSession,
                            Scheduled = x.CampaignEntries.Count
                        }).ToList();
        }

        public List<TestSessionViewModel> GetAvailableSessions(int CenterId)
        {
            return Context.TestSessions.Where(x => x.TestDate >= DateTime.Today && (x.CampaignEntries.Count < x.Center.CapacityPerSession) && x.CenterId == CenterId).OrderBy(x => x.TestDate).ThenBy(x => x.TimeIndex).Select(x => new TestSessionViewModel
            {
                SessionId = x.SessionId,
                CenterName = x.Center.CenterName,
                TestDate = x.TestDate,
                TimeText = x.TimeText,
                Capacity = x.Center.CapacityPerSession,
                Scheduled = x.CampaignEntries.Count
                
            }).ToList();
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


        


        public int Count()
        {
            return Sessions.Count(x => x.TestDate >= DateTime.Today);
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
                        LocationId = x.Center.LocationId,
                        TestDate = x.TestDate,
                        TimeText = x.TimeText,
                        Capacity = x.Center.CapacityPerSession,
                        Scheduled = x.CampaignEntries.Count
                    }).FirstOrDefault();
        }





        public List<TestSessionViewModel> GetCampaignAvailableSessions(int CampaignId, int CenterId, int currentSessionId)
        {
            return Context.CampaignSessions.Where(x => x.CampaignId == CampaignId && x.TestSession.CenterId == CenterId && x.SessionId != currentSessionId)
                         .Select(x => new TestSessionViewModel
                         {
                             SessionId = x.SessionId,
                             LocationId = x.TestSession.Center.LocationId,
                             CenterName = x.TestSession.Center.CenterName,
                             TestDate = x.TestSession.TestDate,
                             TimeText = x.TestSession.TimeText
                         }).ToList();
        }

       
    }
}