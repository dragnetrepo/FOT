using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class PartnerAssignedCenterService : ServiceBase
    {

    

        public List<CenterViewModel> GetAssignedCenters(int PartnerId, int startRow, int maxRows)
        {
            
            IEnumerable<CenterViewModel> query =    Context.PartnerAssignedCenters.Where(x => x.PartnerId == PartnerId).Select(
                    x =>
                    new CenterViewModel
                    {
                        CenterId = x.EntryId,
                        CenterName = x.Center.CenterName,
                        LocationName = x.Center.Location.LocationName
                    }).OrderByDescending(x => x.CenterId);

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }


        public int Count(int PartnerId)
        {
            return Context.PartnerAssignedCenters.Count(x => x.PartnerId == PartnerId);
        }

        public List<CenterViewModel> GetUnassignedCenters(int PartnerId)
        {
            return
                Context.Centers.Where(x => x.PartnerAssignedCenters.Any(y => y.PartnerId == PartnerId) == false)
                       .Select(x => new CenterViewModel
                       {
                           CenterId = x.CenterId,
                           CenterName = x.CenterName
                       }).ToList();
        }

        public AppMessage AssignCenterToPartner(int centerId, int partnerId)
        {
            try
            {
                var item = new PartnerAssignedCenter { PartnerId = partnerId, CenterId = centerId };

                Context.PartnerAssignedCenters.Add(item);
                Context.SaveChanges();

                return new AppMessage
                {
                    IsDone = true,
                    Message = "Added entry successfully.",
                    Status = MessageStatus.Success
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

        public void DeleteCenterFromPartner(int CenterId)
        {
            PartnerAssignedCenter item = Context.PartnerAssignedCenters.Find(CenterId);

            if (item != null)
            {
                Context.PartnerAssignedCenters.Remove(item);
                Context.SaveChanges();
            }
        }
    }
}