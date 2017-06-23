using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.DTO;
using Fot.Lan.Models;

namespace Fot.Lan.Services
{
    public class AssessmentService : ServiceBase
    {

        public ResultResponse AssessmentSubmitted(string ScheduledId, AssessmentResponse[] responses)
        {
            
            

            var item = Context.Candidates.FirstOrDefault(x => x.CandidateGuid.Equals(ScheduledId));

            if (item != null)
            {
                try
                {
                    if (item.AssessmentCompleted) return new ResultResponse{Succeeded = false};


                    item.DateTimeCompleted = DateTime.Now;
                    item.AssessmentCompleted = true;

                    int totalScore = 0;

                    var result = new List<ResultModel>();

                    foreach (var response in responses)
                    {


                        if (response.is_essay)
                        {
                            


                            item.AssessmentResults.Add(new AssessmentResult
                            {
                                AssessmentId = response.assessment_id,
                                SelectedEssayId = response.essay_id,
                                EssayText = response.result
                            }); 
                        }
                        else
                        {
                            int score = ComputeScore(item.BundleId, response.result);
                            totalScore += score;

                            int questionCount = response.result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Length;


                            item.AssessmentResults.Add(new AssessmentResult
                            {
                                AssessmentId = response.assessment_id,
                                Score = Convert.ToBase64String(FotSecurity<string>.Serialize(score.ToString())),
                                CandidateOptions = response.result
                            }); 

                           result.Add(new ResultModel{AssessmentName = GetAssessmentName(item.BundleId,response.assessment_id), Score = score, TotalQuestions = questionCount});
                        }


                    }


                    Context.SaveChanges();

                    new BundleService().DeleteSavedBundle(ScheduledId);

                    return new ResultResponse { Succeeded = true, resultList = result};
                }
                catch(Exception ex)
                {
                    return new ResultResponse { Succeeded = false };
                }

            }
            else
            {
                return new ResultResponse { Succeeded = false };
            }


            
        }


        public string GetAssessmentName(int bundle_id, int assessmentId)
        {
            Bundle bundle = new BundleService().GetBundle(bundle_id);

            return bundle.Assessments.First(x => x.AssessmentId == assessmentId).Name;

        }


        public int ComputeScore(int bundle_id, string result_text)
        {
            string[] qa = result_text.Split(';');

            int score = 0;

            foreach (string st in qa)
            {
                if (IsCorrect(bundle_id, st))
                {
                    score++;
                }


            }

            return score;


        }


        public Question GetQuestion(int bundle_id, int qid)
        {
            Bundle bundle = new BundleService().GetBundle(bundle_id);


            return bundle.Assessments.SelectMany(assessment => assessment.Questions).FirstOrDefault(question => question.QuestionId == qid);
        }

      


        public bool IsCorrect(int bundle_id, string qa)
        {

            string[] both = qa.Split(':');

            int qid = Int32.Parse(both[0]);

             Question q = GetQuestion(bundle_id, qid);

            if (q == null)
            {
                return false;
            }

            Option[] answers = q.Options.Where(x => x.IsCorrect).ToArray();



            if (q.AnswerType.Equals("Single"))
            {
                int x = Int32.Parse(both[1]);
                if (x == answers[0].AnswerId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {

                string[] mult = both[1].Split(',');

                int[] options = new int[answers.Length];
                for (int i = 0; i < answers.Length; i++)
                {
                    options[i] = answers[i].AnswerId;
                }

                return MultCorrect(mult, options);

            }





        }


        public bool MultCorrect(string[] mult, int[] options)
        {
            if (mult.Length == options.Length)
            {
                int[] arr = new int[mult.Length];

                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = Int32.Parse(mult[i]);
                }

                Array.Sort(arr);
                Array.Sort(options);
                bool correct = true;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] == options[i])
                    {

                    }
                    else
                    {
                        correct = false;
                        return correct;
                    }

                }

                return correct;

            }
            else
            {
                return false;
            }




        }
    }
}