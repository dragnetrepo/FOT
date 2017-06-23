using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Client.Models;
using Fot.Client.Services;

namespace Fot.Client
{
    public partial class Feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadList();
            }
        }

        private void LoadList()
        {
            var list = new CandidateFeedbackService().GetFeedbackTypes();

            foreach (var feedbackType in list)
            {
                listFeedbackType.Items.Add(new ListItem(feedbackType.FeedbackReason, feedbackType.EntryId.ToString()));
            }

            listFeedbackType.Items.Add(new ListItem("Other", "0"));


            if (listFeedbackType.Items.Count > 1)
            {
                trOther.Attributes.Add("style", "display:none;");
            }

            if (Session["USERID"] != null)
            {
                var item = new CandidateService().GetCandidateById(Int32.Parse(Session["USERID"].ToString()));

                lblCandidateName.Text = item.FirstName + " " + item.LastName;

                hidCandidateId.Value = item.CandidateId.ToString();
            }

        }

        protected void bttnSend_Click(object sender, EventArgs e)
        {
            SendFeedback();
        }

        private void SendFeedback()
        {
            var item = new CandidateFeedback
                {
                    CandidateId = Int32.Parse(hidCandidateId.Value),
                    FeedBackTypeId = listFeedbackType.SelectedValue.Equals("0") ? default(int?) : Int32.Parse(listFeedbackType.SelectedValue),
                    FeedbackOther = listFeedbackType.SelectedValue.Equals("0") ? txtOther.Text : listFeedbackType.SelectedItem.Text,
                    FeedbackMessage = editor.Content,
                    DateSent = DateTime.Today

                };

            var app = new CandidateFeedbackService().Add(item);

            if (app.IsDone)
            {
                tableFeedback.Visible = false;
            }

            lblStatus.ShowMessage(app);
        }
    }
}