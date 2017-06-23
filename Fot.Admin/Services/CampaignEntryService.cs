using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class CampaignEntryService : ServiceBase
    {
        public IQueryable<CampaignEntry> Entries
        {
            get { return Context.CampaignEntries; }
        }


        private readonly Func<CampaignEntry, CampaignCandidateViewModel> CampaignCandidateViewModelSelector =
            x => new CampaignCandidateViewModel
                {
                    EntryId = x.EntryId,
                    Username = x.Candidate.Username,
                    Password = x.Candidate.Password,
                    FirstName = x.Candidate.FirstName,
                    LastName = x.Candidate.LastName,
                    Email = x.Candidate.Email,
                    MobileNo = x.Candidate.MobileNo,
                    LocationName = x.Candidate.Location.LocationName,
                    Scheduled = x.Scheduled,
                    Tested = x.Tested
                };


        public List<CampaignCandidateScheduledViewModel> GetCampaignCandidateScheduledWithCenterInfo(int CampaignId)
        {
            return Entries.Where(x => x.CampaignId == CampaignId && x.Scheduled && x.Tested == false).OrderBy(x => x.EntryId).
                           Include(x => x.Candidate).
                           Include(x => x.Candidate.Location).
                           Include(x => x.TestSession).
                           Include(x => x.TestSession.Center).
                           Select(x => new CampaignCandidateScheduledViewModel
                               {
                                   EntryId = x.EntryId,
                                   Username = x.Candidate.Username,
                                   Password = x.Candidate.Password,
                                   FirstName = x.Candidate.FirstName,
                                   LastName = x.Candidate.LastName,
                                   MobileNo = x.Candidate.MobileNo,
                                   Email = x.Candidate.Email,
                                   LocationName = x.Candidate.Location.LocationName,
                                   CenterName = x.TestSession.Center.CenterName,
                                   TestDate = x.TestSession.TestDate,
                                   TestTime = x.TestSession.TimeText
                               }).ToList();
        } 

        public List<CampaignCandidateViewModel> GetCampaignCandidates(int CampaignId, string searchTerm, int startRow,
                                                                      int maxRows)
        {
            IEnumerable<CampaignCandidateViewModel> query = null;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                
                  query =  Entries.Where(x => x.CampaignId == CampaignId).OrderBy(x => x.EntryId).
                            Include(x => x.Candidate).
                            Include(x => x.Candidate.Location).
                            Select(CampaignCandidateViewModelSelector);
            }
            else
            {
                
                  query =  Entries.Where(
                        x =>
                        x.CampaignId == CampaignId &&
                        (x.Candidate.Username.Equals(searchTerm) || x.Candidate.FirstName.Equals(searchTerm) ||
                         x.Candidate.LastName.Equals(searchTerm) || x.Candidate.MobileNo.Equals(searchTerm)))
                           .OrderBy(x => x.EntryId)
                           .Include(x => x.Candidate).
                            Include(x => x.Candidate.Location).
                            Select(CampaignCandidateViewModelSelector).ToList();
            }

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }


            return query.ToList();
        }

        public int Count(int CampaignId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Entries.Count(x => x.CampaignId == CampaignId);
            }
            else
            {
                return
                    Entries.Count(
                        x =>
                        x.CampaignId == CampaignId &&
                        (x.Candidate.Username.Equals(searchTerm) || x.Candidate.FirstName.Equals(searchTerm) ||
                         x.Candidate.LastName.Equals(searchTerm) || x.Candidate.MobileNo.Equals(searchTerm)));
            }
        }


        public List<CampaignCandidateViewModel> GetCampaignCandidatesUnscheduled(int CampaignId, int LocationId,
                                                                                 int startRow, int maxRows)
        {


             var query =   Entries.Where(
                    x =>
                    x.CampaignId == CampaignId &&
                    (x.Candidate.LocationId == LocationId ||
                     x.Candidate.Location.ParentLocation.LocationId == LocationId) && x.Scheduled == false).OrderBy(
                         x => x.EntryId).
                         Include(x => x.Candidate).Include(x => x.Candidate.Location).
                        Select(CampaignCandidateViewModelSelector);

            if (startRow >= 0)
            {
               query = query.Skip(startRow).Take(maxRows);
            }


            return query.ToList();


        }

        public int CountUnscheduled(int CampaignId, int LocationId)
        {
            return Entries.Count(x =>
                                 x.CampaignId == CampaignId &&
                                 (x.Candidate.LocationId == LocationId ||
                                  x.Candidate.Location.ParentLocation.LocationId == LocationId) && x.Scheduled == false);
        }



        public List<CampaignCandidateViewModel> GetCampaignCandidatesScheduled(int CampaignId, int LocationId,
                                                                               int startRow, int maxRows)
        {
            
               var query = Entries.Where(
                    x =>
                    x.CampaignId == CampaignId &&
                    (x.Candidate.LocationId == LocationId ||
                     x.Candidate.Location.ParentLocation.LocationId == LocationId) && x.Scheduled && x.Tested == false &&
                    x.TestSession.TestDate >= DateTime.Today).
                        OrderBy(
                            x => x.EntryId).
                        Select(CampaignCandidateViewModelSelector);


               if (startRow >= 0)
               {
                   query = query.Skip(startRow).Take(maxRows);
               }


               return query.ToList();
        }

        public int CountScheduled(int CampaignId, int LocationId)
        {
            return Entries.Count(x =>
                                 x.CampaignId == CampaignId &&
                                 (x.Candidate.LocationId == LocationId ||
                                  x.Candidate.Location.ParentLocation.LocationId == LocationId) && x.Scheduled &&
                                 x.Tested == false && x.TestSession.TestDate >= DateTime.Today);
        }


        public void ScheduleCandidates(int CampaignId, int LocationId, int SessionId, int CandidateCount)
        {
            List<CampaignEntry> list =
                Entries.Where(
                    x =>
                    x.CampaignId == CampaignId &&
                    (x.Candidate.LocationId == LocationId ||
                     x.Candidate.Location.ParentLocation.LocationId == LocationId) && x.Scheduled == false).OrderBy(
                         x => x.EntryId).Take(CandidateCount).ToList();

            list.ForEach(x =>
                {
                    x.SessionId = SessionId;
                    x.Scheduled = true;
                }
                );

            Context.SaveChanges();
        }


        public void ScheduleCandidate(int entryId, int sessionId)
        {
            CampaignEntry entry = Context.CampaignEntries.Find(entryId);

            if (entry != null)
            {
                entry.SessionId = sessionId;
                entry.Scheduled = true;

                Context.SaveChanges();
            }
        }


        public bool CandidateExistsInCampaign(int CampaignId, int CandidateId)
        {
            return Entries.Any(x => x.CampaignId == CampaignId && x.CandidateId == CandidateId);
        }

        public AppMessage Add(CampaignEntry item)
        {
            if (CandidateExistsInCampaign(item.CampaignId, item.CandidateId))
            {
                return new AppMessage
                    {
                        IsDone = false,
                        Message = "Specified candidate already exists in this campaign.",
                        Status = MessageStatus.Error
                    };
            }

            try
            {
                Context.CampaignEntries.Add(item);
                Context.SaveChanges();


                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added candidate to campaign successfully.",
                        Status = MessageStatus.Success,
                        Data = item.EntryId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error};
            }
        }

        public AppMessage Update(CampaignEntry item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {IsDone = true, Message = "Updated entry successfully.", Status = MessageStatus.Success};
            }

            catch (Exception)
            {
                return new AppMessage {IsDone = false, Message = "An error occured.", Status = MessageStatus.Error};
            }
        }


        public void Delete(int EntryId)
        {
            CampaignEntry item = Context.CampaignEntries.Find(EntryId);

            if (item != null && (item.Tested == false))
            {
                Context.CampaignEntries.Remove(item);
                Context.SaveChanges();
            }
        }

        public void UnscheduleCandidate(int entryId, int sessionId)
        {
            CampaignEntry entry = Context.CampaignEntries.Find(entryId);

            if (entry != null)
            {
                entry.SessionId = null;
                entry.Scheduled = false;

                Context.SaveChanges();
            }
        }

        public void UnscheduleCandidates(int CampaignId, int sessionId)
        {
            List<CampaignEntry> list =
                Entries.Where(
                    x =>
                    x.CampaignId == CampaignId && x.SessionId == sessionId && x.Scheduled && x.Tested == false).ToList();

            list.ForEach(x =>
                {
                    x.SessionId = null;
                    x.Scheduled = false;
                }
                );

            Context.SaveChanges();
        }


        public int GetUntestedUnscheduledCandidates(int CampaignId)
        {
            return
                Context.CampaignEntries.Count(
                    x =>
                    x.Scheduled && x.Tested == false && x.TestSession.TestDate < DateTime.Today &&
                    x.CampaignId == CampaignId && x.Campaign.IsUnproctored == false);
        }

        public void ResetUntestedScheduledCandidates(int CampaignId)
        {
            var list =
                Context.CampaignEntries.Where(
                    x =>
                    x.Scheduled && x.Tested == false && x.TestSession.TestDate < DateTime.Today &&
                    x.CampaignId == CampaignId && x.Campaign.IsUnproctored == false).ToList();

            if (list.Count <= 0) return;
            list.ForEach(x =>
                {
                    x.SessionId = null;
                    x.Scheduled = false;
                });

            Context.SaveChanges();
        }


        public int GetCandidatesInCampaignCount(Func<CampaignEntry, bool> predicate)
        {
            return Context.CampaignEntries.Count(predicate);
        }


        public List<CandidateMessagingViewModel> GetCandidateMessagingList(Func<CampaignEntry, bool> predicate)
        {
            return
                Context.CampaignEntries.Where(predicate).Select(x => new CandidateMessagingViewModel
                    {
                        Email = x.Candidate.Email,
                        MobileNo = x.Candidate.MobileNo,
                        CampaignEntryId = x.EntryId,
                    }).ToList();
        }

        public void UpdateLocationForUnscheduledCandidates(int campaignId, int selectedLocationId)
        {

            var candidates = Entries.Where( x => x.CampaignId == campaignId && x.Scheduled == false && x.Tested == false)
                                    .Select(x => x.Candidate)
                                    .ToList();

            candidates.ForEach(x => { x.LocationId = selectedLocationId; });

            Context.SaveChanges();



        }


        public List<CandidateResponseViewModel> GetCandidatesResponses(int CampaignId, int startRow,
                                                                      int maxRows)
        {
            IEnumerable<CandidateResponseViewModel> query = null;

           

                query = Entries.Where(x => x.CampaignId == CampaignId && x.CandidateScheduleResponse != null).OrderBy(x => x.EntryId).
                          Include(x => x.Candidate).
                          Include(x => x.Candidate.Location).
                          Include(x => x.CandidateScheduleResponse).
                          Select(x => new CandidateResponseViewModel
                          {
                              EntryId = x.EntryId,
                              Username = x.Candidate.Username,
                              UniqueID = x.Candidate.ClientUniqueID,
                              FirstName = x.Candidate.FirstName,
                              LastName = x.Candidate.LastName,
                              Email = x.Candidate.Email,
                              MobileNo = x.Candidate.MobileNo,
                              LocationName = x.Candidate.Location.LocationName,
                              Accepted = x.CandidateScheduleResponse.AcceptSchedule,
                              RejectReason = x.CandidateScheduleResponse.RejectReason,
                              DateResponded = x.CandidateScheduleResponse.DateResponded                         
                          });
            
        

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }


            return query.ToList();
        }


        public List<CandidateResponseDownloadViewModel> GetCandidatesResponsesForDownload(int CampaignId)
        {
            IEnumerable<CandidateResponseDownloadViewModel> query = null;



            query = Entries.Where(x => x.CampaignId == CampaignId).OrderBy(x => x.EntryId).
                      Include(x => x.Candidate).
                      Include(x => x.Candidate.Location).
                      Include(x => x.CandidateScheduleResponse).
                      Select(x => new CandidateResponseDownloadViewModel
                      {
                          EntryId = x.EntryId,
                          Username = x.Candidate.Username,
                          UniqueID = x.Candidate.ClientUniqueID,
                          FirstName = x.Candidate.FirstName,
                          LastName = x.Candidate.LastName,
                          Email = x.Candidate.Email,
                          MobileNo = x.Candidate.MobileNo,
                          LocationName = x.Candidate.Location.LocationName,
                          Response = x.CandidateScheduleResponse
 
                      });


            return query.ToList();
        }

        public int CountCandidatesResponses(int CampaignId)
        {

            return Entries.Count(x => x.CampaignId == CampaignId && x.CandidateScheduleResponse != null);

        }

    }
}