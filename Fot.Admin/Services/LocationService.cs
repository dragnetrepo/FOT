using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class LocationService : ServiceBase
    {
        public IQueryable<Location> Locations
        {
            get { return Context.Locations; }
        }


        public List<Location> GetLocations(int startRow, int maxRows)
        {
            if (startRow >= 0)
            {
                return Locations.OrderBy(x => x.LocationName).Skip(startRow).Take(maxRows).ToList();
            }
            else
            {
                return Locations.OrderBy(x => x.LocationName).ToList();
            }
        }

        public int Count()
        {
            return Locations.Count();
        }

        public List<Location> GetLocationsWithCenters()
        {
            return Locations.Where(y => y.Centers.Count > 0).OrderBy(x => x.LocationName).ToList();
        }


        public List<Location> GetPartnerLocationsWitchCenters()
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            var partnerList =
                Context.Centers.Where(x => x.IsPrivateCenter && x.OwnerPartnerId == currentAdmin.PartnerId).Select(
                    x => x.Location).ToList();

            var assignedList = Context.PartnerAssignedCenters.Where(x => x.PartnerId == currentAdmin.PartnerId).Select(
                x => x.Center.Location).ToList();

            partnerList.AddRange(assignedList);

            return partnerList.Distinct().ToList();
        }

        public List<Location> GetLocationsWithForCandidatesInCampaign(int CampaignId)
        {
            return
                Context.CampaignEntries.Where(
                    x =>
                    x.CampaignId == CampaignId && x.Scheduled == false && x.Tested == false)
                       .Select(
                           x =>
                           x.Candidate.Location.MappedToLocation.HasValue
                               ? x.Candidate.Location.ParentLocation
                               : x.Candidate.Location)
                       .Distinct()
                       .ToList();
        }


        public List<Location> GetLocationsWithForScheduledCandidatesInCampaign(int CampaignId)
        {
            return
                Context.CampaignEntries.Where(
                    x =>
                    x.CampaignId == CampaignId && x.Scheduled && x.Tested == false)
                       .Select(x =>
                               x.Candidate.Location.MappedToLocation.HasValue
                                   ? x.Candidate.Location.ParentLocation
                                   : x.Candidate.Location)
                       .Distinct()
                       .ToList();
        }


        public List<LocationViewModel> GetMappedLocations(int startRow, int maxRows)
        {
            IEnumerable<LocationViewModel> query = null;


            query =
                Locations.Where(x => x.MappedToLocation.HasValue)
                         .OrderBy(x => x.LocationName)
                         .Select(x => new LocationViewModel
                             {
                                 LocationId = x.LocationId,
                                 LocationName = x.LocationName,
                                 MappedToLocation = x.ParentLocation.LocationName
                             });


            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }

        public int CountMappedLocations()
        {
            return Locations.Count(x => x.MappedToLocation.HasValue);
        }


        public Location GetLocation(int LocationId)
        {
            return Locations.FirstOrDefault(x => x.LocationId == LocationId);
        }

        public bool Exists(string LocationName)
        {
            return Locations.Any(x => x.LocationName.Equals(LocationName));
        }

        public bool ExistsExcept(int LocationId, string LocationName)
        {
            return Locations.Any(x => x.LocationId != LocationId && x.LocationName.Equals(LocationName));
        }

        public AppMessage Add(Location item)
        {
            if (Exists(item.LocationName.ToUpper()))
            {
                return new AppMessage
                    {IsDone = false, Message = "Specified location already exists.", Status = MessageStatus.Error};
            }

            try
            {
                item.LocationName = item.LocationName.ToUpper();

                Context.Locations.Add(item);
                Context.SaveChanges();


                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added location successfully.",
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

        public AppMessage MapLocation(int LocationId, int MapToLocationId)
        {
            if (LocationId == MapToLocationId)
            {
                return new AppMessage
                    {IsDone = false, Message = "You can't map a location to itself.", Status = MessageStatus.Error};
            }

            Location item = GetLocation(LocationId);
            if (item != null)
            {
                item.MappedToLocation = MapToLocationId;

                return Update(item);
            }

            else
            {
                return new AppMessage
                    {IsDone = false, Message = "Specified location does not exist.", Status = MessageStatus.Error};
            }
        }

        public AppMessage Update(Location item)
        {
            if (ExistsExcept(item.LocationId, item.LocationName.ToUpper()))
            {
                return new AppMessage
                    {IsDone = false, Message = "Specified location already exists.", Status = MessageStatus.Error};
            }


            try
            {
                item.LocationName = item.LocationName.ToUpper();

                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {IsDone = true, Message = "Updated location successfully.", Status = MessageStatus.Success};
            }

            catch (Exception)
            {
                return new AppMessage {IsDone = false, Message = "An error occured.", Status = MessageStatus.Error};
            }
        }

        public void Delete(int LocationId)
        {
            Location item = Context.Locations.Find(LocationId);

            if (item != null)
            {
                Context.Locations.Remove(item);
                Context.SaveChanges();
            }
        }

        public void DeleteMapping(int LocationId)
        {
            Location item = Context.Locations.Find(LocationId);

            if (item != null)
            {
                item.MappedToLocation = null;

                Update(item);
            }
        }
    }
}