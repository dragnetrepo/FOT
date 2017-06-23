using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class TestScheduleService : ServiceBase
    {

        public List<TestScheduleViewModel> GetTestSchedules(int CenterId)
        {
            var list =
                Context.CampaignEntries.Where(
                    x =>
                    x.TestSession.TestDate == DateTime.Today && x.TestSession.CenterId == CenterId && x.Scheduled &&
                    x.Tested == false).
                    Select(x => new TestScheduleViewModel
                        {
                            CandidateId = x.CandidateId,
                            Username = x.Candidate.Username,
                            Password = x.Candidate.Password,
                            Firstname = x.Candidate.FirstName,
                            Lastname = x.Candidate.LastName,
                            MobileNo = x.Candidate.MobileNo,
                            BundleId = x.Campaign.BundleId,
                            CampaignId = x.CampaignId,
                            ShowFeedback = x.Campaign.ShowFeedback,
                            SessionId = x.SessionId.Value
                        }).ToList();

            var distinctList = new List<TestScheduleViewModel>();

            foreach (var item in list.Where(item => distinctList.All(x => x.CandidateId != item.CandidateId)))
            {
                distinctList.Add(item);
            }

            return distinctList;
        }


        public List<AssessmentBundleViewModel> GetAssessmentsRequiredForCenter(int CenterId)
        {
            var list =
                Context.CampaignEntries.Where(
                    x =>
                    x.TestSession.TestDate == DateTime.Today && x.TestSession.CenterId == CenterId && x.Scheduled &&
                    x.Tested == false).
                    Select(x => new AssessmentBundleViewModel
                    {
                        BundleId = x.Campaign.BundleId,
                        Name = x.Campaign.AssessmentBundle.Name
                    }).Distinct().ToList();

            return list;
        }



        public List<DailyScheduleViewModel> GetTodaysSchedule(DateTime date)
        {
            var list =
                Context.Centers.Where(
                    x => x.TestSessions.Any(t => t.TestDate.Equals(date) && t.CampaignEntries.Count > 0)).
                    Select(x => new DailyScheduleViewModel
                        {
                            CenterId = x.CenterId,
                            CenterName = x.CenterName,
                            LocationName = x.Location.LocationName,
                            TotalScheduled = x.TestSessions.Where(y => y.TestDate.Equals(date)).Sum(y => y.CampaignEntries.Count),
                            TotalTested = x.TestSessions.Where(y => y.TestDate.Equals(date)).Sum(y => y.CampaignEntries.Count(q => q.Tested)),
                            DownloadedSchedule = x.ScheduleDownloads.Any(s => s.EntryDate.Equals(date) && s.Downloaded),
                            TriggeredEndOfDay = x.ScheduleDownloads.Any(s => s.EntryDate.Equals(date) && s.EndOfDayTriggered)
                        }).ToList();


            return list;
        }

        public List<SupportStaffViewModel> GetSupportStaff(int centerId)
        {

            var staff = new List<SupportStaffViewModel>();



            var temp1 = Context.AdminUsers.Where(x => x.CenterId == centerId && x.Active)
                .Select(x => new SupportStaffViewModel
                {
                    UserId = x.AdminId,
                    Username = x.Username,
                    Password = x.Password,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    IsCaptureAdmin = x.IsCaptureAdmin,
                    IsSupportStaff = false
                }).ToList();


           
           var temp2 = Context.CenterUsers.Where(x => x.CenterId == centerId && x.Active)
                .Select(x => new SupportStaffViewModel
                {
                    UserId = x.UserId,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    IsCaptureAdmin = false,
                    IsSupportStaff = true
                }).ToList();
           

            staff.AddRange(temp1);
            staff.AddRange(temp2);

            return staff;
        }
    }
}