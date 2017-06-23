using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fot.Lan.Models;
using Fot.Lan.Services;


namespace Fot.Lan.AppServices
{
    public class CandidateService : ServiceBase
    {

        public IQueryable<Candidate> Candidates
        {
            get { return Context.Candidates; }
        }

        public List<CandidateDetailsViewModel> GetCandidates()
        {

            return Candidates.Select(x => new CandidateDetailsViewModel
                {
                    CandidateEntryId = x.CandidateEntryId,
                    CandidateId = x.CandidateId,
                    Username = x.Username,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    DateTimeStarted = x.DateTimeStarted,
                    DateTimeCompleted = x.DateTimeCompleted,
                    AssessmentStarted = x.AssessmentStarted,
                    AssessmentCompleted = x.AssessmentCompleted
                }).ToList();
        }


        public List<CandidateDetailsViewModel> GetCandidatesForSummary()
        {

            return Candidates.Select(x => new CandidateDetailsViewModel
            {
                CandidateEntryId = x.CandidateEntryId,
                DateTimeStarted = x.DateTimeStarted,
                DateTimeCompleted = x.DateTimeCompleted,
                AssessmentStarted = x.AssessmentStarted,
                AssessmentCompleted = x.AssessmentCompleted,
                Synchronized = x.Synchronized
            }).ToList();
        }


        public bool AnyExists()
        {
            return Candidates.Any();

        }



        public void Add(Candidate item)
        {

            Context.Candidates.Add(item);
            Context.SaveChanges();



        }


        public bool Exists(Candidate item)
        {
            return Context.Candidates.Any(x => x.CandidateId == item.CandidateId);

        }

        public void Add(List<Candidate> itemList)
        {

            var tempList = new List<Candidate>();

            itemList.ForEach(x =>
                {
                    if (!Exists(x))
                        tempList.Add(x);
                });

            Context.Candidates.AddRange(tempList);

            Context.SaveChanges();

        }

        public void Delete(int CandidateEntryId)
        {
            var item = Context.Candidates.Find(CandidateEntryId);

            if (item != null)
            {
                Context.Candidates.Remove(item);
                Context.SaveChanges();
            }
        }

        public void DeleteAll(DateTime currentDate)
        {


            Context.Database.ExecuteSqlCommand("delete from Candidate where DownloadDate < {0}", currentDate);
            Context.Database.ExecuteSqlCommand("delete from AssessmentPackage where DownloadDate < {0}", currentDate);
            Context.Database.ExecuteSqlCommand("delete from AdminUser where DownloadDate < {0}", currentDate);
            Context.Database.ExecuteSqlCommand("delete from AdminUser where Synchronized = 1");

        }



        public void DeleteEverything()
        {
            Context.Database.ExecuteSqlCommand("delete from TestFeedback");
            Context.Database.ExecuteSqlCommand("delete from RequiredAssessment");
            Context.Database.ExecuteSqlCommand("delete from AssessmentPackage");
            Context.Database.ExecuteSqlCommand("delete from AssessmentResult");
            Context.Database.ExecuteSqlCommand("delete from CandidateAssessment");
            Context.Database.ExecuteSqlCommand("delete from Candidate");
            Context.Database.ExecuteSqlCommand("delete from PhotoLog");

        }




        public bool CanTriggerEndOfDay()
        {
            var list = GetCandidatesForSummary();



            int completed = list.Count(x => x.AssessmentCompleted);
            int synchronized = list.Count(x => x.Synchronized);

            if (completed == 0) return false;


            return completed == synchronized;
        }


        public bool LoginIsValid(string username, string password)
        {
            var item = Context.Candidates.FirstOrDefault(x => x.Username.Equals(username));

            if (item != null)
            {
                if (item.Password.Equals(password))
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public bool BundleExists(int BundleId)
        {
            return Context.AssessmentPackages.Any(x => x.BundleId == BundleId) || Context.RequiredAssessments.Any(x => x.BundleId == BundleId);
        }

        public List<RequiredAssessment> GetRequiredAssessmentList(List<RequiredAssessment> list)
        {

            var tempList = list.ToList();

            foreach (var i in list)
            {
                if (BundleExists(i.BundleId))
                {
                    tempList.Remove(i);
                }
            }

            return tempList;
        }

        public void AddRequiredAssessments(List<RequiredAssessment> realList)
        {
            realList.ForEach(x => Context.RequiredAssessments.Add(new RequiredAssessment { BundleId = x.BundleId, Name = x.Name }));

            Context.SaveChanges();
        }


        public List<RequiredAssessment> GetRequiredAssessments()
        {
            return Context.RequiredAssessments.ToList();
        }

        public void SaveBundle(AssessmentPackage assessmentPackage)
        {
            Context.AssessmentPackages.Add(assessmentPackage);

            var item = Context.RequiredAssessments.FirstOrDefault(x => x.BundleId == assessmentPackage.BundleId);

            if (item != null)
            {
                Context.RequiredAssessments.Remove(item);
            }



            Context.SaveChanges();
        }


        public bool StaffExists(List<AdminUser> list, int staffId)
        {

            return list.Any(x => x.ActualUserId == staffId);
        }

        public void AddStaff(List<AdminUser> staffList)
        {
            var tempList = new List<AdminUser>();

            var currentStaffList = Context.AdminUsers.AsNoTracking().ToList();

            staffList.ForEach(x =>
            {
                if (!StaffExists(currentStaffList, x.ActualUserId))
                    tempList.Add(x);
            });

            Context.AdminUsers.AddRange(tempList);

            Context.SaveChanges();
        }
    }
}
