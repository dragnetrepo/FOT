using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorApp.Models;
using Fot.DTO;
using Assessment = Fot.DTO.Assessment;

namespace AuthorApp.Services
{
    public class DTOService : ServiceBase
    {

        

        public byte[] AssessmentBytes(int id)
        {

            var assessment = GetAssessment(id);

            return FotSecurity<Assessment>.Serialize(assessment);

        }


        public Assessment GetAssessment(int Id)
        {
          

            var result = Context.Assessments.FirstOrDefault(x => x.AssessmentId == Id);

           
                

                var temp = new Assessment
                    {
                        AssessmentId = result.AssessmentId,
                        Name = result.Name,
                        Duration = result.Duration,
                        InstructionText = result.InstructionText,
                        InstructionImage = result.InstructionImage,
                        Timed = result.Timed,
                        AssessmentType = result.AssessmentType == AssessmentType.MCQ ? "MCQ" : "ESSAY",
                        RandomizeQuestions = result.RandomizeQuestions,
                        RandomizeOptions = result.RandomizeOptions,
                        QuestionsPerTest = result.QuestionsPerTest,
                        AdvancedOutputOptions = result.AdvancedOutputOptions,
                        Questions = GetQuestions(result.AssessmentId),
                        Topics = GetTopics(result.AssessmentId),
                        Levels = GetLevels(result.AssessmentId),
                        Groups = GetGroups(result.AssessmentId)


                   };

              
            

            return temp;
        } 


        public List<Question> GetQuestions(int AssessmentId)
        {
            var list = new List<Question>();

            var result = Context.AssessmentQuestions.Where(x => x.AssessmentId == AssessmentId).ToList();

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
                    QuestionText = x.QuestionText,
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