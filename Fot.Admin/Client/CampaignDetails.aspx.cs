using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
   [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
   [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Schedule)]
    public partial class CampaignDetails : System.Web.UI.Page
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
                else
                {
                    Response.Redirect(UrlMapper.ManageCampaigns);
                }

            }
        }

        private void LoadForUpdate(int id)
        {
            var item = new PartnerCampaignService().GetCampaign(id);

            if(item != null)
            {
                txtCampaignName.Text = item.CampaignName;
                listAssessmentBundles.SelectedValue = item.BundleId.ToString();
                chkActive.Checked = item.Active;
                chkFeedback.Checked = item.ShowFeedback;
                chkViewResults.Checked = item.ViewResult;
                hidId.Value = id.ToString();

                editor.Content = item.Instructions;

                lblCampaignType.Text = item.IsUnproctored ? "Unproctored" : "Proctored";

                trStartDate.Visible = trEndDate.Visible = trProctored.Visible = trSeb.Visible = item.IsUnproctored;

                if (!string.IsNullOrWhiteSpace(item.InvitationLogo))
                {
                    imgLogo.ImageUrl = ConfigurationManager.AppSettings["PhotoUrl"] + item.InvitationLogo;
                }

                

                if (item.IsUnproctored)
                {
                    txtStartDate.SelectedDate = item.StartDate;
                    txtEndDate.SelectedDate = item.EndDate;


                    if (item.StartDate < DateTime.Today)
                    {
                        txtStartDate.Enabled = false;

                    }

                    chkProctoring.Checked = item.EnableProctoring;


                    RadTabStrip1.Tabs[1].Visible = false;
                    RadTabStrip1.Tabs[2].Visible = false;

                    chkSeb.Checked = item.RequireSEB;
                }

                if (new PartnerCampaignService().AnyCandidateTestedOrScheduled(id))
                {
                    listAssessmentBundles.Enabled = false;
                }



                int noShowCount = new CampaignEntryService().GetUntestedUnscheduledCandidates(id);

                if (noShowCount > 0)
                {
                    lblCount.Text = noShowCount.ToString("#,##0");
                    divReset.Visible = true;
                }

                var currentAdmin = new AdminUserService().GetCurrentAdmin();

                if (currentAdmin.HasUsersAccess || currentAdmin.HasResultsAccess)
                {
                    boxResult.Attributes.Add("onclick", "location.href='Results.aspx?id=" + id + "'");
                }
                else
                {
                    boxResult.Visible = false;
                }

                if (!currentAdmin.HasUsersAccess)
                {
                    divCampaignReports.Visible = false;
                }



                linkScheduling.Text = item.IsUnproctored ? string.Empty : "<div class='boxDiv' onclick=\"location.href='Scheduling.aspx?id="+ id +"';\" ><div class='codeStyle'>Scheduling</div></div>";

                linkResponses.Text = "<div class='boxDiv' onclick=\"location.href='InvitationResponses.aspx?id=" + id + "';\" ><div class='codeStyle'>Invite Responses</div></div>";

            
            
            }
            else
            {
                Response.Redirect(UrlMapper.ManageCampaigns);
            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateCampaign();
        }


        private void UpdateCampaign()
        {
            var service = new PartnerCampaignService();

            var item = service.GetCampaign(Int32.Parse(hidId.Value));

            if (item != null)
            {
                item.CampaignName = txtCampaignName.Text;
                item.BundleId = Int32.Parse(listAssessmentBundles.SelectedValue);
                item.Active = chkActive.Checked;
                item.ShowFeedback = chkFeedback.Checked;
                item.ViewResult = chkViewResults.Checked;


                if (item.IsUnproctored)
                {
                    if (item.StartDate != txtStartDate.SelectedDate)
                    {
                        item.StartDate = txtStartDate.SelectedDate;
                    }

                    if (item.EndDate != txtEndDate.SelectedDate)
                    {
                        item.EndDate = txtEndDate.SelectedDate;
                    }

                    item.EnableProctoring = chkProctoring.Checked;

                    item.RequireSEB = chkSeb.Checked;
                }

                var app = service.Update(item);

                lblStatus.ShowMessage(app);
            }
        }

        protected void bttnReset_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(hidId.Value);

            new CampaignEntryService().ResetUntestedScheduledCandidates(id);

            divReset.Visible = false;

            LoadForUpdate(id);
        }

        protected void bttnUpdateCampaignInstructions_Click(object sender, EventArgs e)
        {
            UpdateCampaignInstructions();
        }

        private void UpdateCampaignInstructions()
        {
            var service = new PartnerCampaignService();

            var item = service.GetCampaign(Int32.Parse(hidId.Value));

            if (item != null)
            {

                item.Instructions = editor.Content;

                var app = service.Update(item);

                lblStatus.ShowMessage(app);
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument.Equals("Rebind"))
            {
                RadGrid1.DataBind();
            }
        }


        public bool UploadIsValid()
        {
            var file = picFile.PostedFile;
            var app = new AppMessage();

            if (file == null || file.ContentLength < 2)
            {
                app = new AppMessage()
                {
                    IsDone = false,
                    Message = "No image file was selected.",
                    Status = MessageStatus.Error
                };

                lblStatus.ShowMessage(app);

                return false;
            }
            var fileExt = Path.GetExtension(file.FileName.ToLower());
            if (!fileExt.Equals(".jpg") && !fileExt.Equals(".jpeg"))
            {
                app = new AppMessage()
                {
                    IsDone = false,
                    Message = "Only Jpeg files (.jpg, .jpeg) are allowed.",
                    Status = MessageStatus.Error
                };

                lblStatus.ShowMessage(app);

                return false;
            }

            if (file.ContentLength > 204800)
            {
                app = new AppMessage()
                {
                    IsDone = false,
                    Message = "File should not exceed 200kb in size.",
                    Status = MessageStatus.Error
                };

                lblStatus.ShowMessage(app);

                return false;
            }



            var img = System.Drawing.Image.FromStream(file.InputStream);

            if (img.Width > 400 || img.Height > 90)
            {
                app = new AppMessage()
                {
                    IsDone = false,
                    Message = "Image width should not exceed 400px and height should not exceed 90px.",
                    Status = MessageStatus.Error
                };

                lblStatus.ShowMessage(app);

                return false;
            }



            return true;
        }

        private void UploadFile()
        {


            var fileName = System.Guid.NewGuid().ToString().Replace("-", string.Empty) + ".jpg";
            var path = ConfigurationManager.AppSettings["PhotoDir"];
            var fullPath = Path.Combine(path, fileName);

            picFile.SaveAs(fullPath);

            var service = new ServiceBase();
            int campaignId = Int32.Parse(hidId.Value);


            var item = service.Context.Campaigns.FirstOrDefault(x => x.CampaignId == campaignId);

            if (!string.IsNullOrWhiteSpace(item.InvitationLogo))
            {
                var tempPath = Path.Combine(path, item.InvitationLogo);

                File.Delete(tempPath);
            }

            item.InvitationLogo = fileName;


            service.Context.SaveChanges();

            lblStatus.ShowMessage(new AppMessage
            {
                IsDone = true,
                Message = "Logo uploaded successfully.",
                Status = MessageStatus.Success
            });


            imgLogo.ImageUrl = ConfigurationManager.AppSettings["PhotoUrl"] + fileName;


        }

        protected void bttnUpload_Click(object sender, EventArgs e)
        {
            if (picFile.HasFile && UploadIsValid())
            {
                UploadFile();
            }
        }

        protected void bttnUpdateOptions_Click(object sender, EventArgs e)
        {
            UpdateOptions();
        }

        private void UpdateOptions()
        {
            var service = new ServiceBase();
            int campaignId = Int32.Parse(hidId.Value);


            var item = service.Context.Campaigns.FirstOrDefault(x => x.CampaignId == campaignId);


            if (string.IsNullOrWhiteSpace(item.InvitationLogo) && Int32.Parse(listOptions.SelectedValue) > 1)
            {
                lblStatus.ShowMessage(new AppMessage
                {
                    IsDone = true,
                    Message = "Upload a company logo before selecting an option that requires it.",
                    Status = MessageStatus.Error
                });

                return;
            }


            item.LogoPlacement = Int32.Parse(listOptions.SelectedValue);

            service.Context.SaveChanges();

            lblStatus.ShowMessage(new AppMessage
            {
                IsDone = true,
                Message = "Logo options updated successfully.",
                Status = MessageStatus.Success
            });
        }

     
    }
}