using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using AuthorApp.Infrastructure;
using AuthorApp.Models;


namespace AuthorApp.Services
{
    public class LimitedQuestionViewModel
    {
        public int QuestionId { get; set; }
        public int AssessmentId { get; set; }
        public int? TopicId { get; set; }
        public int? DifficultyLevel { get; set; }
    }

    public class AssessmentQuestionService : ServiceBase
    {
        public IQueryable<AssessmentQuestion> Questions
        {
            get { return Context.AssessmentQuestions; }
        }


        public int QuestionCountByAssessment(int AssessmentId)
        {
            return Questions.Count(x => x.AssessmentId == AssessmentId);
        }


        public AssessmentQuestion GetQuestion(int QuestionId)
        {
            return Questions.FirstOrDefault(x => x.QuestionId == QuestionId);
        }


        public List<QuestionViewModel> GetQuestions(int AssessmentId)
        {
            return Questions.Where(x => x.AssessmentId == AssessmentId).Select(x => new QuestionViewModel
                {
                    QuestionId = x.QuestionId,
                    AnswerType = x.AnswerType,
                    OptionCount = x.AssessmentAnswers.Count(),
                    QuestionImage = x.QuestionImage,
                    Approved = x.Approved,
                    ValidQuestion = x.AssessmentAnswers.Count() > 1 && x.AssessmentAnswers.Any(y => y.IsCorrect)
                }).ToList();
        }


        public byte[] GetQuestionImage(int QuestionId)
        {
            AssessmentQuestion item = GetQuestion(QuestionId);

            return item == null ? null : item.QuestionImage;
        }

        public AppMessage Add(AssessmentQuestion item)
        {
            try
            {
                Context.AssessmentQuestions.Add(item);
                Context.SaveChanges();

                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added question successfully.",
                        Status = MessageStatus.Success,
                        Data = item.QuestionId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error};
            }
        }


        public AppMessage Update(AssessmentQuestion item)
        {
       
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {IsDone = true, Message = "Updated question successfully.", Status = MessageStatus.Success};
            }

            catch (Exception)
            {
                return new AppMessage {IsDone = false, Message = "An error occured.", Status = MessageStatus.Error};
            }
        }

        public void Delete(int QuestionId)
        {
            AssessmentQuestion item = Context.AssessmentQuestions.Find(QuestionId);

            if (item != null)
            {
                Context.AssessmentQuestions.Remove(item);
                Context.SaveChanges();
            }
        }

        public void ResetAllAnswersIfMoreThanOne(int QuestionId)
        {
            List<AssessmentAnswer> list =
                Context.AssessmentAnswers.Where(x => x.QuestionId == QuestionId && x.IsCorrect).ToList();

            if (list.Count > 0)
            {
                list.ForEach(x => x.IsCorrect = false);

                Context.SaveChanges();
            }
        }

        public int GetTotalCountForTopicAndLevel(int assessmentId, int? topicId, int? levelId)
        {
            return
                Questions.Count(
                    x =>
                    x.AssessmentId == assessmentId && (topicId.HasValue ? x.TopicId == topicId : x.TopicId == null) &&
                    (levelId.HasValue ? x.DifficultyLevel == levelId : x.DifficultyLevel == null));
        }


       

       

        public int GetLocalTotalCountForTopicAndLevel(List<LimitedQuestionViewModel> questionList, int assessmentId, int? topicId, int? levelId)
        {
            return
                questionList.Count(
                    x =>
                    x.AssessmentId == assessmentId && (topicId.HasValue ? x.TopicId == topicId : x.TopicId == null) &&
                    (levelId.HasValue ? x.DifficultyLevel == levelId : x.DifficultyLevel == null));
        }
    }
}