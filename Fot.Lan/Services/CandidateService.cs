using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fot.Lan.Models;
using Fot.Lan.Services;


namespace Fot.Lan.Services
{
    public class CandidateService : ServiceBase
    {


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


        public List<CandidateViewModel> GetUncapturedCandidates()
        {
            return Context.Candidates.Where(x => x.CandidatePhoto == null).Select(x => new CandidateViewModel
                {
                    CandidateId = x.CandidateEntryId,
                    Username = x.Username,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname

                }).ToList();
        }


        public void UpdateCandidatePhoto(int candidateId, byte[] imgBytes, int adminId)
        {
            var item = Context.Candidates.FirstOrDefault(x => x.CandidateEntryId == candidateId);

            if (item != null)
            {
                item.CandidatePhoto = imgBytes;
                item.PhotoCapturedByAdminId = adminId;
                Context.SaveChanges();
            }
        }

        public Candidate GetCandidate(string scheduledid)
        {
            return Context.Candidates.FirstOrDefault(x => x.CandidateGuid.Equals(scheduledid));

        }


        public Candidate GetCandidateByUsername(string username)
        {
            return Context.Candidates.FirstOrDefault(x => x.Username.Equals(username));
        }


        public void CandidateStarted(Candidate item)
        {
            if (!item.AssessmentStarted)
            {
                item.AssessmentStarted = true;
                item.DateTimeStarted = DateTime.Now;

                Context.SaveChanges();
            }

        }


        public bool AssessmentBundleExists(int BundleId)
        {
            return Context.AssessmentPackages.Any(x => x.BundleId == BundleId);
        }

        public string AssessmentName(int BundleId)
        {
            var item =
                Context.AssessmentPackages.Where(x => x.BundleId == BundleId).Select(x => x.BundleName).FirstOrDefault();

            return item;
        }
    }
}
