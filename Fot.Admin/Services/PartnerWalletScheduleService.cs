using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class PartnerWalletScheduleService : ServiceBase
    {

        public decimal GetTotalAmountScheduled(int PartnerId)
        {
            var partner = Context.Partners.Find(PartnerId);

            var scheduledList = GetWalletScheduled(PartnerId);

            decimal total = 0.0m;

            scheduledList.ForEach(x =>
                {
                    total += x.IsPrivateCenter || x.IsUnProctored
                                 ? partner.CostPerTestPrivate.Value
                                 : partner.CostPerTestPublic.Value;
                });



            return total;
        }



        public List<WalletScheduledViewModel> GetWalletScheduled(int PartnerId)
        {

            return
                Context.CampaignEntries.Where(
                    x =>
                    x.Campaign.PartnerId == PartnerId && (x.Scheduled || x.Campaign.IsUnproctored) && x.Tested == false)
                       .Select(x => new WalletScheduledViewModel
                           {
                               EntryId = x.EntryId,
                               IsPrivateCenter = x.SessionId.HasValue ? x.TestSession.Center.IsPrivateCenter : false,
                               IsUnProctored = x.Campaign.IsUnproctored
                           }).ToList();
        }
    }
}