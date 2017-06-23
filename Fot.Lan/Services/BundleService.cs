using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.DTO;
using Fot.Lan.Models;

namespace Fot.Lan.Services
{
    public class BundleService : ServiceBase
    {
        public Bundle GetBundle(int BundleId)
        {
            Bundle bundle = null;

            if (HttpContext.Current.Application[BundleId.ToString()] == null)
            {
                var item = Context.AssessmentPackages.FirstOrDefault(x => x.BundleId == BundleId);

                bundle = FotSecurity<Bundle>.Deserialize(item.BundleData);

                HttpContext.Current.Application[BundleId.ToString()] = bundle;
            }
            else
            {
                bundle = HttpContext.Current.Application[BundleId.ToString()] as Bundle;
            }


            return bundle;
        }


        public AppBundle GetAppBundle(string CandidateGuid, int BundleId)
        {
            if (BundleExists(CandidateGuid))
            {
                return GetSavedBundle(CandidateGuid);
            }


            Bundle bundle = GetBundle(BundleId);

            var item = Context.Candidates.FirstOrDefault(x => x.CandidateGuid == CandidateGuid);

            if (item != null) bundle.ShowFeedback = item.ShowFeedback;


            var appBundle = new AppBundle
                {
                    bundle_id = bundle.BundleId,
                    bundle_name = bundle.Name,
                    bundle_icon = bundle.DescriptionImage,
                    assessments = GetAssessments(bundle.Assessments),
                    save_as_you_go = bundle.SaveAsYouGo,
                    show_feedback = bundle.ShowFeedback,
                    show_results_on_submit = bundle.ShowResultsOnSubmit,
                    current_assessment_index = -1,
                    min_aggregate_score = bundle.MinAggregateScore,
                    allow_assessment_selection = bundle.AllowAssessmentSelection
                };


            if (bundle.SaveAsYouGo)
            {
                SaveBundle(CandidateGuid, appBundle);
            }


            return appBundle;
        }

        public AppBundle GetSavedBundle(string CandidateGuid)
        {
            var savedItem = Context.CandidateAssessments.FirstOrDefault(x => x.CandidateGuid.Equals(CandidateGuid));

            var appBundle = FotSecurity<AppBundle>.Deserialize(savedItem.AssessmentData);

            try
            {
                if (savedItem.CurrentAssessmentState != null)
                {
                    var bundleStatus = FotSecurity<AppBundle>.Deserialize(savedItem.CurrentAssessmentState);

                    appBundle.current_assessment_index = bundleStatus.current_assessment_index;

                    foreach (var item in bundleStatus.assessments)
                    {
                        var temp = appBundle.assessments.First(x => x.assessment_id == item.assessment_id);

                        if (temp != null)
                        {
                            temp.time_remaining = item.time_remaining;
                            temp.current_question_index = item.current_question_index;
                            temp.started = item.started;

                            if (temp.assessment_type.Equals("MCQ"))
                            {
                                foreach (var questionStatus in item.questions)
                                {
                                    var tempQuestion =
                                        temp.questions.First(x => x.question_id == questionStatus.question_id);
                                    if (tempQuestion != null)
                                    {
                                        tempQuestion.seen = questionStatus.seen;
                                        tempQuestion.answered = questionStatus.answered;

                                        foreach (var answerStatus in questionStatus.answers)
                                        {
                                            var tempAnswer =
                                                tempQuestion.answers.First(x => x.answer_id == answerStatus.answer_id);
                                            if (tempAnswer != null)
                                            {
                                                tempAnswer.selected = answerStatus.selected;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (item.essays.Any(x => x.selected))
                                {

                                    var selectedEssay = item.essays.First(y => y.selected);

                                    var tempEssay = temp.essays.First(x => x.essay_id == selectedEssay.essay_id);
                                    if (tempEssay != null)
                                    {
                                        tempEssay.selected = true;
                                        tempEssay.candidate_response = selectedEssay.candidate_response;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return appBundle;
        }


        public void SaveBundle(string CandidateGuid, AppBundle bundle)
        {
            var candidate = Context.Candidates.FirstOrDefault(x => x.CandidateGuid == CandidateGuid);


            var item = new CandidateAssessment
                {
                    CandidateEntryId = candidate.CandidateEntryId,
                    CandidateGuid = CandidateGuid,
                    SaveCount = 1,
                    EntryDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    AssessmentData = FotSecurity<AppBundle>.Serialize(bundle)
                };

            Context.CandidateAssessments.Add(item);
            Context.SaveChanges();
        }


        public void DeleteSavedBundle(string CandidateGuid)
        {
            var item = Context.CandidateAssessments.FirstOrDefault(x => x.CandidateGuid.Equals(CandidateGuid));

            if (item != null)
            {
                Context.CandidateAssessments.Remove(item);
                Context.SaveChanges();
            }
        }

        private bool BundleExists(string CandidateGuid)
        {
            return Context.CandidateAssessments.Any(x => x.CandidateGuid.Equals(CandidateGuid));
        }

        private AppAssessment[] GetAssessments(List<Assessment> list)
        {
            var assessments = new List<AppAssessment>();

            foreach (var item in list)
            {
                var temp = new AppAssessment
                    {
                        assessment_id = item.AssessmentId,
                        assessment_name = item.Name,
                        duration = item.Duration,
                        assessment_type = item.AssessmentType,
                        timed = item.Timed,
                        show_calculator = item.ShowCalculator,
                        instruction_image = item.InstructionImage,
                        essays = GetEssays(item.Essays),
                        questions = item.AdvancedOutputOptions
                                        ? GetQuestionsAdvanced(item.OutputConfigs, item.Questions, item.QuestionsPerTest,
                                                               item.RandomizeQuestions,
                                                               item.RandomizeOptions)
                                        : GetQuestions(item.Questions, item.QuestionsPerTest, item.RandomizeQuestions,
                                                       item.RandomizeOptions),
                        time_remaining = -1,
                        started = false,
                        current_question_index = -1
                    };

                assessments.Add(temp);
            }


            return assessments.ToArray();
        }


        private AppEssay[] GetEssays(List<Essay> listEssays)
        {
            return
                listEssays.Select(
                    x =>
                    new AppEssay
                        {
                            essay_id = x.EssayId,
                            topic = x.Topic,
                            candidate_response = string.Empty,
                            selected = false
                        }).ToArray();
        }

        private AssessmentQuestion[] GetQuestions(List<Question> listQuestions, int QuestionsPerTest,
                                                  bool RandomizeQuestions, bool RandomizeOptions)
        {
            var questions = new List<AssessmentQuestion>();

            var tempListQuestions = RandomizeQuestions
                                        ? listQuestions.OrderBy(x => Guid.NewGuid()).ToList()
                                        : listQuestions; // randomize the list
            tempListQuestions = QuestionsPerTest > 0
                                    ? tempListQuestions.Take(QuestionsPerTest).ToList()
                                    : tempListQuestions; // take required number of questions


            if (QuestionsPerTest == 0)
            {
                tempListQuestions = tempListQuestions.OrderBy(x => x.GroupId).ToList();
                    //order by group_id to be sure grouped questions follow each other sequentially
            }
            else
            {
                tempListQuestions = ProcessGroupedQuestions(tempListQuestions, listQuestions, QuestionsPerTest);
                    //get grouped items not in list and trim as needed.
            }


            tempListQuestions = ReProcessTheList(tempListQuestions);


            foreach (var item in tempListQuestions)
            {
                var temp = new AssessmentQuestion
                    {
                        question_id = item.QuestionId,
                        answer_type = item.AnswerType,
                        options_layout_is_vertical = item.OptionsLayoutIsVertical,
                        item_img = item.QuestionImage,
                        question_text = item.AdditionalText,
                        answers = GetAnswers(item.Options, RandomizeOptions),
                        seen = false,
                        answered = false
                    };

                questions.Add(temp);
            }


            return questions.ToArray();
        }


        private List<Question> ReProcessTheList(List<Question> list)
        {
            var allItems = new List<Question>();


            var noGroup = list.Where(x => x.GroupId == null).ToList();

            noGroup = noGroup.OrderBy(x => Guid.NewGuid()).ToList();


            var groupIds = list.Where(x => x.GroupId != null).Select(x => x.GroupId).Distinct().ToList();

            var listOfList = new List<List<Question>>();

            listOfList.Add(noGroup);

            foreach (var id in groupIds)
            {
                int i = id.Value;

                var temp = list.Where(x => x.GroupId == i).ToList();
                temp = temp.OrderBy(x => Guid.NewGuid()).ToList();

                listOfList.Add(temp);
            }

            listOfList = listOfList.OrderBy(x => Guid.NewGuid()).ToList();

            listOfList.ForEach(allItems.AddRange);


            return allItems;
        }


        //process grouped questions
        public List<Question> ProcessGroupedQuestions(List<Question> tempListQuestions, List<Question> listQuestion,
                                                      int QuestionsPerTest)
        {
            var groupIdList =
                tempListQuestions.Where(x => x.GroupId.HasValue).Select(x => x.GroupId).Distinct().ToList();

            foreach (var i in groupIdList)
            {
                int tempGroupId = i.Value;
                var subList = listQuestion.Where(x => x.GroupId == tempGroupId).ToList();

                subList.ForEach(x =>
                    {
                        if (!tempListQuestions.Contains(x))
                        {
                            tempListQuestions.Add(x);
                        }
                    });
            }

            tempListQuestions = tempListQuestions.OrderBy(x => x.GroupId).ToList();


            tempListQuestions = tempListQuestions.Count > QuestionsPerTest
                                    ? tempListQuestions.Take(QuestionsPerTest).ToList()
                                    : tempListQuestions;


            return tempListQuestions;
        }


        private AssessmentQuestion[] GetQuestionsAdvanced(List<OutputConfig> listRules, List<Question> listQuestions,
                                                          int QuestionsPerTest, bool RandomizeQuestions,
                                                          bool RandomizeOptions)
        {
            if (listRules.Count == 0)
            {
                return GetQuestions(listQuestions, QuestionsPerTest, RandomizeQuestions, RandomizeOptions);
            }


            var questions = new List<AssessmentQuestion>();

            var fullList = ReProcessTheList(listQuestions);// listQuestions.OrderBy(x => Guid.NewGuid()).ToList();
            var tempListQuestions = new List<Question>();

            foreach (var rule in listRules)
            {
                var ret =
                    fullList.Where(
                        x =>
                        (rule.TopicId.HasValue ? x.TopicId == rule.TopicId : x.TopicId == null) &&
                        (rule.DifficultyLevel.HasValue
                             ? x.DifficultyLevel == rule.DifficultyLevel
                             : x.DifficultyLevel == null)).Take(
                                 rule.NumQuestions).ToList();

                tempListQuestions.AddRange(ret);


                foreach (var question in ret)
                {
                    fullList.Remove(question);
                }
            }


            tempListQuestions = RandomizeQuestions
                                    ? tempListQuestions.OrderBy(x => Guid.NewGuid()).ToList()
                                    : tempListQuestions;


            tempListQuestions = tempListQuestions.OrderBy(x => x.GroupId).ToList();
                //order by group_id to be sure grouped questions follow each other sequentially


            tempListQuestions = ReProcessTheList(tempListQuestions);


            foreach (var item in tempListQuestions)
            {
                var temp = new AssessmentQuestion
                    {
                        question_id = item.QuestionId,
                        answer_type = item.AnswerType,
                        options_layout_is_vertical = item.OptionsLayoutIsVertical,
                        item_img = item.QuestionImage,
                        question_text = item.AdditionalText,
                        answers = GetAnswers(item.Options, RandomizeOptions)
                    };

                questions.Add(temp);
            }


            return questions.ToArray();
        }

        private AssessmentAnswer[] GetAnswers(List<Option> listAnswers, bool RandomizeOptions)
        {
            var answers = new List<AssessmentAnswer>();

            listAnswers = RandomizeOptions ? listAnswers.OrderBy(x => Guid.NewGuid()).ToList() : listAnswers;


            foreach (var item in listAnswers)
            {
                var temp = new AssessmentAnswer
                    {
                        answer_id = item.AnswerId,
                        answer_image = item.AnswerImage,
                        answer_text = item.AnswerText,
                        is_image = item.IsImage,
                    };

                answers.Add(temp);
            }


            return answers.ToArray();
        }


        public void SaveState(string CandidateGuid, AppBundle bundleStatus)
        {
            if (BundleExists(CandidateGuid))
            {
                var tempItem =
                    Context.CandidateAssessments.Select(
                        x => new { x.CandidateEntryId, x.CandidateGuid, x.LastUpdated, x.SaveCount })
                           .FirstOrDefault(x => x.CandidateGuid.Equals(CandidateGuid));

                var item = new CandidateAssessment()
                {
                    CandidateEntryId = tempItem.CandidateEntryId,
                    LastUpdated = tempItem.LastUpdated,
                    SaveCount = tempItem.SaveCount,
                };


                Context.CandidateAssessments.Attach(item);


                item.LastUpdated = DateTime.Now;
                item.SaveCount = item.SaveCount + 1;
                item.CurrentAssessmentState = FotSecurity<AppBundle>.Serialize(bundleStatus);


                Context.Entry(item).Property(x => x.LastUpdated).IsModified = true;
                Context.Entry(item).Property(x => x.SaveCount).IsModified = true;
                Context.Entry(item).Property(x => x.CurrentAssessmentState).IsModified = true;


                Context.SaveChanges();
            }
        }
    }
}