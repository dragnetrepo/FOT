using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class PartnerWalletEntryService : ServiceBase
    {
      

        public List<PartnerWalletEntryViewModel> GetWalletEntries(int PartnerId, int startRow, int maxRows)
        {
            
             IEnumerable<PartnerWalletEntryViewModel> query =   Context.PartnerWalletEntries.Where(x => x.PartnerId == PartnerId)
                       .Select(x => new PartnerWalletEntryViewModel
                           {
                               EntryId = x.EntryId,
                               Amount = x.Amount,
                               Reference = x.Reference,
                               EntryDate = x.EntryDate,
                               EntryAdmin = x.AdminUser.Username
                           }).OrderByDescending(x => x.EntryId);

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }

        public int Count(int PartnerId)
        {
            return Context.PartnerWalletEntries.Count(x => x.PartnerId == PartnerId);
        }

        public AppMessage Add(PartnerWalletEntry item)
        {


            try
            {
                var partner = Context.Partners.FirstOrDefault(x => x.PartnerId == item.PartnerId);

                Context.PartnerWalletEntries.Add(item);

                partner.WalletBalance = partner.WalletBalance + item.Amount;

                Context.SaveChanges();
           

                return new AppMessage
                {
                    IsDone = true,
                    Message = "Added wallet deposit successfully.",
                    Status = MessageStatus.Success,
                    Data = item.EntryId
                };
            }

            catch (Exception ex)
            {
                return new AppMessage { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }

        public void Delete(int EntryId)
        {
            var item = Context.PartnerWalletEntries.Find(EntryId);

            var partner = Context.Partners.FirstOrDefault(x => x.PartnerId == item.PartnerId);

            var amountScheduled = new PartnerWalletScheduleService().GetTotalAmountScheduled(item.PartnerId);


            if (item != null && partner.WalletBalance >= (item.Amount + amountScheduled))
            {
                

                partner.WalletBalance = partner.WalletBalance - item.Amount;
                Context.PartnerWalletEntries.Remove(item);
                
                Context.SaveChanges();
            }
        }
    }
}