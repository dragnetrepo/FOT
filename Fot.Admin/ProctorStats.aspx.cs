using Fot.Admin.Infrastructure;
using Fot.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fot.Admin
{
    public partial class ProctorStats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Page.IsPostBack)
            {

                var id = Request.QueryString["id"];

                LoadInfo(id);
            }
        }


        public void LoadInfo(string id)
        {
            var ctx = new ServiceBase().Context;

            var entry = ctx.CampaignEntries.Where(x => x.ProctorPlaybackId == id).Select(x => new { x.Candidate.FirstName, x.Candidate.LastName, x.CampaignId }).FirstOrDefault();

            lblCandidateName.Text = entry.FirstName + " " + entry.LastName;

            lblFrame.Text = $"<iframe src='https://app.proview.io/embedded/{id}' width='100%' height='650px;'style = 'overflow:auto'>";


            hidId.Value = entry.CampaignId.ToString();
        }

        protected void bttnBackToResults_Click(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];

            var ctx = new ServiceBase().Context;

            var entry = ctx.CampaignEntries.Where(x => x.ProctorPlaybackId == id).Select(x => new {x.CampaignId }).FirstOrDefault();

            Response.Redirect(UrlMapper.Results + "?id=" + entry.CampaignId.ToString());
        }
    }
}