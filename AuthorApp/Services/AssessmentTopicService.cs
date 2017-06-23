using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AuthorApp.Infrastructure;
using AuthorApp.Models;


namespace AuthorApp.Services
{
    public class AssessmentTopicService : ServiceBase
    {
        public IQueryable<AssessmentTopic> Topics
        {
            get { return Context.AssessmentTopics; }
        }


        public List<AssessmentTopic> GetTopics(int AssessmentId)
        {
            return Topics.Where(x => x.AssessmentId == AssessmentId).OrderByDescending(x => x.TopicId).ToList();

        }

   



        public AssessmentTopic GetTopic(int TopicId)
        {
            return Topics.FirstOrDefault(x => x.TopicId == TopicId);
        }


        public bool Exists(string Topic, int AssessmentId)
        {
            return Topics.Any(x => x.Topic.Equals(Topic) && x.AssessmentId == AssessmentId);

        }

        public bool ExistsExcept(int assessmentId, string Topic, int TopicId)
        {
            return Context.AssessmentTopics.Any(x => x.AssessmentId == assessmentId && x.Topic.Equals(Topic) && x.TopicId != TopicId);

        }

        public AppMessage Add(AssessmentTopic item)
        {
             if(Exists( item.Topic, item.AssessmentId))
             {
                 return new AppMessage() { IsDone = false, Message = "A topic already exists with specified name.", Status = MessageStatus.Error };
             }
            try
            {
                Context.AssessmentTopics.Add(item);
                Context.SaveChanges();

                return new AppMessage() { IsDone = true, Message = "Added assessment topic successfully.", Status = MessageStatus.Success };
            }

            catch (Exception ex)
            {
                return new AppMessage() { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }


        public AppMessage Update(AssessmentTopic item)
        {
             if(ExistsExcept(item.AssessmentId, item.Topic, item.TopicId))
             {
                 return new AppMessage() { IsDone = false, Message = "A topic already exists with specified name.", Status = MessageStatus.Error };
             }
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage() { IsDone = true, Message = "Updated assessment topic successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage() { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public void Delete(int TopicId)
        {
            var item = Context.AssessmentTopics.Find(TopicId);

            if (item != null)
            {
                Context.AssessmentTopics.Remove(item);
                Context.SaveChanges();
            }
        }



    }
}