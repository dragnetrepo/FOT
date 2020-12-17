using Fot.Admin.Models;
using Fot.Admin.Services;
using Fot.Client.Infrastructure;
using Fot.DTO;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Fot.Admin.Infrastructure
{
    public class ProcessHub : Hub
    {
        public readonly FotContext ctx;
        public ProcessHub()
        {
            var service = new CandidateService();
             ctx = service.Context;
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.Add(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.Remove(Context.ConnectionId, roomName);
        }


        public async Task  SubmitEntry(int entryId)
        {

            this.Clients.Client(this.Context.ConnectionId).sendMessage("Submitting Candidate Assessment"); //.SendAsync(BACKUP_MESSAGE, "Backup completed successfully");

            var entry = ctx.CandidateAssessments.FirstOrDefault(x => x.CampaignEntryId == entryId);

            try
            {
                if (entry != null)
                {
                    var savedItem = GetSavedBundle(entry);

                    var res = GetResponses(savedItem);

                    var flag = await Submit(entry.CandidateGuid, res);

                    this.Clients.Client(this.Context.ConnectionId).sendMessage(flag.Succeeded ? "Candidate assessment submitted successfully." : "Candidate assessment submission failed. Error: " + flag.ErrorMessage );

                    if (flag.Succeeded)
                        await UserSubmit(entry.CandidateGuid);


                    await Task.Delay(TimeSpan.FromSeconds(2));
                }

                
            }
            catch(Exception ex)
            {
                this.Clients.Client(this.Context.ConnectionId).sendMessage("An error occured during the submission process!"+ ex.Message);

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        private async Task UserSubmit(string id)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var serviceUrl = ConfigurationManager.AppSettings["ServiceUrl"];

            var data = await client.GetAsync($"{serviceUrl}/Tests/UserSubmit/" + id);

        }

        public async Task SubmitAll(int campaignId)
        {
            var entries = ctx.CandidateAssessments.Where(x => x.CampaignEntry.CampaignId == campaignId)
                .Select(x => new {x.CampaignEntryId, x.CampaignEntry.Candidate.Username }).ToList();

            foreach(var entry in entries)
            {
                this.Clients.Client(this.Context.ConnectionId).sendMessage($"Submitting Entry [{entry.Username}]");

                var a = ctx.CandidateAssessments.AsNoTracking().FirstOrDefault(x => x.CampaignEntryId == entry.CampaignEntryId);

                if (a != null)
                {
                    var savedItem = GetSavedBundle(a);

                    var res = GetResponses(savedItem);

                    var flag = await Submit(a.CandidateGuid, res);

                    this.Clients.Client(this.Context.ConnectionId).sendMessage(flag.Succeeded? $"Submitted entry [{entry.Username}] successfully." : $"Submission failed for entry [{entry.Username}] Error: " + flag.ErrorMessage);

                    if (flag.Succeeded)
                        await UserSubmit(a.CandidateGuid);
                }
            }
           

            this.Clients.Client(this.Context.ConnectionId).sendMessage("All Assessments Entries Have Been Processed.");

            await Task.Delay(TimeSpan.FromSeconds(2));
        }


        private async Task<ResultResponse> Submit(string id, List<AssessmentResponse> responses)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var serviceUrl = ConfigurationManager.AppSettings["ServiceUrl"];

            var data = await client.PostAsJsonAsync($"{serviceUrl}/Tests/SubmitTest/" + id, responses);


            if(data.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await data.Content.ReadAsStringAsync();

                var ret = JsonConvert.DeserializeObject<ResultResponse>(result);

                return ret;
            }
            else
            {
                return new ResultResponse { Succeeded = false, ErrorMessage = "Service Failed"};
            }

        }


        private AppBundle GetSavedBundle(CandidateAssessment savedItem)
        {
            var appBundle = FotSecurity<AppBundle>.Deserialize(savedItem.AssessmentData.ToArray());

            var list = new List<int>();
            try
            {
                if (savedItem.CurrentAssessmentState != null)
                {
                    var bundleStatus = FotSecurity<AppBundle>.Deserialize(savedItem.CurrentAssessmentState.ToArray());

                    appBundle.current_assessment_index = bundleStatus.current_assessment_index;

                    foreach (var item in bundleStatus.assessments)
                    {

                        var temp = appBundle.assessments.First(x => x.assessment_id == item.assessment_id);
                        temp.time_remaining = item.time_remaining;
                        temp.current_question_index = item.current_question_index;
                        temp.started = item.started;

                        if (temp.assessment_type.Equals("MCQ"))
                        {
                            foreach (var questionStatus in item.questions)
                            {
                                var tempQuestion = temp.questions.First(x => x.question_id == questionStatus.question_id);
                                tempQuestion.seen = questionStatus.seen;
                                tempQuestion.answered = questionStatus.answered;

                                foreach (var answerStatus in questionStatus.answers)
                                {
                                    var tempAnswer = tempQuestion.answers.First(x => x.answer_id == answerStatus.answer_id);
                                    tempAnswer.selected = answerStatus.selected;
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
            catch (Exception ex)
            {

            }


            return appBundle;
        }


        private List<AssessmentResponse> GetResponses(AppBundle bundle)
        {
            var responses = new List<AssessmentResponse>();

            var list = new List<int>();

            foreach (var entry in bundle.assessments)
            {
                if (list.Contains(entry.assessment_id)) { continue; } else { list.Add(entry.assessment_id); }

                var temp = new AssessmentResponse();

                temp.assessment_id = entry.assessment_id;

                if (entry.assessment_type == "MCQ")
                {
                    var options = new List<string>();
                    foreach (var question in entry.questions)
                    {
                        var answered = question.answers.Count(y => y.selected);

                        var item = $"{question.question_id}:{(answered > 0 ? string.Join(",", question.answers.Where(t => t.selected).Select(y => y.answer_id).ToList()) : "0")}";
                        options.Add(item);
                    }

                    temp.result = string.Join(";", options);

                }
                else
                {
                    var selectedEssay = entry.essays.FirstOrDefault(x => x.selected);
                    temp.is_essay = true;
                    if(selectedEssay != null)
                    {
                        temp.result = selectedEssay.candidate_response;
                        temp.essay_id = selectedEssay.essay_id;
                    }

                }

                responses.Add(temp);

            }

            return responses;
        }
    }

    public class ResultResponse
    {
        public bool Succeeded { get; set; }
        public List<ResultModel> resultList { get; set; }

        public string ErrorMessage { get; set; }


        public ResultResponse()
        {
            resultList = new List<ResultModel>();
        }
    }

    public class ResultModel
    {
        public string AssessmentName { get; set; }
        public int Score { get; set; }
    }
}
