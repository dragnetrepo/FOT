using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using AuthorApp.Infrastructure;
using AuthorApp.Models;
using AuthorApp.Services;
using System.Data.Entity;

namespace AuthorApp.Services
{
    public class AssessmentService : ServiceBase
    {
        public IQueryable<Assessment> Assessments
        {
            get { return Context.Assessments; }
        }


        public List<AssessmentViewModel> GetAssessments()
        {
            return Context.Assessments.Where(x => x.AuthorAdminId == MainWindow.CurrentUser.AdminId).OrderByDescending(x => x.AssessmentId).Select(x => new AssessmentViewModel
                {
                    AssessmentId = x.AssessmentId,
                    Name = x.Name,
                    AssessmentType = x.AssessmentType,
                    QuestionCount = x.AssessmentType == AssessmentType.MCQ ? x.AssessmentQuestions.Count() : x.AssessmentTopics.Count(),
                    DateAdded = x.DateAdded,
                    LastUpdated = x.DateLastUpdated.Value
                }).ToList();
        }

        public List<AssessmentViewModel> GetAdminAssessments()
        {
            return Assessments.OrderByDescending(x => x.AssessmentId).Select(x => new AssessmentViewModel
            {
                AssessmentId = x.AssessmentId,
                Name = x.Name,
                Author = x.AdminUser.Username,
                AssessmentType = x.AssessmentType,
                QuestionCount = x.AssessmentType == AssessmentType.MCQ ? x.AssessmentQuestions.Count() : x.AssessmentTopics.Count(),
                DateAdded = x.DateAdded,
                LastUpdated = x.DateLastUpdated.Value
            }).ToList();
        }



        public Assessment GetAssessmentForPreview(int AssessmentId)
        {
            Context.Configuration.LazyLoadingEnabled = false;
            var res = Context.Assessments.FirstOrDefault(x => x.AssessmentId == AssessmentId);

            Context.Configuration.LazyLoadingEnabled = true;

            return res;

        }


        public Assessment GetAssessment(int AssessmentId)
        {
            return Assessments.FirstOrDefault(x => x.AssessmentId == AssessmentId);
        }

        public Assessment GetAssessmentAndAttributes(int AssessmentId)
        {
            return Assessments.Include(x => x.AssessmentTopics).Include(x => x.QuestionDifficultyLevels).Include(x => x.AssessmentQuestions).Include(x => x.QuestionGroups).FirstOrDefault(x => x.AssessmentId == AssessmentId);
        }

        public byte[] GetInstructionImage(int AssessmentId)
        {
            var item = GetAssessment(AssessmentId);

            return item == null ? null : item.InstructionImage;
        }


        public AppMessage Add(Assessment item)
        {
            try
            {
                Context.Assessments.Add(item);
                Context.SaveChanges();

                
                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added assessment successfully.",
                        Status = MessageStatus.Success,
                        Data = item.AssessmentId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error};
            }
        }


      

        public AppMessage Update(Assessment item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {IsDone = true, Message = "Updated assessment successfully.", Status = MessageStatus.Success};
            }

            catch (Exception)
            {
                return new AppMessage {IsDone = false, Message = "An error occured.", Status = MessageStatus.Error};
            }
        }

        public void Delete(int AssessmentId)
        {
            Assessment item = Context.Assessments.Find(AssessmentId);
            
            if (item != null)
            {
                Context.Assessments.Remove(item);
                Context.SaveChanges();

             
            }
        }


    }
}