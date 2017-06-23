using System;
using System.Collections.Generic;
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
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Author)]
    public partial class AddOrEditBundle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadBundle(id);
                }
            }
        }

        private void LoadBundle(int id)
        {
            var bundle = new AssessmentBundleService().GetBundle(id);

            if(bundle != null)
            {
                txtBundleName.Text = bundle.Name;
                editor.Content = bundle.Description;
                chkSaveAsYouGo.Checked = bundle.SaveAsYouGo;
                chkShowResultsOnSubmit.Checked = bundle.ShowResultsOnSubmit;
                chkSendNotification.Checked = bundle.SendResultNotification;

                chkAllowAssessmentSelection.Checked = bundle.AllowAssessmentSelection;


                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                divAssessments.Visible = true;

                hidId.Value = id.ToString();


                if (bundle.MinAggregatePassScore.HasValue)
                {
                    chkShowPassFailMessage.Checked = true;
                    txtMinScore.Text = bundle.MinAggregatePassScore.Value.ToString();

                }

            }
            else
            {
                Response.Redirect(UrlMapper.Bundles);
            }

        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddBundle();
        }

        private void AddBundle()
        {
            if(string.IsNullOrWhiteSpace(txtBundleName.Text)) return;


            string content = editor.Content;
            content = Utilities.FormatHtmlContent(content);


            string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + content + @"</td></tr></table>";


           
            byte[] htmlImage = new Html2ImageBinary(html).GetImage();


            var bundle = new AssessmentBundle
                {
                    Name = txtBundleName.Text,
                    Description = editor.Content,
                    ShowResultsOnSubmit = chkShowResultsOnSubmit.Checked,
                    SaveAsYouGo = chkSaveAsYouGo.Checked,
                    SendResultNotification = chkSendNotification.Checked,
                    DescriptionImage = htmlImage,
                    AllowAssessmentSelection = chkAllowAssessmentSelection.Checked
                };

            if (chkShowPassFailMessage.Checked)
            {
                int minScore = Int32.Parse(txtMinScore.Text);

                if (minScore > 0)
                {
                    bundle.MinAggregatePassScore = minScore;
                }
            }

            var app = new AssessmentBundleService().Add(bundle);

            if(app.IsDone)
            {
                divAssessments.Visible = true;
                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;
                hidId.Value = ((int)app.Data).ToString();

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Added assessment bundle", LogEntryDetails = "User added a new assessment bundle ["+bundle.Name+"]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            }

            lblStatus.ShowMessage(app);

            
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateBundle();
        }

        private void UpdateBundle()
        {
            var bundleService = new AssessmentBundleService();

            var bundle = bundleService.GetBundle(Int32.Parse(hidId.Value));

            string content = editor.Content;
            content = Utilities.FormatHtmlContent(content);


            string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + content + @"</td></tr></table>";

            byte[] htmlImage = new Html2ImageBinary(html).GetImage();

            if(bundle != null)
            {
                bundle.Name = txtBundleName.Text;
                bundle.Description = editor.Content;
                bundle.SaveAsYouGo = chkSaveAsYouGo.Checked;
                bundle.ShowResultsOnSubmit = chkShowResultsOnSubmit.Checked;
                bundle.SendResultNotification = chkSendNotification.Checked;
                bundle.DescriptionImage = htmlImage;
                bundle.AllowAssessmentSelection = chkAllowAssessmentSelection.Checked;


                if (chkShowPassFailMessage.Checked)
                {
                    int minScore = Int32.Parse(txtMinScore.Text);

                    if (minScore > 0)
                    {
                        bundle.MinAggregatePassScore = minScore;
                    }
                    else
                    {
                        bundle.MinAggregatePassScore = null;
                    }
                   
                }
                else
                {
                    bundle.MinAggregatePassScore = null;
                }

                var app = bundleService.Update(bundle);

                lblStatus.ShowMessage(app);

                if (app.IsDone)
                {
                    var admin = new AdminUserService().GetCurrentAdmin();

                    new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Updated assessment bundle", LogEntryDetails = "User updated assessment bundle [" + bundle.Name + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

                }

            }
            
        }

        protected void bttnAddSelected_Click(object sender, EventArgs e)
        {
            if (listAssessments.SelectedIndex >= 0)
            {
                AddAssessment();
            }
        }

     

        private void AddAssessment()
        {
            var app = new AssessmentService().AddAssessmentToBundle(Int32.Parse(listAssessments.SelectedValue), Int32.Parse(hidId.Value));

            if(app.IsDone)
            {
                listAssessments.DataBind();
                RadGrid1.DataBind();
            }
        }

        protected void RadGrid1_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
        {
            listAssessments.DataBind();
            RadGrid1.DataBind();
        }
    }
}