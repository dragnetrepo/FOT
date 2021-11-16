using System;
using System.Collections.Generic;

namespace Fot.DTO
{
    [Serializable]
    public class Bundle
    {
        public int BundleId { get; set; }
        public string Name { get; set; }
        public byte[] DescriptionImage { get; set; }
        public bool ShowResultsOnSubmit { get; set; }
        public bool SaveAsYouGo { get; set; }
        public bool ShowFeedback { get; set; }
        public int? MinAggregateScore { get; set; }

        public bool AllowAssessmentSelection { get; set; }

        public string ExternalLink { get; set; }
        public List<Assessment> Assessments { get; set; }
    }

    [Serializable]
    public class Assessment
    {
        public int AssessmentId { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string InstructionText { get; set; }
        public byte[] InstructionImage { get; set; }
        public bool Timed { get; set; }
        public string AssessmentType { get; set; }
        public bool RandomizeQuestions { get; set; }
        public bool RandomizeOptions { get; set; }
        public bool AdvancedOutputOptions { get; set; }
        public int QuestionsPerTest { get; set; }
        public bool ShowCalculator { get; set; }


        public List<OutputConfig> OutputConfigs { get; set; }
        public List<Question> Questions { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Essay> Essays { get; set; }
        public List<Level> Levels { get; set; }
        public List<Group> Groups { get; set; }
    }

    [Serializable]
    public class Question
    {
        public int QuestionId { get; set; }
        public int AssessmentId { get; set; }
        public int? TopicId { get; set; }
        public int? GroupId { get; set; }
        public string QuestionText { get; set; }
        public int? DifficultyLevel { get; set; }
        public byte[] QuestionImage { get; set; }
        public string AdditionalText { get; set; }
        public string AnswerType { get; set; }
        public bool OptionsLayoutIsVertical { get; set; }


        public List<Option> Options { get; set; }
    }

    [Serializable]
    public class Option
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public byte[] AnswerImage { get; set; }
        public bool IsImage { get; set; }
        public bool IsCorrect { get; set; }
    }

    [Serializable]
    public class OutputConfig
    {
        public int ConfigId { get; set; }
        public int AssessmentId { get; set; }
        public int? TopicId { get; set; }
        public int? DifficultyLevel { get; set; }
        public int NumQuestions { get; set; }
    }


    [Serializable]
    public class Topic
    {
        public int TopicId { get; set; }
        public int AssessmentId { get; set; }
        public string TopicText { get; set; }
    }

    [Serializable]
    public class Level
    {
        public int LevelId { get; set; }
        public int AssessmentId { get; set; }
        public string LevelName { get; set; }
        public int LevelWeight { get; set; }
    }

    [Serializable]
    public class Group
    {
        public int GroupId { get; set; }
        public int AssessmentId { get; set; }
        public string GroupName { get; set; }
    }

    [Serializable]
    public class Essay
    {
        public int EssayId { get; set; }
        public int? AssessmentId { get; set; }
        public string Topic { get; set; }
    }
}