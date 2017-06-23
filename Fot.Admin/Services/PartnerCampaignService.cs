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
    public class PartnerCampaignService : ServiceBase
    {

        public IQueryable<Campaign> Campaigns
        {
            get { return Context.Campaigns; }
        }


     

        public List<CampaignViewModel> GetCampaigns(string searchTerm, int startRow, int maxRows)
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();
                
                IEnumerable<CampaignViewModel> query =
                 Campaigns.Where(x => x.PartnerId == currentAdmin.PartnerId).OrderByDescending(x => x.DateCreated).Select(
                     x => new CampaignViewModel
                     {
                         CampaignId = x.CampaignId,
                         CampaignName = x.CampaignName,
                         PartnerName = x.Partner.PartnerName,
                         CandidateCount = x.CampaignEntries.Count(),
                         CampaignType = x.IsUnproctored ? "Unproctored" : "Proctored"

                     });

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.CampaignName.ToLower().Contains(searchTerm.ToLower()));
            }

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }


        public int Count(string searchTerm)
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Campaigns.Count(x => x.PartnerId == currentAdmin.PartnerId);
            }
            else
            {
                return Campaigns.Count(x => x.PartnerId == currentAdmin.PartnerId && x.CampaignName.ToLower().Contains(searchTerm.ToLower()));
            }
           
            
        }

        public Campaign GetCampaign(int CampaignId)
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            return Campaigns.FirstOrDefault(x => x.CampaignId == CampaignId && x.PartnerId == currentAdmin.PartnerId);
        }



        public CampaignStatsViewModel GetCampaignStats(int CampaignId)
        {
            var list =
                Context.CampaignEntries.Where(x => x.CampaignId == CampaignId).Select(y => new { y.EntryId, y.Scheduled, y.Tested })
                    .ToList();
            var ret = new CampaignStatsViewModel();

            if (list.Count > 0)
            {
                ret.Total = list.Count();
                ret.Scheduled = list.Count(x => x.Scheduled && x.Tested == false);
                ret.Unscheduled = list.Count(x => x.Scheduled == false && x.Tested == false);
            }

            return ret;

        }



        public int GetCandidateCountInLocation(int campaignId, int LocationId)
        {
            return Context.CampaignEntries.Count(x => x.CampaignId == campaignId && (x.Candidate.LocationId == LocationId || x.Candidate.Location.ParentLocation.LocationId == LocationId) && x.Scheduled == false);
        }

        public int GetScheduledCandidateCountInLocation(int campaignId, int LocationId)
        {
            return Context.CampaignEntries.Count(x => x.CampaignId == campaignId && (x.Candidate.LocationId == LocationId || x.Candidate.Location.ParentLocation.LocationId == LocationId) && x.Scheduled && x.Tested == false);
        }

        public AppMessage Add(Campaign item)
        {


            try
            {
                Context.Campaigns.Add(item);
                Context.SaveChanges();


                return new AppMessage
                {
                    IsDone = true,
                    Message = "Added campaign successfully.",
                    Status = MessageStatus.Success,
                    Data = item.PartnerId
                };
            }

            catch (Exception ex)
            {
                return new AppMessage { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }





        public AppMessage Update(Campaign item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Updated campaign successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public void Delete(int CampaignId)
        {
            var item = Context.Campaigns.Find(CampaignId);

            if (item != null)
            {
                Context.Campaigns.Remove(item);
                Context.SaveChanges();
            }
        }


        public bool AnyCandidateTestedOrScheduled(int campaignId)
        {
            return Context.CampaignEntries.Any(x => x.CampaignId == campaignId && (x.Tested || x.Scheduled));

        }

        public List<QuestionsToServeCountViewModel> GetQuestionsToServeCountInBundle(int CampaignId)
        {
            var campaign = GetCampaign(CampaignId);

            return
                Context.AssessmentBundleEntries.Where(x => x.BundleId == campaign.BundleId)
                       .Select(x => new QuestionsToServeCountViewModel
                       {
                           AssessmentId = x.AssessmentId,
                           QuestionsToServe = x.Assessment.QuestionsPerTest == 0 ? x.Assessment.AssessmentQuestions.Count() : x.Assessment.QuestionsPerTest
                       }).ToList();
        }

    }
}