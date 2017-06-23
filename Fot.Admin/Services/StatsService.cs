using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class StatsService : ServiceBase
    {
        public int[] GetAssessmentCount()
        {
            var list =
                Context.Assessments.Where(x => x.OwnerPartnerId.HasValue == false)
                       .Select(x => new {x.AssessmentId, x.AssessmentType})
                       .ToList();

            return new int[]
                {
                    list.Count, //total assessments
                    list.Count(x => x.AssessmentType == AssessmentType.Essay), //essay assessments
                    list.Count(x => x.AssessmentType == AssessmentType.MCQ) // mcq assessments
                };
        }


        public int GetQuestionCount()
        {
            return Context.AssessmentQuestions.Count(x => x.Assessment.OwnerPartnerId.HasValue == false);
        }

        public int GetOptionCount()
        {
            return Context.AssessmentAnswers.Count(x => x.AssessmentQuestion.Assessment.OwnerPartnerId.HasValue == false);
        }


        public int GetCandidateCount()
        {
            return Context.Candidates.Count();
        }


        public int[] GetAdminCount()
        {
            var list = Context.AdminUsers.Select(x => new {x.AdminId, x.IsCenterAdmin, x.IsPartnerAdmin}).ToList();


            return new int[]
                {
                    list.Count, //total admins
                    list.Count(x => x.IsPartnerAdmin == false && x.IsCenterAdmin == false), //regular admins
                    list.Count(x => x.IsPartnerAdmin), //partner admins
                    list.Count(x => x.IsCenterAdmin) // center admins
                };
        }


        public int GetCenterCount()
        {
            return Context.Centers.Count(x => x.IsPrivateCenter == false);
        }

        public int GetPartnerCount()
        {
            return Context.Partners.Count();
        }

        public int GetCampaignCount()
        {
            return Context.Campaigns.Count(x => x.Partner.IsSelfManaged == false);
        }
    }
}