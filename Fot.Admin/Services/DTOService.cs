using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;
using Fot.DTO;
using Assessment = Fot.DTO.Assessment;

namespace Fot.Admin.Services
{
    public class DTOService : ServiceBase
    {

        public bool AssessmentsInBundleAreValid(int BundleId)
        {
            var result = Context.AssessmentBundleEntries.Where(x => x.BundleId == BundleId).Select(x => new AssessmentViewModel
            {
                AssessmentId = x.AssessmentId,
                IsValid = x.Assessment.AssessmentType == AssessmentType.MCQ ? x.Assessment.AssessmentQuestions.Any() && !x.Assessment.AssessmentQuestions.Any(y => y.AssessmentAnswers.Count() < 2 || !y.AssessmentAnswers.Any(t => t.IsCorrect)) : x.Assessment.EssayTopics.Any()
            }).ToList();


            return result.All(x => x.IsValid);

        }

        public BundlePackage GetBundlePackage(int BundleId)
        {
            if (AssessmentsInBundleAreValid(BundleId))
            {
                var bundle = GetBundle(BundleId);

                if (bundle != null)
                {
                    var package = new BundlePackage
                        {
                            BundleId = bundle.BundleId,
                            BundleName = bundle.Name,
                            BundleData = FotSecurity<Bundle>.Serialize(bundle),
                            IsDone = true
                        };

                    return package;
                }
                else
                {
                    return new BundlePackage
                        {
                            IsDone = false,
                            ErrorMessage = "Requested assessment package does not exist."

                        };
                }
            }
            else
            {
                return new BundlePackage
                {
                    IsDone = false,
                    ErrorMessage = "The specified Assessment Bundle contains invalid assessments. Contact the administrator to fix the affected assessments."

                };
            }
        }

        public Bundle GetBundle(int BundleId)
        {

            var item = Context.AssessmentBundles.FirstOrDefault(x => x.BundleId == BundleId);

            if (item == null) return null;

            var bundle = new Bundle
                {
                    BundleId = BundleId,
                    Name = item.Name,
                    DescriptionImage = item.DescriptionImage,
                    ShowResultsOnSubmit = item.ShowResultsOnSubmit,
                    SaveAsYouGo = item.SaveAsYouGo,
                    MinAggregateScore = item.MinAggregatePassScore,
                    AllowAssessmentSelection = item.AllowAssessmentSelection,
                    Assessments = GetAssessments(BundleId)
                };

            return bundle;


        }


        public List<Assessment> GetAssessments(int BundleId)
        {
            var list = new List<Assessment>();

            var result = Context.AssessmentBundleEntries.Where(x => x.BundleId == BundleId).Select(x => x.Assessment).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new Assessment
                    {
                        AssessmentId = x.AssessmentId,
                        Name = x.Name,
                        Duration = x.Duration,
                        InstructionImage = x.InstructionImage,
                        Timed = x.Timed,
                        AssessmentType = x.AssessmentType == AssessmentType.MCQ ? "MCQ" : "ESSAY",
                        RandomizeQuestions = x.RandomizeQuestions,
                        RandomizeOptions = x.RandomizeOptions,
                        QuestionsPerTest = x.QuestionsPerTest,
                        ShowCalculator = x.ShowCalculator,
                        AdvancedOutputOptions = x.AdvancedOutputOptions,
                        OutputConfigs = GetConfigs(x.AssessmentId),
                        Questions = x.HasFixedQuestions ? GetFixedQuestions(x.AssessmentId) : GetQuestions(x.AssessmentId),
                        Topics = GetTopics(x.AssessmentId),
                        Essays = GetEssays(x.AssessmentId),
                        Levels = GetLevels(x.AssessmentId),
                        Groups = GetGroups(x.AssessmentId)


                   };

                list.Add(temp);
            }

            return list;
        } 


        public List<Question> GetQuestions(int AssessmentId)
        {
            var list = new List<Question>();

            var result = Context.AssessmentQuestions.Where(x => x.AssessmentId == AssessmentId && x.Retired == false).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new Question()
                {
                    QuestionId = x.QuestionId,
                    AssessmentId = x.AssessmentId,
                    TopicId = x.TopicId,
                    GroupId = x.GroupId,
                    DifficultyLevel = x.DifficultyLevel,
                    QuestionImage = x.QuestionImage,
                    AdditionalText = x.AdditionalText,
                    AnswerType = x.AnswerType,
                    OptionsLayoutIsVertical = x.OptionsLayoutIsVertical,

                    Options = GetOptions(x.QuestionId)

                };

                list.Add(temp);
            }

            return list;
        }

        public List<Question> GetFixedQuestions(int AssessmentId)
        {
            var list = new List<Question>();

            var result = Context.FixedQuestions.Where(x => x.AssessmentId == AssessmentId && x.AssessmentQuestion.Retired == false).Select(x => x.AssessmentQuestion).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new Question()
                {
                    QuestionId = x.QuestionId,
                    AssessmentId = x.AssessmentId,
                    TopicId = x.TopicId,
                    GroupId = x.GroupId,
                    DifficultyLevel = x.DifficultyLevel,
                    QuestionImage = x.QuestionImage,
                    AdditionalText = x.AdditionalText,
                    AnswerType = x.AnswerType,
                    OptionsLayoutIsVertical = x.OptionsLayoutIsVertical,

                    Options = GetOptions(x.QuestionId)

                };

                list.Add(temp);
            }


            if(!list.Any()) return GetQuestions(AssessmentId);

            return list;
        }


        public List<Option> GetOptions(int QuestionId)
        {

            var list = new List<Option>();

            var result = Context.AssessmentAnswers.Where(x => x.QuestionId == QuestionId).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new Option()
                {
                    AnswerId = x.AnswerId,
                    QuestionId = x.QuestionId,
                    AnswerText = x.AnswerText,
                    AnswerImage = x.AnswerImage,
                    IsImage = x.IsImage,
                    IsCorrect = x.IsCorrect           

                };

                list.Add(temp);
            }

            return list;
        } 


        public List<OutputConfig> GetConfigs(int AssessmentId)
        {
            var list = new List<OutputConfig>();

            var result = Context.AssessmentOutputConfigs.Where(x => x.AssessmentId == AssessmentId).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new OutputConfig
                {
                    ConfigId = x.ConfigId,
                    AssessmentId = x.AssessmentId,
                    TopicId = x.TopicId,
                    DifficultyLevel = x.DifficultyLevel,
                    NumQuestions = x.NumQuestions
                };

                list.Add(temp);
            }

            return list;
        } 


        public List<Essay> GetEssays(int AssessmentId)
        {
            var list = new List<Essay>();

            var result = Context.EssayTopics.Where(x => x.AssessmentId == AssessmentId).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new Essay()
                {
                    EssayId = x.EssayId,
                    AssessmentId = x.AssessmentId,
                    Topic = x.Topic
                
                };

                list.Add(temp);
            }

            return list;
        } 

        public List<Topic> GetTopics(int AssessmentId)
        {
            var list = new List<Topic>();

            var result = Context.AssessmentTopics.Where(x => x.AssessmentId == AssessmentId).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new Topic()
                {
                    TopicId = x.TopicId,
                    AssessmentId = x.AssessmentId,
                    TopicText = x.Topic

                };

                list.Add(temp);
            }

            return list;
        } 


        public List<Level> GetLevels(int AssessmentId)
        {
            var list = new List<Level>();

            var result = Context.QuestionDifficultyLevels.Where(x => x.AssessmentId == AssessmentId).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new Level()
                {
                    LevelId = x.LevelId,
                    AssessmentId = x.AssessmentId,
                    LevelName = x.LevelName,
                    LevelWeight = x.LevelWeight

                };

                list.Add(temp);
            }

            return list;
        } 

        public List<Group> GetGroups(int AssessmentId)
        {
            var list = new List<Group>();

            var result = Context.QuestionGroups.Where(x => x.AssessmentId == AssessmentId).ToList();

            foreach (var item in result)
            {
                var x = item;

                var temp = new Group()
                {
                    GroupId = x.GroupId,
                    AssessmentId = x.AssessmentId,
                    GroupName = x.GroupName
                    
                };

                list.Add(temp);
            }

            return list;
        } 
    }
}