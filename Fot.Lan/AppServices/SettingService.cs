using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fot.Lan.Models;
using Fot.Lan.Services;


namespace Fot.Lan.AppServices
{
    public class SettingService : ServiceBase
    {

        public bool GetImageCapatureSetting()
        {
            var item = Context.Settings.FirstOrDefault(x => x.SettingName.Equals("IMAGE_CAPTURE"));

            if(item != null)
            {
                return Convert.ToBoolean(item.SettingValue);
            }
            else
            {
                return false;
            }
        }

        public void SetImageCaptureSetting(bool flag)
        {
            var item = Context.Settings.FirstOrDefault(x => x.SettingName.Equals("IMAGE_CAPTURE"));

            if(item != null)
            {
                item.SettingValue = flag.ToString();

                Context.SaveChanges();

            }
        }


        public string GetCaptureAdminUsername()
        {
            var item = Context.Settings.FirstOrDefault(x => x.SettingName.Equals("CAPTURE_ADMIN_USERNAME"));

            if (item != null) return item.SettingValue;

            return string.Empty;
        }

        public string GetCaptureAdminPassword()
        {
            var item = Context.Settings.FirstOrDefault(x => x.SettingName.Equals("CAPTURE_ADMIN_PASSWORD"));

            if (item != null) return item.SettingValue;

            return string.Empty;
        }

        public void UpdateAdminUsernameAndPassword(string username, string password)
        {
            var item = Context.Settings.FirstOrDefault(x => x.SettingName.Equals("CAPTURE_ADMIN_USERNAME"));

            item.SettingValue = username;

            var item2 = Context.Settings.FirstOrDefault(x => x.SettingName.Equals("CAPTURE_ADMIN_PASSWORD"));

            item2.SettingValue = password;

            Context.SaveChanges();


        }



        public List<AdminUserViewModel> GetStaffList()
        {
            return Context.AdminUsers.Select(x => new AdminUserViewModel
            {
                ActualUserId = x.ActualUserId,
                IsSupportStaff = x.IsSupportStaff,
                DownloadDate = x.DownloadDate,
                PreTestPhoto = x.PreTestPhoto,
                PostTestPhoto = x.PostTestPhoto,
                Synchronized = x.Synchronized,
                PreTestCapturedByAdminId = x.PreTestCapturedByAdminId.HasValue ? x.PreTestCaptureAdmin.ActualUserId : default(int?),
                PostTestCapturedByAdminId = x.PostTestCapturedByAdminId.HasValue ? x.PostTestCaptureAdmin.ActualUserId : default(int?)


            }).ToList();
        }


        public List<AdminUserViewModel> GetPendingStaffList()
        {
            return Context.AdminUsers.Where(x => x.PostTestPhoto != null && x.Synchronized == false).Select(x => new AdminUserViewModel
            {
                ActualUserId = x.ActualUserId,
                IsSupportStaff = x.IsSupportStaff,
                DownloadDate = x.DownloadDate,
                PreTestPhoto = x.PreTestPhoto,
                PostTestPhoto = x.PostTestPhoto,
                Synchronized = x.Synchronized,
                PreTestCapturedByAdminId = x.PreTestCapturedByAdminId.HasValue ? x.PreTestCaptureAdmin.ActualUserId : default(int?),
                PostTestCapturedByAdminId = x.PostTestCapturedByAdminId.HasValue ? x.PostTestCaptureAdmin.ActualUserId : default(int?)


            }).ToList();
        }


        public void SynchronizeStaffList(List<int> list)
        {
            var items = Context.AdminUsers.Where(x => list.Contains(x.ActualUserId)).ToList();

            items.ForEach(x => x.Synchronized = true);

            Context.SaveChanges();

        }


        public List<TestedViewModel> GetTestedList()
        {
            var items =
                Context.Candidates.Where(
                    x => x.AssessmentCompleted && x.Synchronized == false && x.AssessmentResults.Any())
                    .Select(x => new TestedViewModel
                    {
                        CandidateEntryId = x.CandidateEntryId,
                        CandidateId = x.CandidateId,
                        CampaignId = x.CampaignId,
                        SessionId = x.SessionId,
                        CandidatePhoto = x.CandidatePhoto,
                        DateTimeStarted = x.DateTimeStarted.Value,
                        DateTimeCompleted = x.DateTimeCompleted.Value,
                        PhotoCapturedByAdminId = x.PhotoCapturedByAdminId.HasValue ? x.AdminUser.ActualUserId : default(int?),
                        AssessmentResults = x.AssessmentResults.Select(y => new AssessmentResultViewModel
                        {
                            EntryId = y.EntryId,
                            CandidateEntryId = y.CandidateEntryId,
                            AssessmentId = y.AssessmentId,
                            Score = y.Score,
                            CandidateOptions = y.CandidateOptions,
                            SelectedEssayId = y.SelectedEssayId,
                            EssayText = y.EssayText
                            
                        }).ToList(),
                        TestFeedback = x.TestFeedback != null ? new TestFeedbackViewModel
                        {
                            Directions = x.TestFeedback.Directions,
                            WaitTime = x.TestFeedback.WaitTime,
                            Professionalism = x.TestFeedback.Professionalism,
                            StartTime = x.TestFeedback.StartTime,
                            Briefing = x.TestFeedback.Briefing,
                            Registration = x.TestFeedback.Registration,
                            Overall = x.TestFeedback.Overall,
                            UnsatisfactoryArea = x.TestFeedback.UnsatisfactoryArea,
                            SatisfactoryArea = x.TestFeedback.SatisfactoryArea,
                            Comments = x.TestFeedback.Comments

                        } : null

                    }).ToList();

            return items;
        }

        public List<PhotoLog> GetPhotoLogs()
        {
            var items = Context.PhotoLogs.ToList();

            return items;
        }

        public void SynchronizeCandidate(int entryId)
        {
            var sql = "update Candidate set Synchronized = 1 where CandidateEntryId = {0}";


            Context.Database.ExecuteSqlCommand(sql, entryId);
        }
    }
}
