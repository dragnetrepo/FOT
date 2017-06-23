using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Fot.Client.Models;

namespace Fot.Client.Services
{
    public class CandidateService : ServiceBase
    {
        public Candidate GetCandidateByUsername(string username)
        {
            return Context.Candidates.FirstOrDefault(x => x.Username.Equals(username));
        }

        public Candidate GetCandidateById(int CandidateId)
        {
            return Context.Candidates.FirstOrDefault(x => x.CandidateId == CandidateId);
        }


        public List<CandidateAssessmentViewModel> GetCandidateAssessments(int CandidateId)
        {
            var result =
                Context.CampaignEntries.Where(
                    x =>
                    x.CandidateId == CandidateId && x.Campaign.IsUnproctored &&
                    x.Tested == false && x.Campaign.Active && x.Campaign.StartDate <= DateTime.Today && x.Campaign.EndDate >= DateTime.Today)
                       .Select(x => new CandidateAssessmentViewModel
                           {
                               AssessmentName = x.Campaign.AssessmentBundle.Name,
                               CandidateGuid = x.CandidateAssessment.CandidateGuid
                           }).ToList();


            return result;
        }


        public CandidateViewModel GetCandidateByScheduleId(string CandidateGuid)
        {
            return
                Context.CandidateAssessments.Where(x => x.CandidateGuid.Equals(CandidateGuid))
                       .Select(x => new CandidateViewModel
                           {
                               CandidateId = x.CampaignEntry.CandidateId,
                               CandidateGuid = x.CandidateGuid,
                               AssessmentCompleted = x.CampaignEntry.Tested,
                               BundleId = x.CampaignEntry.Campaign.BundleId
                           }).FirstOrDefault();
        }


        public List<CandidateScheduleViewModel> GetCandidateSchedules(int CandidateId)
        {
            var result =
                Context.CampaignEntries.Where(
                    x =>
                    x.Candidate.CandidateId.Equals(CandidateId) && x.Scheduled && x.Tested == false &&
                    x.Campaign.IsUnproctored == false && x.TestSession.TestDate >= DateTime.Today)
                       .Select(x => new CandidateScheduleViewModel()
                           {
                               CampaignEntryId = x.EntryId,
                               SessionId = x.SessionId.Value,
                               CenterName = x.TestSession.Center.CenterName,
                               Address = x.TestSession.Center.Address,
                               Location = x.TestSession.Center.Location.LocationName,
                               TestDate = x.TestSession.TestDate,
                               TimeText = x.TestSession.TimeText,
                               IsPrivateCenter = x.TestSession.Center.IsPrivateCenter
                           }).ToList();

            return result;
        }


        public AppMessage UpdateCandidateSchedule(int CampaignEntryId, int NewSessionId)
        {
            var session = new TestSessionService().GetSessionDetails(NewSessionId);


            if (session.Scheduled < session.Capacity)
            {
                var item = GetCampaignEntry(CampaignEntryId);

                item.SessionId = session.SessionId;
                item.Candidate.LocationId = session.LocationId;


                Context.SaveChanges();

                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Your schedule has been updated successfully.",
                        Status = MessageStatus.Success
                    };
            }

            else
            {
                return new AppMessage
                    {
                        IsDone = false,
                        Message = "Specified session is already full.",
                        Status = MessageStatus.Error
                    };
            }
        }


        public CampaignEntry GetCampaignEntry(int CampaignEntryId)
        {
            return
                Context.CampaignEntries.FirstOrDefault(x => x.EntryId == CampaignEntryId);
        }

        public CandidateScheduleViewModel GetCandidateSchedule(int CampaignEntryId)
        {
            return
                Context.CampaignEntries.Where(x => x.EntryId == CampaignEntryId && x.Scheduled && x.Tested == false &&
                    x.Campaign.IsUnproctored == false && x.TestSession.TestDate >= DateTime.Today)
                       .Select(x => new CandidateScheduleViewModel
                           {
                               CampaignEntryId = x.EntryId,
                               CampaignId = x.CampaignId,
                               SessionId = x.SessionId.Value,
                               CenterName = x.TestSession.Center.CenterName,
                               Address = x.TestSession.Center.Address,
                               Location = x.TestSession.Center.Location.LocationName,
                               TestDate = x.TestSession.TestDate,
                               TimeText = x.TestSession.TimeText,
                               IsPrivateCenter = x.TestSession.Center.IsPrivateCenter
                           }).FirstOrDefault();
        }

        public InvitationViewModel GetCandidateInvitation(int CampaignEntryId)
        {
            return
                Context.CampaignEntries.Where(x => x.EntryId == CampaignEntryId && x.Scheduled && x.Tested == false &&
                    x.Campaign.IsUnproctored == false && x.TestSession.TestDate >= DateTime.Today).Include(y => y.CandidateScheduleResponse)
                       .Select(x => new InvitationViewModel
                       {
                           CandidateId = x.CandidateId,
                           Fullname = x.Candidate.FirstName + " " + x.Candidate.LastName,
                           Username = x.Candidate.Username,
                           Password = x.Candidate.Password,
                           CenterName = x.TestSession.Center.CenterName,
                           Address = x.TestSession.Center.Address,
                           Location = x.TestSession.Center.Location.LocationName,
                           TestDate = x.TestSession.TestDate,
                           TimeText = x.TestSession.TimeText,
                           Instructions = x.Campaign.Instructions,
                           LogoPlacement = x.Campaign.LogoPlacement,
                           LogoFileName = x.Campaign.InvitationLogo,
                           HasResponded = x.CandidateScheduleResponse != null,
                           PhotoFileName = x.Candidate.PhotoFileName
                       }).FirstOrDefault();
        }
    }
}