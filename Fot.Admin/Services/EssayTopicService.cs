﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class EssayTopicService : ServiceBase
    {

        public IQueryable<EssayTopic> EssayTopics
        {
            get { return Context.EssayTopics; }
        }


      

        public List<EssayTopic> GetTopics(int AssessmentId, int startRow, int maxRows)
        {
            new AuthorAssignedAssessmentService().CheckAuthorAccess(AssessmentId);

            IEnumerable<EssayTopic> query = EssayTopics.Where(x => x.AssessmentId == AssessmentId).OrderByDescending(x => x.EssayId);

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }


        public int Count(int AssessmentId)
        {
            return  EssayTopics.Count(x => x.AssessmentId == AssessmentId);
            
        }



        public EssayTopic GetTopic(int EssayId)
        {
            var essay =  EssayTopics.FirstOrDefault(x => x.EssayId == EssayId);

            if (essay != null)
            {
                //new AuthorAssignedAssessmentService().CheckAuthorAccess(essay.AssessmentId.Value);
            }

            return essay;
        }

        public AppMessage Add(EssayTopic item)
        {
       
            try
            {
                Context.EssayTopics.Add(item);
                Context.SaveChanges();

                return new AppMessage() { IsDone = true, Message = "Added essay topic successfully.", Status = MessageStatus.Success };
            }

            catch (Exception ex)
            {
                return new AppMessage() { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }


        public AppMessage Update(EssayTopic item)
        {
          
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage() { IsDone = true, Message = "Updated essay topic successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage() { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public void Delete(int EssayId)
        {
            var item = Context.EssayTopics.Find(EssayId);

            if (item != null)
            {
                Context.EssayTopics.Remove(item);
                Context.SaveChanges();
            }
        }
    }
}