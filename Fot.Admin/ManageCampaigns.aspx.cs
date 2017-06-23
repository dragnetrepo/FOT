using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Schedule)]
    public partial class ManageCampaigns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if(e.Argument.Equals("Rebind"))
            {
                RadGrid1.DataBind();
            }
        }

        protected void listPartners_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }
    }
}