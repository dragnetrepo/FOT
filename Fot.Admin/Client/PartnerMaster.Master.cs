using System;
using System.Web.UI.WebControls;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
    public partial class PartnerMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ProcessNavigation();

        }


        public void ProcessNavigation()
        {

            var admin = new AdminUserService().GetCurrentAdmin();

            if (admin.HasUsersAccess) return;

            AdminPanelBar.Items[0].Visible = admin.CanAuthor;

            AdminPanelBar.Items[1].Visible = admin.CanSchedule;

            AdminPanelBar.Items[2].Visible = admin.HasCenterUsersAccess;

            AdminPanelBar.Items[2].Items[0].Visible = false;

            AdminPanelBar.Items[2].ChildGroupHeight = new Unit(40);


            AdminPanelBar.Items[3].Items[0].Visible = false;


           

        }
    }
}