using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.PartnerUsers)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.CenterUsers)]
    public partial class OtherAdmins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            var admin = new AdminUserService().GetCurrentAdmin();

            if (!admin.IsGlobalAdmin)
            {
                RadTabStrip1.Tabs[0].Visible = admin.HasPartnerUsersAccess;
                RadTabStrip1.Tabs[1].Visible = admin.HasCenterUsersAccess;


                if (admin.HasPartnerUsersAccess && !admin.HasCenterUsersAccess)
                {
                    RadTabStrip1.SelectedIndex = 0;
                    RadMultiPage1.SelectedIndex = 0;
                }
                else if (admin.HasCenterUsersAccess && !admin.HasPartnerUsersAccess)
                {
                    RadTabStrip1.SelectedIndex = 1;
                    RadMultiPage1.SelectedIndex = 1;
                }
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if(e.Argument.Equals("Rebind"))
            {
                PartnerGrid.DataBind();
                CenterGrid.DataBind();
            }
        }
    }
}