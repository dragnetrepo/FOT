using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class CenterService : ServiceBase
    {
       

       
        public List<CenterViewModel> GetCenters(int startRow, int maxRows)
        {
           
            
            IEnumerable<CenterViewModel> query =
                Context.Centers.Where(x => x.IsPrivateCenter == false).OrderByDescending(x => x.CenterId).Select(x => new CenterViewModel
                    {
                        CenterId = x.CenterId,
                        CenterName = x.CenterName,
                        LocationName = x.Location.LocationName,
                        CapacityPerSession = x.CapacityPerSession,
                        Active = x.Active
                    });

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }

        public int Count()
        {
            return Context.Centers.Count(x => x.IsPrivateCenter ==false);
        }


        public List<CenterViewModel> GetCentersInLocation(int LocationId)
        {
            return
                Context.Centers.Where(x => x.LocationId == LocationId && x.IsPrivateCenter == false).OrderByDescending(x => x.CenterId).Select(
                    x => new CenterViewModel
                        {
                            CenterId = x.CenterId,
                            CenterName = x.CenterName,
                            LocationName = x.Location.LocationName,
                            CapacityPerSession = x.CapacityPerSession,
                            Active = x.Active
                        }).ToList();
        }

       


        public Center GetCenter(int CenterId)
        {
           
            return Context.Centers.FirstOrDefault(x => x.CenterId == CenterId);

        }

        public AppMessage Add(Center item)
        {
            try
            {
                Context.Centers.Add(item);
                Context.SaveChanges();


                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added center successfully.",
                        Status = MessageStatus.Success,
                        Data = item.LocationId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error};
            }
        }


        public AppMessage Update(Center item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {IsDone = true, Message = "Updated center successfully.", Status = MessageStatus.Success};
            }

            catch (Exception)
            {
                return new AppMessage {IsDone = false, Message = "An error occured.", Status = MessageStatus.Error};
            }
        }


        public void Delete(int CenterId)
        {
            Center item = Context.Centers.Find(CenterId);

            if (item != null)
            {
                Context.Centers.Remove(item);
                Context.SaveChanges();
            }
        }

        public List<Center> GetCentersForCandidatesInCampaign(int CampaignId)
        {
            return
                Context.CampaignEntries.Where(
                    x =>
                    x.CampaignId == CampaignId && x.Scheduled && x.Tested == false && x.TestSession.TestDate >= DateTime.Today)
                       .Select(x => x.TestSession.Center)
                       .Distinct()
                       .ToList();
        }
    }
}