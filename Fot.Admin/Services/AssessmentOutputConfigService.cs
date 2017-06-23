using System;
using System.Collections.Generic;
using System.Linq;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class AssessmentOutputConfigService : ServiceBase
    {
        public IQueryable<AssessmentOutputConfig> Configs
        {
            get { return Context.AssessmentOutputConfigs; }
        }


        public int? GetQuestionCountByAssessment(int AssessmentId)
        {
            var ret = Configs.Where(x => x.AssessmentId == AssessmentId).Sum(x => (int?)x.NumQuestions);

            return  ret;
        }


        public List<ConfigViewModel> GetConfigs(int AssessmentId)
        {
            return
                Configs.Where(x => x.AssessmentId == AssessmentId).OrderBy(x => x.ConfigId).Select(
                    x => new ConfigViewModel
                        {
                            ConfigId = x.ConfigId,
                            Topic = x.AssessmentTopic.Topic,
                            DifficultyLevel = x.QuestionDifficultyLevel.LevelName,
                            NumQuestions = x.NumQuestions
                        }).ToList();
        }


        public void Delete(int ConfigId)
        {
            AssessmentOutputConfig item = Context.AssessmentOutputConfigs.Find(ConfigId);

            if (item != null)
            {
                Context.AssessmentOutputConfigs.Remove(item);
                Context.SaveChanges();

                new AssessmentService().UpdateQuestionsPerTest(item.AssessmentId, GetQuestionCountByAssessment(item.AssessmentId) ?? 0);

            }
        }


        public bool Exists(AssessmentOutputConfig item)
        {
                  

            return
                Configs.Any(
                    x =>
                    x.AssessmentId == (item.AssessmentId) &&
                    (item.TopicId.HasValue ? x.TopicId == item.TopicId : x.TopicId == null) &&
                    (item.DifficultyLevel.HasValue
                         ? x.DifficultyLevel == item.DifficultyLevel
                         : x.DifficultyLevel == null));
           
        }




        public AppMessage Add(AssessmentOutputConfig item)
        {
            if (Exists(item))
            {
                return new AppMessage
                    {
                        IsDone = false,
                        Message = "An entry with the specified parameters already exists",
                        Status = MessageStatus.Error
                    };
            }

            if (item.NumQuestions <= 0 )
            {
                return new AppMessage
                {
                    IsDone = false,
                    Message = "The specified number of questions is invalid.",
                    Status = MessageStatus.Error
                };
            }

            if(item.NumQuestions > new AssessmentQuestionService().GetTotalCountForTopicAndLevel(item.AssessmentId, item.TopicId, item.DifficultyLevel))
            {
                return new AppMessage
                {
                    IsDone = false,
                    Message = "The specified number of questions exceeds to total possible number of questions.",
                    Status = MessageStatus.Error
                };
            }

            try
            {
                Context.AssessmentOutputConfigs.Add(item);
                Context.SaveChanges();

                new AssessmentService().UpdateQuestionsPerTest(item.AssessmentId, GetQuestionCountByAssessment(item.AssessmentId)?? 0);

                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added entry successfully.",
                        Status = MessageStatus.Success,
                        Data = item.ConfigId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error};
            }
        }
    }
}