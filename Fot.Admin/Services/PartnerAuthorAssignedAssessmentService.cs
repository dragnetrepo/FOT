using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class PartnerAuthorAssignedAssessmentService : ServiceBase
    {

     

        public List<AssessmentBundleEntryViewModel> GetAssignedAssessments(int AdminId, int startRow, int maxRows)
        {

            IEnumerable<AssessmentBundleEntryViewModel> query = null;

            
              query =   Context.AuthorAssignedAssessments.Where(x => x.AdminId == AdminId).Select(
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


        public int Count(int AdminId)
        {
            return Context.AuthorAssignedAssessments.Count(x => x.AdminId == AdminId);
        }

        public List<AssessmentViewModel> GetUnassignedAssessments(int AdminId)
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            return
                Context.Assessments.Where(x => x.OwnerPartnerId == currentAdmin.PartnerId && x.AuthorAssignedAssessments.Any(y => y.AdminId == AdminId) == false)
                       .Select(x => new AssessmentViewModel
                       {
                           AssessmentId = x.AssessmentId,
                           Name = x.Name
                       }).ToList();
        }

        public AppMessage AssignAssessmentToAuthor(int assessmentId, int adminId)
        {
            try
            {
                var item = new AuthorAssignedAssessment { AdminId = adminId, AssessmentId = assessmentId };

                Context.AuthorAssignedAssessments.Add(item);
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

        public void DeleteAssessmentFromAuthor(int EntryId)
        {
            AuthorAssignedAssessment item = Context.AuthorAssignedAssessments.Find(EntryId);

            if (item != null)
            {
                Context.AuthorAssignedAssessments.Remove(item);
                Context.SaveChanges();
            }
        }


        public void CheckAuthorAccess(int assesmentId)
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            if (!currentAdmin.HasUsersAccess)
            {
                bool valid =
                    Context.AuthorAssignedAssessments.Any(
                        x => x.AssessmentId == assesmentId && x.AdminId == currentAdmin.AdminId);

                if (!valid || !currentAdmin.CanAuthor)
                {
                    HttpContext.Current.Response.Redirect(UrlMapper.Assessments);
                }
            }
        }
    }
}