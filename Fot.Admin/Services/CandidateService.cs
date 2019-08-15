using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class CandidateService : ServiceBase
    {
        

        public List<CandidateViewModel> GetCandidates(string searchTerm, int startRow, int maxRows)
        {
            IEnumerable<CandidateViewModel> query = null;


            if (string.IsNullOrWhiteSpace(searchTerm))
            {

                
                query =  Context.Candidates.Select(
                        x => new CandidateViewModel
                            {
                                CandidateId = x.CandidateId,
                                UserName = x.Username,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                MobileNo = x.MobileNo,
                                Email = x.Email,
                                Location = x.Location.LocationName,
                                //CampaignCount = x.CampaignEntries.Count
                            });
            }
            else
            {
                
                 query =  Context.Candidates.Where(x => x.Username.Equals(searchTerm) || x.FirstName.Equals(searchTerm) || x.LastName.Equals(searchTerm) || x.Email.Equals(searchTerm)).Select(
                       x => new CandidateViewModel
                       {
                           CandidateId = x.CandidateId,
                           UserName = x.Username,
                           FirstName = x.FirstName,
                           LastName = x.LastName,
                           MobileNo = x.MobileNo,
                           Email = x.Email,
                           Location = x.Location.LocationName,
                          // CampaignCount = x.CampaignEntries.Count
                       });
            }

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }

        public int Count(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {

                return Context.Candidates.Count();

            }
            else
            {
                return Context.Candidates.Count(x => x.Username.Equals(searchTerm) || x.FirstName.Equals(searchTerm) || x.LastName.Equals(searchTerm) || x.Email.Equals(searchTerm));
                
            }
        }


        public bool Exists(string email)
        {
            return Context.Candidates.Any(x => x.Username.Equals(email));
        }

        public Candidate GetCandidate(string email)
        {
            return Context.Candidates.FirstOrDefault(x => x.Username.Equals(email));
        }

        public Candidate GetCandidateById(int CandidateId)
        {
            return Context.Candidates.Find(CandidateId);
        }

        public AppMessage Add(Candidate item)
        {

            try
            {
                Context.Candidates.Add(item);
                Context.SaveChanges();


                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added candidate successfully.",
                        Status = MessageStatus.Success,
                        Data = item.CandidateId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {IsDone = false, Message = "An error occurred." + ex.Message, Status = MessageStatus.Error};
            }
        }

        public AppMessage Update(Candidate item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {IsDone = true, Message = "Updated candidate successfully.", Status = MessageStatus.Success};
            }

            catch (Exception)
            {
                return new AppMessage {IsDone = false, Message = "An error occured.", Status = MessageStatus.Error};
            }
        }

        public bool IsInCampaign(int CandidateId)
        {
            return Context.Candidates.Any(x => x.CandidateId == CandidateId && x.CampaignEntries.Count > 0);

        }

        public void Delete(int CandidateId)
        {
            if(IsInCampaign(CandidateId)) return;

            Candidate item = Context.Candidates.Find(CandidateId);

            if (item != null)
            {
                Context.Candidates.Remove(item);
                Context.SaveChanges();
            }
        }



 

        public List<CandidateCampaignViewModel> GetCandidateCampaigns(int CandidateId, int startRow, int maxRows)
        {
            
             IEnumerable<CandidateCampaignViewModel> query =  Context.CampaignEntries.Where(x => x.CandidateId == CandidateId)
                       .Select(x => new CandidateCampaignViewModel
                       {
                           EntryId = x.EntryId,
                           CampaignId = x.CampaignId,
                           CampaignName = x.Campaign.CampaignName,
                           Scheduled = x.Scheduled,
                           Tested = x.Tested,
                           DateTested = x.DateTested
                       }).OrderByDescending(x => x.EntryId);

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        } 

        public int CountCandidateCampaigns(int CandidateId)
        {
            return Context.CampaignEntries.Count(x => x.CandidateId == CandidateId);
        }


    }
}