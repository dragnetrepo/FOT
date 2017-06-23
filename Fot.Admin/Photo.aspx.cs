using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Services;

namespace Fot.Admin
{
    public partial class Photo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int entryId = Int32.Parse(Request.QueryString["id"]);

            LoadPhoto(entryId);
        }

        private void LoadPhoto(int entryId)
        {
            var ctx = new ServiceBase().Context;

            var entry = ctx.CampaignEntries.Where(x => x.EntryId == entryId && (x.PhotoCaptured.HasValue && x.PhotoCaptured.Value)).Include(x => x.AdminUser).FirstOrDefault();


            if (entry != null)
            {

                if (entry.PhotoCapturedBy.HasValue)
                {
                   lblAdmin.Text = entry.AdminUser.Username; 
                }
                

                var campaignFolder = entry.CampaignId;
                var url =  "~/photos/" + campaignFolder + "/" + string.Format("{0}_{1}.jpg", entry.CandidateId, entry.EntryId);

                imgPhoto.ImageUrl = url;
            }
            else
            {
                form1.Visible = false;
            }

        }
    }
}