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

namespace Fot.Admin.Client
{
    
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    public partial class Administrators : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                var currentAdmin = new AdminUserService().GetCurrentAdmin();
                hidPartnerId.Value = currentAdmin.PartnerId.ToString();
            }

        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if(e.Argument.Equals("Rebind"))
            {
                AdminGrid.DataBind();
            }
        }

        protected void bttnAddAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.AddOrEditAdmin);
        }
    }
}