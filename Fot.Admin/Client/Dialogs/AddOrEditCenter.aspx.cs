using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client.Dialogs
{
    public partial class AddOrEditCenter : System.Web.UI.Page
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
            var item = new PartnerCenterService().GetCenter(id);

            if (item != null)
            {
                hidId.Value = id.ToString();
                txtCenterName.Text = item.CenterName;
                txtAddress.Text = item.Address;
                listLocation.SelectedValue = item.LocationId.ToString();
                txtCapacity.Text = item.CapacityPerSession.ToString();
                chkActive.Checked = item.Active;

                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                this.Title = "Update Center";

            }
            else
            {
                form1.Visible = false;
            }
        }


        public void RegisterScript()
        {


            String csname1 = "PopupScript";
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.

            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                StringBuilder cstext1 = new StringBuilder();
                // cstext1.Append("alert('Hello World!');");
                var fnName = "refreshGrid";
                cstext1.Append("CallFunctionOnParentPage('" + fnName + "');");


                cs.RegisterStartupScript(cstype, csname1, cstext1.ToString(), true);
            }

        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddCenter();
        }

        private void AddCenter()
        {
            var item = new Center
            {
                CenterName = txtCenterName.Text,
                Address = txtAddress.Text,
                LocationId = Int32.Parse(listLocation.SelectedValue),
                CapacityPerSession = Int32.Parse(txtCapacity.Text),
                RatePerTested = 0 ,
                Active = chkActive.Checked
            };

            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            item.IsPrivateCenter = true;
            item.OwnerPartnerId = currentAdmin.PartnerId;


            var app = new PartnerCenterService().Add(item);

            lblStatus.ShowMessage(app);
            if (app.IsDone)
            {
                txtCenterName.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtCapacity.Text = string.Empty;
                

                RegisterScript();
            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateCenter();
        }

        private void UpdateCenter()
        {
            var centerService = new PartnerCenterService();

            var item = centerService.GetCenter(Int32.Parse(hidId.Value));

            if (item != null)
            {
                item.CenterName = txtCenterName.Text;
                item.Address = txtAddress.Text;
                item.LocationId = Int32.Parse(listLocation.SelectedValue);
                item.CapacityPerSession = Int32.Parse(txtCapacity.Text);
                item.Active = chkActive.Checked;


                var app = centerService.Update(item);

                lblStatus.ShowMessage(app);

                if (app.IsDone)
                {
                    RegisterScript();

                }
            }
        }
    }
}