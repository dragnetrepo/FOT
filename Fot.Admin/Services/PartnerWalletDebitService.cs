using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;
using System.Data.Entity;

namespace Fot.Admin.Services
{
    public class PartnerWalletDebitService : ServiceBase
    {
        public List<WalletDebitEntryViewModel> GetDebitEntries(int PartnerId, int startRow, int maxRows)
        {
            IEnumerable<WalletDebitEntryViewModel> query =
                Context.PartnerWalletDebits.Where(x => x.PartnerId == PartnerId)
                       .Include(x => x.CampaignEntry.Campaign.CampaignName)
                       .Include(x => x.CampaignEntry.Candidate.FirstName)
                       .Include(x => x.CampaignEntry.Candidate.LastName)
                       .Select(x => new WalletDebitEntryViewModel
                           {
                               EntryId = x.EntryId,
                               CampaignName = x.CampaignEntry.Campaign.CampaignName,
                               CandidateName =
                                   x.CampaignEntry.Candidate.FirstName + " " + x.CampaignEntry.Candidate.LastName,
                               Amount = x.Amount.Value,
                               DebitDate = x.DebitDate.Value
                           }).OrderByDescending(x => x.EntryId);


            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }


        public int Count(int PartnerId)
        {
            return Context.PartnerWalletDebits.Count(x => x.PartnerId == PartnerId);
        }
    }
}