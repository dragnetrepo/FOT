using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
    
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    public partial class AddOrEditAdmin : System.Web.UI.Page
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
            var item = new AdminUserService().GetAdminUser(id);

            if (item != null)
            {
                hidId.Value = id.ToString();
                txtEmail.Text = item.Username;
                txtFirstName.Text = item.Firstname;
                txtLastName.Text = item.Lastname;
                txtMobileNumber.Text = item.MobileNo;
                chkActive.Checked = item.Active;

                chkListPermissions.Items[0].Selected = item.CanAuthor;
                chkListPermissions.Items[1].Selected = item.CanSchedule;
                chkListPermissions.Items[2].Selected = item.HasResultsAccess;
                chkListPermissions.Items[3].Selected = item.HasCenterUsersAccess;

                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                lblHeader.Text = "Update Administrator";


                divAuthorAssessments.Visible = item.CanAuthor;


                txtPassword.CssClass = string.Empty;


            }
            else
            {
                Response.Redirect(UrlMapper.Administrators);
            }

        }

    

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddAdmin();
        }

        private void AddAdmin()
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();


            var item = new AdminUser
            {
                Username = txtEmail.Text,
                Password = txtPassword.Text,
                Firstname = txtFirstName.Text,
                Lastname = txtLastName.Text,
                MobileNo = txtMobileNumber.Text,
                Active = chkActive.Checked,
                IsPartnerAdmin = true,
                PartnerId = currentAdmin.PartnerId,
                CanAuthor = chkListPermissions.Items[0].Selected,
                CanSchedule = chkListPermissions.Items[1].Selected,
                HasResultsAccess = chkListPermissions.Items[2].Selected,
                HasCenterUsersAccess = chkListPermissions.Items[3].Selected
            };

            var app = new AdminUserService().AddAdminUser(item);

            lblStatus.ShowMessage(app);

            if (app.IsDone)
            {
                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Added an admin", LogEntryDetails = "User added a new administrator [" + item.Username + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });


                if (item.CanAuthor)
                {
                    
                    LoadForUpdate(item.AdminId);
                }
                else
                {
                    Response.Redirect(UrlMapper.Administrators);
                }
              
               
            }

            
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAdmin();
        }

        private void UpdateAdmin()
        {
            var service = new AdminUserService();

            var item = service.GetAdminUser(Int32.Parse(hidId.Value));

            if (item != null)
            {
                item.Username = txtEmail.Text;
                item.Password = string.IsNullOrWhiteSpace(txtPassword.Text) ? string.Empty : txtPassword.Text;
                item.Firstname = txtFirstName.Text;
                item.Lastname = txtLastName.Text;
                item.MobileNo = txtMobileNumber.Text;
                item.Active = chkActive.Checked;
                item.CanAuthor = chkListPermissions.Items[0].Selected;
                item.CanSchedule = chkListPermissions.Items[1].Selected;
                item.HasResultsAccess = chkListPermissions.Items[2].Selected;
                item.HasCenterUsersAccess = chkListPermissions.Items[3].Selected;


                var app = new AdminUserService().UpdateAdminUser(item);

                if (app.IsDone)
                {

                      divAuthorAssessments.Visible = item.CanAuthor;


                      var admin = new AdminUserService().GetCurrentAdmin();

                      new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Updated an admin", LogEntryDetails = "User updated an administrator [" + item.Username + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

                  
                }

                lblStatus.ShowMessage(app);

            }

        }

        protected void bttnAddAssessment_Click(object sender, EventArgs e)
        {
            AddAssessment();
        }

        private void AddAssessment()
        {

            if (listAssessments.SelectedIndex < 0) return;

            var service = new PartnerAuthorAssignedAssessmentService();


            int assessmentId = Int32.Parse(listAssessments.SelectedValue);
            int adminId = Int32.Parse(hidId.Value);



            var app = service.AssignAssessmentToAuthor(assessmentId, adminId);

            GridAssessments.DataBind();
            listAssessments.DataBind();


        }

        protected void GridAssessments_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
        {
            listAssessments.DataBind();
        }
    }
}