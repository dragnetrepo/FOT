using System;
using System.Collections.Generic;
using System.Linq;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class PartnerAssignedAssessmentService : ServiceBase
    {
        public List<AssessmentBundleEntryViewModel> GetAssignedAssessments(int PartnerId, int startRow, int maxRows)
        {
            IEnumerable<AssessmentBundleEntryViewModel> query = Context.PartnerAssignedAssessments.Where(
                x => x.PartnerId == PartnerId).Select(
                    x =>
                    new AssessmentBundleEntryViewModel
                        {
                            EntryId = x.EntryId,
                            Name = x.Assessment.Name,
                            AssessmentType = x.Assessment.AssessmentType
                        }).OrderByDescending(x => x.EntryId);

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }


        public int Count(int PartnerId)
        {
            return Context.PartnerAssignedAssessments.Count(x => x.PartnerId == PartnerId);
        }

        public List<AssessmentViewModel> GetUnassignedAssessments(int PartnerId)
        {
            return
                Context.Assessments.Where(x => x.PartnerAssignedAssessments.Any(y => y.PartnerId == PartnerId) == false)
                       .Select(x => new AssessmentViewModel
                           {
                               AssessmentId = x.AssessmentId,
                               Name = x.Name
                           }).ToList();
        }

        public AppMessage AssignAssessmentToPartner(int assessmentId, int partnerId)
        {
            try
            {
                var item = new PartnerAssignedAssessment {PartnerId = partnerId, AssessmentId = assessmentId};

                Context.PartnerAssignedAssessments.Add(item);
                Context.SaveChanges();

                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added entry successfully.",
                        Status = MessageStatus.Success
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {
                        IsDone = false,
                        Message = "An error occured." + ex.Message,
                        Status = MessageStatus.Error
                    };
            }
        }

        public void DeleteAssessmentFromPartner(int EntryId)
        {
            PartnerAssignedAssessment item = Context.PartnerAssignedAssessments.Find(EntryId);

            if (item != null)
            {
                Context.PartnerAssignedAssessments.Remove(item);
                Context.SaveChanges();
            }
        }
    }
}