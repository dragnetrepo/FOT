using System;
using Fot.Admin.Services;

namespace Fot.Admin.TestCenter
{
    public partial class CenterMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ProcessNavigation();
        }

        private void ProcessNavigation()
        {
            var admin = new AdminUserService().GetCurrentAdmin();

            if (admin.IsCenterAdmin) return;

            AdminPanelBar.Items[0].Visible = false;
        }
    }
}