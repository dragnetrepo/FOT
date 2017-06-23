using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin
{
    public partial class Summary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSummary();
        }



        public void LoadSummary()
        {
            var id = Int32.Parse(Request.QueryString["id"]);

            var ctx = new ServiceBase().Context;

            var assessment = ctx.Assessments.Find(id);

            if (assessment != null)
            {
                lblSummary.Text = assessment.Name;
            }
            else
            {
                Response.Redirect("Assessments.aspx");
            }
        }



        public AssessmentSummaryViewModel GetDetails()
        {

            var ctx = new ServiceBase().Context;

            var id = Int32.Parse(Request.QueryString["id"]);

            var item = ctx.Assessments.Where(x => x.AssessmentId == id)
            .Select(x => new AssessmentSummaryViewModel
            {
                AssessmentName = x.Name,
                Developer = x.AuthorId.HasValue ? x.AssessmentAuthor.AuthorName : string.Empty,
                YearCreated = x.DateAdded.Year.ToString(),
                AssessmentType = x.AssessmentType.ToString(),
                Deployments = x.AssessmentBundleEntries.Any() ? x.AssessmentBundleEntries.Sum(y => y.AssessmentBundle.Campaigns.Count(z => z.CampaignEntries.Any(v => v.Tested))) : 0,
                Campaigns = x.AssessmentBundleEntries.SelectMany(y => y.AssessmentBundle.Campaigns.Where(z => z.CampaignEntries.Any(v => v.Tested)).Select(t => new PartnerCampaign{PartnerName = t.Partner.PartnerName, CampaignName = t.CampaignName }))


            }).FirstOrDefault();

            return item;
        } 
    }
}