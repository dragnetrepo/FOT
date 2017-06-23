using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void UpdatePassword()
        {
            var app = new AdminUserService().ChangePassword(User.Identity.Name, txtOldPassword.Text, txtNewPassword.Text);

            lblStatus.ShowMessage(app);
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                UpdatePassword();
            }
        }
    }
}