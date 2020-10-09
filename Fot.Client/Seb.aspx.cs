using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fot.Lan;
using Fot.Client.Infrastructure;
using Fot.Client.Models;
using Fot.Client.Services;
using Telerik.Web.UI;
using System.Linq;

namespace Fot.Client
{

    public partial class Seb : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!Page.IsPostBack)
            {
                var key = Request.QueryString["key"];

                if(string.IsNullOrWhiteSpace(key))
                {
                    //show errors
                    return;
                }

                DoLogin(key);
            }
        }


        public void DoLogin(string key)
        {

            var agent = Request.UserAgent;

            if(!agent.Contains("SEB") && !agent.Contains("Dragon54Dragnet21333"))
            {
                lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Assessment must be done within <strong>Safe Exam Browser</strong>", Status = MessageStatus.Error });
                return;
            }


            var candidateService = new CandidateService();
            var ctx = candidateService.Context;
            var item = ctx.Candidates.FirstOrDefault(x => x.SebGuid == key);


            if (item != null)
            {

                    var list = candidateService.GetCandidateAssessments(item.CandidateId);

                    if (list.Count > 0)
                    {
                            trTakeTest.Visible = true;

                        ListView1.DataSource = list;
                        ListView1.DataBind();


                    }
                    else
                    {
                        lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "You are not currently scheduled for an assessment.", Status = MessageStatus.Error });
                    }

            }
            else
            {
                lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Invalid exam key", Status = MessageStatus.Error });
            }
        }

        protected void bttnTest_ServerClick(object sender, EventArgs e)
        {
            var candidateService = new CandidateService();
            var ctx = candidateService.Context;
            var key = Request.QueryString["key"];
            var item = ctx.Candidates.FirstOrDefault(x => x.SebGuid == key);

            var list = candidateService.GetCandidateAssessments(item.CandidateId);

            Response.Redirect("Tests/TakeTest/" + list.First().CandidateGuid);


        }
    }
}