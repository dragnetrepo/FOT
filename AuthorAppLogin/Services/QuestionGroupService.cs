using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorApp.Infrastructure;
using AuthorApp.Models;


namespace AuthorApp.Services
{
    public class QuestionGroupService : ServiceBase
    {
        public IQueryable<QuestionGroup> Groups
        {
            get { return Context.QuestionGroups; }
        } 


        public List<QuestionGroup> GetGroups(int AssessmentId)
        {
            return Groups.Where(x => x.AssessmentId == AssessmentId).OrderByDescending(y => y.GroupId).ToList();

        }


        public List<QuestionGroup> GetGroups(int AssessmentId, int startRow, int maxRows)
        {
            return Groups.Where(x => x.AssessmentId == AssessmentId).OrderByDescending(y => y.GroupId).Skip(startRow).Take(maxRows).ToList();

        }


        public int Count(int AssessmentId)
        {
            return Groups.Count(x => x.AssessmentId == AssessmentId);

        }

        public AppMessage Add(QuestionGroup item)
        {
            try
            {
               

                Context.QuestionGroups.Add(item);
                Context.SaveChanges();

            

                return new AppMessage
                {
                    IsDone = true,
                    Message = "Added group successfully.",
                    Status = MessageStatus.Success,
                    Data = item.GroupId
                };


            }

            catch (Exception ex)
            {
                return new AppMessage { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }


        public void Delete(int GroupId)
        {
            var item = Context.QuestionGroups.Find(GroupId);

            if (item != null)
            {
                Context.QuestionGroups.Remove(item);
                Context.SaveChanges();
            }
        }
    }
}