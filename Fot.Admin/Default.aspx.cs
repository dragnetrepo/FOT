using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using Telerik.Web.UI;

namespace Fot.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Utilities.CheckSerial();


            if (!Page.IsPostBack)
            {
                ProcessLogout();
            }
        }

        private void ProcessLogout()
        {
            if (Session["ADMIN_STATUS"] != null)
            {
                var app = (AppMessage)Session["ADMIN_STATUS"];

                lblStatus.ShowMessage(app);

                Session["ADMIN_STATUS"] = null;
            }
        }

        protected void bttnLogin_Click(object sender, EventArgs e)
        {
            DoLogin();
        }

        public void DoLogin()
        {
            if (Membership.ValidateUser(txtUsername.Text, txtPassword.Text))
            {

                var returnUrl = Request.QueryString["ReturnUrl"];

                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = UrlMapper.Main;
                }
                else
                {
                    returnUrl = returnUrl.Equals(Request.ApplicationPath + "/") || returnUrl.Equals("/") ? UrlMapper.Main : returnUrl;
                }


                var admin = new AdminUserService().GetAdminUserByName(txtUsername.Text);


                if (admin.Active)
                {
                    Session["CURRENT_ADMIN"] = admin;

                    if (admin.LastPasswordChangedDate.HasValue == false || DateTime.Today.Subtract(admin.LastPasswordChangedDate.Value).TotalDays > 90)
                    {
                        divLogin.Visible = false;
                        divPassword.Visible = true;

                        var message = admin.LastPasswordChangedDate.HasValue ? "Password change is required at least once every 3 months. Please change your password." : "Password change is required on first login. Please change your password.";

                        var app = new AppMessage()
                        {
                            Status = MessageStatus.Info,
                            Message = message
                                
                        };


                        lblStatus.ShowMessage(app);

                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(txtUsername.Text, false);


                        Session["CURRENT_ADMIN"] = admin;

                        if (admin.IsCenterAdmin || admin.IsCaptureAdmin)
                        {
                            Response.Redirect(UrlMapper.CenterMain);
                        }
                        else if (admin.IsPartnerAdmin)
                        {
                            var partner = new PartnerService().GetPartner(admin.PartnerId.Value);
                            if (partner.IsSelfManaged)
                            {
                                Response.Redirect(UrlMapper.PartnerMain);
                            }
                            else
                            {
                                var app = new AppMessage()
                                    {
                                        Status = MessageStatus.Error,
                                        Message =
                                            "Could not login! Specified partner account may have been changed from 'Self Managed'."
                                    };


                                lblStatus.ShowMessage(app);
                            }
                        }
                        else
                        {
                            //Response.Redirect(IsLocalUrl(returnUrl) ? returnUrl : UrlMapper.Main);
                            Response.Redirect(UrlMapper.Main);
                        }
                    }

                }
                else
                {
                    var app = new AppMessage() { Status = MessageStatus.Error, Message = "Could not login! Your account is currently inactive." };
                    lblStatus.ShowMessage(app);
                }
            }
            else
            {
                var app = new AppMessage() { Status = MessageStatus.Error, Message = "Invalid login details" };


                lblStatus.ShowMessage(app);
            }
        }

        private bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            Uri absoluteUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                return String.Equals(this.Request.Url.Host, absoluteUri.Host,
                                     StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
                               && !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
                               && Uri.IsWellFormedUriString(url, UriKind.Relative);
                return isLocal;
            }
        }

        protected void bttnChangePassword_Click(object sender, EventArgs e)
        {
            var admin = Session["CURRENT_ADMIN"] as AdminUser;

            if(admin == null) Response.Redirect(UrlMapper.Default);

            var app = new AdminUserService().ChangePassword(admin.Username, txtNewPassword.Text);

            Response.Redirect(UrlMapper.Default);
            
        }
    }
}