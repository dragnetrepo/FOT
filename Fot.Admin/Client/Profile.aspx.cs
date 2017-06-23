using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadPartner();
            }
        }

        private void LoadPartner()
        {
            var admin = new AdminUserService().GetCurrentAdmin();

            if (admin != null)
            {
                var partner = new PartnerService().GetPartner(admin.PartnerId.Value);

                lblPartnerName.Text = partner.PartnerName;

                lblBalance.Text = partner.WalletBalance.ToString("#,##0.00");

                hidId.Value = admin.PartnerId.ToString();

            }



        }

      
    }
}