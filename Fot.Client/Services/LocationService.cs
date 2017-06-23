using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fot.Client.Infrastructure;
using Fot.Client.Models;

namespace Fot.Client.Services
{
    public class LocationService : ServiceBase
    {
        public IQueryable<Location> Locations
        {
            get { return Context.Locations; }
        }

        public List<Location> GetLocations()
        {
            return Locations.OrderBy(x => x.LocationName).ToList();
        }

        public List<Location> GetLocations(int startRow, int maxRows)
        {
            return Locations.OrderBy(x => x.LocationName).Skip(startRow).Take(maxRows).ToList();
        }

        public int Count()
        {
            return Locations.Count();
        }

        public List<Location> GetLocationsWithCenters()
        {
            return Locations.Where(y => y.Centers.Count > 0).OrderBy(x => x.LocationName).ToList();
        }

        public List<LocationViewModel> GetMappedLocations()
        {
            return
                Locations.Where(x => x.MappedToLocation.HasValue).OrderBy(x => x.LocationName).Select(
                    x => new LocationViewModel
                        {
                            LocationId = x.LocationId,
                            LocationName = x.LocationName,
                            MappedToLocation = x.ParentLocation.LocationName
                        }).ToList();
        }

        public List<LocationViewModel> GetMappedLocations(int startRow, int maxRows)
        {
            return
                Locations.Where(x => x.MappedToLocation.HasValue).OrderBy(x => x.LocationName).Skip(startRow).Take(
                    maxRows).Select(x => new LocationViewModel
                        {
                            LocationId = x.LocationId,
                            LocationName = x.LocationName,
                            MappedToLocation = x.ParentLocation.LocationName
                        }).ToList();
        }

        public int CountMappedLocations()
        {
            return Locations.Count(x => x.MappedToLocation.HasValue);
        }


        public Location GetLocation(int LocationId)
        {
            return Locations.FirstOrDefault(x => x.LocationId == LocationId);
        }



      

        public List<Location> GetMappableLocations()
        {
            return
                Locations.Where(x => x.MappedToLocation.HasValue == false && x.MappedLocations.Count == 0).OrderBy(
                    x => x.LocationName).ToList();
        }

        public List<Location> GetPossibleParentLocations()
        {
            return
                Locations.Where(x => x.MappedToLocation.HasValue == false && x.MappedLocations.Count >= 0).OrderBy(
                    x => x.LocationName).ToList();
        }




        public List<Location> GetCampaignLocationsWithCenters(int CampaignId)
        {
            return Context.CampaignSessions.Where(x => x.CampaignId == CampaignId)
                         .Select(x => x.TestSession.Center.Location).Distinct().ToList();
        }
  
    }
}