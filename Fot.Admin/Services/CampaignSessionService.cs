using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class CampaignSessionService : ServiceBase
    {

        public List<CampaignSessionViewModel> GetCampaignSessions(int campaignId, int startRow, int maxRows)
        {

            return Context.CampaignSessions.Where(x => x.CampaignId == campaignId)
                          .Select(x => new CampaignSessionViewModel
                              {
                                  EntryId = x.EntryId,
                                  LocationName = x.TestSession.Center.Location.LocationName,
                                  CenterName = x.TestSession.Center.CenterName,
                                  TestDate = x.TestSession.TestDate,
                                  TestTime = x.TestSession.TimeText
                              }).OrderByDescending(x => x.EntryId).Skip(startRow).Take(maxRows).ToList();
        } 


        public int Count(int campaignId)
        {
            return Context.CampaignSessions.Count(x => x.CampaignId == campaignId);
        }


        public AppMessage Add(CampaignSession item)
        {

            if (EntryExists(item))
            {
                return new AppMessage { Message = "This session already exists in your list of campaign sessions", IsDone = false, Status = MessageStatus.Error};
            }

            try
            {
                Context.CampaignSessions.Add(item);

                Context.SaveChanges();

                return new AppMessage { Message = "Session added successfully.", IsDone = true, Status = MessageStatus.Success };
            }
            catch (Exception ex)
            {
                return new AppMessage { Message = "An error occured. Error:" + ex.Message, IsDone = false, Status = MessageStatus.Error };
            }


        }


        public void Delete(int EntryId)
        {
            var item = Context.CampaignSessions.Find(EntryId);

            if (item != null)
            {
                Context.CampaignSessions.Remove(item);
                Context.SaveChanges();
            }
        }


        public bool EntryExists(CampaignSession item)
        {
            return Context.CampaignSessions.Any(x => x.SessionId == item.SessionId && x.CampaignId == item.CampaignId);
        }
    }
}