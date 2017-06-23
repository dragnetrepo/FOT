using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Services;

namespace Fot.Admin.Dialogs
{
    public partial class FeedbackMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForUpdate(id);
                }


            }
        }

        private void LoadForUpdate(int id)
        {
            var item = new CandidateFeedbackService().GetCandidateFeedback(id);

            if (item != null)
            {
                lblCandidate.Text = item.CandidateName;
                lblDate.Text = item.DateSent.ToString("dd-MMM-yyyy");
                lblSubject.Text = item.Subject;
                lblMessage.Text = item.Message;

            }
            else
            {
                form1.Visible = false;
            }
        }

    }
}