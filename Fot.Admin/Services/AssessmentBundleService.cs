using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class AssessmentBundleService : ServiceBase
    {
        public IQueryable<AssessmentBundle> Bundles
        {
            get { return Context.AssessmentBundles; }
        }


     

        public List<AssessmentBundleViewModel> GetBundles(int startRow, int maxRows)
        {
            IEnumerable<AssessmentBundleViewModel> query = null;

            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            if (currentAdmin.IsPartnerAdmin)
            {
                
                  query =  Bundles.Where(x => x.OwnerPartnerId == currentAdmin.PartnerId)
                           .OrderByDescending(x => x.BundleId)
                           .Select(
                               x => new AssessmentBundleViewModel
                                   {
                                       BundleId = x.BundleId,
                                       Name = x.Name
                                   });
            }

            else
            {
                
                 query =   Bundles.Where(x => x.OwnerPartnerId.HasValue == false)
                           .OrderByDescending(x => x.BundleId)
                          
                           .Select(
                               x => new AssessmentBundleViewModel
                                   {
                                       BundleId = x.BundleId,
                                       Name = x.Name
                                   });
            }

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }

        public int Count()
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            if (currentAdmin.IsPartnerAdmin)
            {
                return Bundles.Count(x => x.OwnerPartnerId == currentAdmin.PartnerId);
            }
            else
            {
                return Bundles.Count(x => x.OwnerPartnerId.HasValue == false);
            }
           
        }

        public AssessmentBundle GetBundle(int BundleId)
        {
            return Bundles.FirstOrDefault(x => x.BundleId == BundleId);
        }


        public byte[] GetDescriptionImage(int BundleId)
        {
            AssessmentBundle item = GetBundle(BundleId);

            return item == null ? null : item.DescriptionImage;
        }


        public AppMessage Add(AssessmentBundle item)
        {
            try
            {
                Context.AssessmentBundles.Add(item);
                Context.SaveChanges();

                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added assessment bundle successfully.",
                        Status = MessageStatus.Success,
                        Data = item.BundleId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error};
            }
        }


        public AppMessage Update(AssessmentBundle item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {IsDone = true, Message = "Updated assessment bundle successfully.", Status = MessageStatus.Success};
            }

            catch (Exception)
            {
                return new AppMessage {IsDone = false, Message = "An error occured.", Status = MessageStatus.Error};
            }
        }

        public void Delete(int BundleId)
        {
            AssessmentBundle item = Context.AssessmentBundles.Find(BundleId);

            if (item != null)
            {
                Context.AssessmentBundles.Remove(item);
                Context.SaveChanges();
            }
        }
    }
}