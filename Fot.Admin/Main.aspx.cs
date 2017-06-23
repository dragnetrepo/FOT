using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Services;

namespace Fot.Admin
{

    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadStats();

        }

        private void LoadStats()
        {
            var service = new StatsService();


            var assessmentArray = service.GetAssessmentCount();

            lblAssessments.Text = assessmentArray[0].ToString("#,##0");

            lblEssays.Text = assessmentArray[1].ToString("#,##0");

            lblMcq.Text = assessmentArray[2].ToString("#,##0");

            lblQuestions.Text = service.GetQuestionCount().ToString("#,##0");

            lblOptions.Text = service.GetOptionCount().ToString("#,##0");


            var adminArray = service.GetAdminCount();

            lblAdministrators.Text = adminArray[0].ToString("#,##0");

            lblRegularAdmins.Text = adminArray[1].ToString("#,##0");

            lblPartnerAdmins.Text = adminArray[2].ToString("#,##0");

            lblCenterAdmins.Text = adminArray[3].ToString("#,##0");

            lblCandidates.Text = service.GetCandidateCount().ToString("#,##0");


            lblCampaigns.Text = service.GetCampaignCount().ToString("#,##0");

            lblPartners.Text = service.GetPartnerCount().ToString("#,##0");

            lblCenters.Text = service.GetCenterCount().ToString("#,##0");

        }

 
    }
}