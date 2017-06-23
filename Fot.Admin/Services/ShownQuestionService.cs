using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class ShownQuestionService : ServiceBase
    {
        public int QuestionShownCount(int CampaignId, int QuestionId)
        {
            return Context.ShownQuestions.Count(x => x.CampaignId == CampaignId && x.QuestionId == QuestionId);
        }


        public List<ChosenOption> GetChosenOptions(int CampaignId, int QuestionId)
        {
            return
                Context.ChosenOptions.Where(
                    x => x.ShownQuestion.CampaignId == CampaignId && x.ShownQuestion.QuestionId == QuestionId)
                       .ToList();
        }
    }
}