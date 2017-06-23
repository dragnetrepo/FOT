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
    public class PartnerAssessmentQuestionService : ServiceBase
    {
        public IQueryable<AssessmentQuestion> Questions
        {
            get { return Context.AssessmentQuestions; }
        }


        public int QuestionCountByAssessment(int AssessmentId, int ValidEntry)
        {
            if (ValidEntry == 0)
            {
                return Questions.Count(x => x.AssessmentId == AssessmentId);
            }
            else
            {
                if (ValidEntry == 1)
                {
                    return Questions.Count(x => x.AssessmentId == AssessmentId && (x.AssessmentAnswers.Count() > 1 && x.AssessmentAnswers.Any(y => y.IsCorrect)));
                }
                else
                {
                    return Questions.Count(x => x.AssessmentId == AssessmentId && (x.AssessmentAnswers.Count() > 1 && x.AssessmentAnswers.Any(y => y.IsCorrect)) == false);
                }
            }
        }



        public AssessmentQuestion GetQuestion(int QuestionId)
        {
            var question = Questions.FirstOrDefault(x => x.QuestionId == QuestionId);

            if (question != null)
            {
                CheckPartnerAccess(question.AssessmentId);
            }

            return question;
        }


        public List<QuestionViewModel> GetQuestions(int AssessmentId, int ValidEntry, int startRow, int maxRows)
        {
            CheckPartnerAccess(AssessmentId);

            IEnumerable<QuestionViewModel> query =
                Questions.Where(x => x.AssessmentId == AssessmentId).Select(x => new QuestionViewModel
                    {
                        QuestionId = x.QuestionId,
                        AnswerType = x.AnswerType,
                        OptionCount = x.AssessmentAnswers.Count(),
                        QuestionImage = x.QuestionImage,
                        ValidQuestion = x.AssessmentAnswers.Count() > 1 && x.AssessmentAnswers.Any(y => y.IsCorrect)
                    }).OrderByDescending(x => x.QuestionId);


            if (ValidEntry > 0)
            {
                if (ValidEntry == 1)
                {
                    query = query.Where(x => x.ValidQuestion);
                }
                else if (ValidEntry == 2)
                {
                    query = query.Where(x => x.ValidQuestion == false);
                }
            }




            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
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
                    {
                        IsDone = false,
                        Message = "An error occured." + ex.Message,
                        Status = MessageStatus.Error
                    };
            }
        }


        public AppMessage Update(AssessmentQuestion item)
        {
            if (!IsValidAfterUpdate(item.QuestionId, item.TopicId, item.DifficultyLevel))
            {
                return new AppMessage
                    {
                        IsDone = false,
                        Message =
                            "Updating this question would invalidate a config entry in the <strong>Advanced Retrieval Options</strong> page.",
                        Status = MessageStatus.Error
                    };
            }
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Updated question successfully.",
                        Status = MessageStatus.Success
                    };
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


        public bool IsValidAfterDelete(int QuestionId)
        {
            AssessmentQuestion item =
                Questions.Where(x => x.QuestionId == QuestionId).Include(x => x.Assessment).FirstOrDefault();


            Assessment assessment = item.Assessment;

            if (!assessment.AdvancedOutputOptions)
            {
                int total = QuestionCountByAssessment(assessment.AssessmentId, 0);

                if (total <= assessment.QuestionsPerTest)
                {
                    return false;
                }

                return true;
            }
            else
            {
                List<LimitedQuestionViewModel> questionList =
                    Questions.Where(x => x.AssessmentId == assessment.AssessmentId).Select(
                        x =>
                        new LimitedQuestionViewModel
                            {
                                QuestionId = x.QuestionId,
                                AssessmentId = x.AssessmentId,
                                TopicId = x.TopicId,
                                DifficultyLevel = x.DifficultyLevel
                            }).ToList();

                questionList.Remove(questionList.First(x => x.QuestionId == QuestionId));


                var configList =
                    Context.AssessmentOutputConfigs.Where(x => x.AssessmentId == assessment.AssessmentId).ToList();


                foreach (var config in configList)
                {
                    var temp = config;
                    if (
                        GetLocalTotalCountForTopicAndLevel(questionList, temp.AssessmentId, temp.TopicId,
                                                           temp.DifficultyLevel) < temp.NumQuestions)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool IsValidAfterUpdate(int QuestionId, int? topicId, int? levelId)
        {
            AssessmentQuestion item =
                Questions.Where(x => x.QuestionId == QuestionId).Include(x => x.Assessment).FirstOrDefault();


            Assessment assessment = item.Assessment;

            if (assessment.AdvancedOutputOptions)
            {
                List<LimitedQuestionViewModel> questionList =
                    Questions.Where(x => x.AssessmentId == assessment.AssessmentId).Select(
                        x =>
                        new LimitedQuestionViewModel
                            {
                                QuestionId = x.QuestionId,
                                AssessmentId = x.AssessmentId,
                                TopicId = x.TopicId,
                                DifficultyLevel = x.DifficultyLevel
                            }).ToList();

                questionList.First(x => x.QuestionId == QuestionId).TopicId = topicId;
                questionList.First(x => x.QuestionId == QuestionId).DifficultyLevel = levelId;


                var configList =
                    Context.AssessmentOutputConfigs.Where(x => x.AssessmentId == assessment.AssessmentId).ToList();


                foreach (var config in configList)
                {
                    var temp = config;
                    if (
                        GetLocalTotalCountForTopicAndLevel(questionList, temp.AssessmentId, temp.TopicId,
                                                           temp.DifficultyLevel) < temp.NumQuestions)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        public int GetLocalTotalCountForTopicAndLevel(List<LimitedQuestionViewModel> questionList, int assessmentId,
                                                      int? topicId, int? levelId)
        {
            return
                questionList.Count(
                    x =>
                    x.AssessmentId == assessmentId && (topicId.HasValue ? x.TopicId == topicId : x.TopicId == null) &&
                    (levelId.HasValue ? x.DifficultyLevel == levelId : x.DifficultyLevel == null));
        }

        public void CheckPartnerAccess(int assesmentId)
        {
            new PartnerAuthorAssignedAssessmentService().CheckAuthorAccess(assesmentId);
        }

        public List<AssessmentQuestion> GetQuestionsAndOptions(int AssessmentId)
        {
            return
                Context.AssessmentQuestions.Where(x => x.AssessmentId == AssessmentId)
                       .Include(x => x.AssessmentAnswers)
                       .ToList();
        }
    }
}