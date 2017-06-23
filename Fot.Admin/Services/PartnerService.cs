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
    public class PartnerService : ServiceBase
    {
        public IQueryable<Partner> Partners
        {
            get { return Context.Partners; }
        }

     

        public List<Partner> GetPartners(int startRow, int maxRows)
        {
            if (startRow >= 0)
            {
                return Partners.OrderBy(x => x.PartnerName).Skip(startRow).Take(maxRows).ToList();
            }
            else
            {
                return Partners.OrderBy(x => x.PartnerName).ToList();
            }
        } 


        public int Count()
        {
            return Partners.Count();
        }

        public List<Partner> GetNonSelfManagedPartners()
        {
            return Partners.Where(x => x.IsSelfManaged == false).OrderBy(x => x.PartnerName).ToList();
        }

        public List<Partner> GetSelfManagedPartners()
        {
            return Partners.Where(x => x.IsSelfManaged).OrderBy(x => x.PartnerName).ToList();
        } 


        public Partner GetPartner(int PartnerId)
        {
            return Partners.FirstOrDefault(x => x.PartnerId == PartnerId);
        }


        public AppMessage Add(Partner item)
        {
           

            try
            {
                Context.Partners.Add(item);
                Context.SaveChanges();


                return new AppMessage
                {
                    IsDone = true,
                    Message = "Added partner successfully.",
                    Status = MessageStatus.Success,
                    Data = item.PartnerId
                };
            }

            catch (Exception ex)
            {
                return new AppMessage { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }

       

       

        public AppMessage Update(Partner item)
        {
          


            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Updated partner successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public void Delete(int PartnerId)
        {
            var item = Context.Partners.Find(PartnerId);

            if (item != null)
            {
                Context.Partners.Remove(item);
                Context.SaveChanges();
            }
        }
    }
}