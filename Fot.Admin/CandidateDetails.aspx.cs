using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    public partial class CandidateDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
              if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadCandidate(id);
                }
            }
        
        }

        private void LoadCandidate(int id)
        {
            var service = new CandidateService();
            var candidate = service.GetCandidateById(id);

            if (candidate != null)
            {
                lblUsername.Text = candidate.Username;
                lblFname.Text = candidate.FirstName;
                lblLname.Text = candidate.LastName;
                lblMobile.Text = candidate.MobileNo;


                hidId.Value = id.ToString();

                if (candidate.LocationId.HasValue)
                {
                    listLocations.SelectedValue = candidate.LocationId.Value.ToString();
                }


                var campaignList = service.GetCandidateCampaigns(id, -1, -1);

                var testedList = campaignList.Where(x => x.Tested).ToList();

                if (testedList.Count > 0)
                {
                    var latest = testedList.OrderByDescending(x => x.DateTested).First();

                    var campaignFolder = UrlMapper.RootPhotosDirectory + latest.CampaignId.ToString();

                    var imageUrl = Path.Combine(campaignFolder, string.Format("{0}_{1}.jpg", id, latest.EntryId));

                    if (File.Exists(Server.MapPath(imageUrl)))
                    {
                        CandidatePicture.ImageUrl = imageUrl;
                    }
                    else
                    {
                        CandidatePicture.Visible = false;
                    }

                }

            }
            else
            {
                Response.Redirect(UrlMapper.ManageCandidates);
            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateCandidateLocation();
        }

        private void UpdateCandidateLocation()
        {
            var service = new CandidateService();

            var item = service.GetCandidateById(Int32.Parse(hidId.Value));

            item.LocationId = Int32.Parse(listLocations.SelectedValue);

            var app = service.Update(item);


            if (app.IsDone)
            {
                app.Message = "Candidate location updated successfully.";
                lblStatus.ShowMessage(app);
            }
            else
            {

                lblStatus.ShowMessage(app);
            }

        }
    }
}