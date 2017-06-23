using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Author)]
    public partial class ImportAssessment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttnImport_Click(object sender, EventArgs e)
        {
            ImportFile();
        }

       

        private void ImportFile()
        {
           


            if(byteFile.HasFile)
            {
                try
                {
                    var bytes = byteFile.FileBytes;

                    var item = Fot.DTO.FotSecurity<Fot.DTO.Assessment>.Deserialize(bytes);


                    var assessment = new Assessment();

                    assessment.Name = item.Name;
                    assessment.Duration = item.Duration;
                    assessment.InstructionText = item.InstructionText;
                    assessment.InstructionImage = item.InstructionImage;
                    assessment.Timed = item.Timed;
                    assessment.AssessmentType = AssessmentType.MCQ;
                    assessment.RandomizeQuestions = item.RandomizeQuestions;
                    assessment.RandomizeOptions = item.RandomizeOptions;
                    assessment.AdvancedOutputOptions = false;
                    assessment.QuestionsPerTest = 0;
                    assessment.DateAdded = DateTime.Today;
                    assessment.DateLastUpdated = DateTime.Today;

                    foreach (var topic in item.Topics)
                    {
                        assessment.AssessmentTopics.Add(new AssessmentTopic{Topic = topic.TopicText});
                    }

                    foreach (var level in item.Levels)
                    {
                        assessment.QuestionDifficultyLevels.Add(new QuestionDifficultyLevel{LevelName = level.LevelName, LevelWeight = level.LevelWeight});
                    }

                    foreach (var group in item.Groups)
                    {
                        assessment.QuestionGroups.Add(new QuestionGroup{GroupName = group.GroupName});
                    }

                    foreach (var question in item.Questions)
                    {
                        var temp = new AssessmentQuestion();
                        temp.QuestionText = question.QuestionText;
                        temp.QuestionImage = question.QuestionImage;
                        temp.AdditionalText = question.AdditionalText;
                        temp.AnswerType = question.AnswerType;
                        temp.OptionsLayoutIsVertical = question.OptionsLayoutIsVertical;

                        if (question.TopicId.HasValue)
                        {
                            string topicText = item.Topics.First(x => x.TopicId == question.TopicId).TopicText;

                            temp.AssessmentTopic =
                                assessment.AssessmentTopics.FirstOrDefault(x => x.Topic.Equals(topicText));

                        }

                        if (question.DifficultyLevel.HasValue)
                        {
                            string levelName = item.Levels.First(x => x.LevelId == question.DifficultyLevel).LevelName;

                            temp.QuestionDifficultyLevel =
                                assessment.QuestionDifficultyLevels.FirstOrDefault(x => x.LevelName.Equals(levelName));

                        }

                        if (question.GroupId.HasValue)
                        {
                            string groupName = item.Groups.First(x => x.GroupId == question.GroupId).GroupName;

                            temp.QuestionGroup =
                                assessment.QuestionGroups.FirstOrDefault(x => x.GroupName.Equals(groupName));

                        }

                        foreach (var answer in question.Options)
                        {
                            var tempAnswer = new AssessmentAnswer();

                            tempAnswer.AnswerText = answer.AnswerText;
                            tempAnswer.AnswerImage = answer.AnswerImage;
                            tempAnswer.IsCorrect = answer.IsCorrect;
                            tempAnswer.IsImage = answer.IsImage;

                            temp.AssessmentAnswers.Add(tempAnswer);
                        }

                        assessment.AssessmentQuestions.Add(temp);



                      

                    }


                    var app = new AssessmentService().Add(assessment);

                    lblStatus.ShowMessage(app);



                }
                catch(Exception ex)
                {
                    lblStatus.ShowMessage(new AppMessage { Message = "An error occured during importation of file.", Status = MessageStatus.Error, IsDone = false });
                }
                
            }
            else
            {
                lblStatus.ShowMessage(new AppMessage{Message = "No file selected", Status = MessageStatus.Error, IsDone = false});
            }
        }
    }
}