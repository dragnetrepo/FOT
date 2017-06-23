using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class AssessmentAnswerService : ServiceBase
    {
        public IQueryable<AssessmentAnswer> Answers
        {
            get { return Context.AssessmentAnswers; }
        } 

        public int AnswerCountByQuestion(int QuestionId)
        {
            return Answers.Count(x => x.QuestionId == QuestionId);

        }


        public List<AnswerViewModel> GetAnswers(int QuestionId)
        {
            return Answers.Where(x => x.QuestionId == QuestionId).Select(y => new AnswerViewModel
                {
                    AnswerId = y.AnswerId,
                    AnswerText = y.AnswerText,
                    AnswerImage = y.AnswerImage,
                    AnswerType = y.AssessmentQuestion.AnswerType,
                    IsCorrect = y.IsCorrect,
                    IsImage = y.IsImage    
                }).ToList();

        } 


       

        public AssessmentAnswer GetAnswer(int AnswerId)
        {
            return Answers.FirstOrDefault(x => x.AnswerId == AnswerId);
        }

        public byte[] GetAnswerImage(int AnswserId)
        {
            var item = GetAnswer(AnswserId);

            if(item != null && item.IsImage)
            {
                return item.AnswerImage;
            }
            else
            {
                return null;
            }
        }

        public AppMessage Add(AssessmentAnswer item)
        {
            try
            {
                bool isCorrect = item.IsCorrect;
                item.IsCorrect = false;

                Context.AssessmentAnswers.Add(item);
                Context.SaveChanges();

                if (isCorrect)
                {
                    SetAsCorrectAnswer(item.AnswerId, item.QuestionId);
                }

                return new AppMessage
                {
                    IsDone = true,
                    Message = "Added option successfully.",
                    Status = MessageStatus.Success,
                    Data = item.AnswerId
                };

                
            }

            catch (Exception ex)
            {
                return new AppMessage { IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error };
            }
        }

   

        public void SetAsCorrectAnswer(int AnswerId, int QuestionId)
        {
            var question = new AssessmentQuestionService().GetQuestion(QuestionId);

            if(question.AnswerType.Equals("Single"))
            {
                var list = Context.AssessmentAnswers.Where(x => x.QuestionId == QuestionId && x.IsCorrect).ToList();
                list.ForEach(x => x.IsCorrect = false);

                var item = Answers.FirstOrDefault(x => x.AnswerId == AnswerId);

                if(item != null)
                {
                    item.IsCorrect = true;

                    Context.SaveChanges();
                }

                

            }
            else
            {
                var item = Answers.FirstOrDefault(x => x.AnswerId == AnswerId);

                if (item != null)
                {
                    item.IsCorrect = true;

                    Context.SaveChanges();
                }

               
            }
        }


        public void UnsetAsCorrectAnswer(int AnswerId)
        {

                var item = Answers.FirstOrDefault(x => x.AnswerId == AnswerId);

                if (item != null)
                {
                    item.IsCorrect = false;

                    Context.SaveChanges();
                }

                
            
        }


        public AppMessage Update(AssessmentAnswer item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Updated option successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public void Delete(int AnswerId)
        {
            var item = Context.AssessmentAnswers.Find(AnswerId);

            if (item != null)
            {
                Context.AssessmentAnswers.Remove(item);
                Context.SaveChanges();
            }
        }
    }
}