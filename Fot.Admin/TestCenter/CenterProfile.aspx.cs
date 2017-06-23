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

namespace Fot.Admin.TestCenter
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.CenterAdmin)]
    public partial class CenterProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                LoadCenter();
            }

        }

        private void LoadCenter()
        {
            var admin = new AdminUserService().GetCurrentAdmin();

            var center = new CenterService().GetCenter(admin.CenterId.Value);

            if (center != null)
            {
                txtCenterName.Text = center.CenterName;
                txtAddress.Text = center.Address;
                lblLocation.Text = center.Location.LocationName;
                lblRate.Text = center.RatePerTested.HasValue
                                   ? center.RatePerTested.Value.ToString("#,##0.00")
                                   : string.Empty;
                txtCapacity.Text = center.CapacityPerSession.ToString();

                hidId.Value = center.CenterId.ToString();
            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            int capacity;
            if (string.IsNullOrWhiteSpace(txtCenterName.Text) || !Int32.TryParse(txtCapacity.Text, out capacity)) return;
            UpdateCenter();
        }

        private void UpdateCenter()
        {
            var centerService = new CenterService();

            var item = centerService.GetCenter(Int32.Parse(hidId.Value));

            if (item != null)
            {
                item.CenterName = txtCenterName.Text;
                item.Address = txtAddress.Text;
               
                item.CapacityPerSession = Int32.Parse(txtCapacity.Text);
                
              


                var app = centerService.Update(item);

                lblStatus.ShowMessage(app);

                
            }
        }
    }
}