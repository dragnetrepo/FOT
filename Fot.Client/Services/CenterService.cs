using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fot.Client.Infrastructure;
using Fot.Client.Models;

namespace Fot.Client.Services
{
    public class CenterService : ServiceBase
    {
        public IQueryable<Center> Centers
        {
            get { return Context.Centers; }
        }

        public List<CenterViewModel> GetCenters()
        {
            return Centers.OrderByDescending(x => x.CenterId).Select(x => new CenterViewModel
                {
                    CenterId = x.CenterId,
                    CenterName = x.CenterName,
                    LocationName = x.Location.LocationName,
                    CapacityPerSession = x.CapacityPerSession,
                    Active = x.Active
                }).ToList();
        }

        public List<CenterViewModel> GetCenters(int startRow, int maxRows)
        {
           
            
            return
                Centers.OrderByDescending(x => x.CenterId).Skip(startRow).Take(maxRows).Select(x => new CenterViewModel
                    {
                        CenterId = x.CenterId,
                        CenterName = x.CenterName,
                        LocationName = x.Location.LocationName,
                        CapacityPerSession = x.CapacityPerSession,
                        Active = x.Active
                    }).ToList();
        }

        public int Count()
        {
            return Centers.Count();
        }


        public List<CenterViewModel> GetCentersInLocation(int LocationId)
        {
            return
                Centers.Where(x => x.LocationId == LocationId).OrderByDescending(x => x.CenterId).Select(
                    x => new CenterViewModel
                        {
                            CenterId = x.CenterId,
                            CenterName = x.CenterName,
                            LocationName = x.Location.LocationName,
                            CapacityPerSession = x.CapacityPerSession,
                            Active = x.Active
                        }).ToList();
        }

        public List<CenterViewModel> GetCentersInLocation(int LocationId, int startRow, int maxRows)
        {
            return
                Centers.Where(x => x.LocationId == LocationId).OrderByDescending(x => x.CenterId).Skip(startRow).Take(
                    maxRows).Select(x => new CenterViewModel
                        {
                            CenterId = x.CenterId,
                            CenterName = x.CenterName,
                            LocationName = x.Location.LocationName,
                            CapacityPerSession = x.CapacityPerSession,
                            Active = x.Active
                        }).ToList();
        }

        public int CountCenterInLocation(int LocationId)
        {
            return Centers.Count(x => x.LocationId == LocationId);
        }


        public Center GetCenter(int CenterId)
        {
            return Centers.FirstOrDefault(x => x.CenterId == CenterId);

        }


        public List<CenterViewModel> GetCampaignCentersInLocation(int CampaignId, int LocationId)
        {

            return Context.CampaignSessions.Where(x => x.CampaignId == CampaignId && x.TestSession.Center.LocationId == LocationId)
                        .Select(x => new CenterViewModel
                        {
                            CenterId = x.TestSession.CenterId,
                            CenterName = x.TestSession.Center.CenterName
                        }).Distinct().ToList();

        }
   
    }
}