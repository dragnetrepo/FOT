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

namespace Fot.Admin.Dialogs
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
            var item = new CenterService().GetCenter(id);

            if (item != null)
            {
                hidId.Value = id.ToString();
                txtCenterName.Text = item.CenterName;
                txtAddress.Text = item.Address;
                listLocation.SelectedValue = item.LocationId.ToString();
                txtCapacity.Text = item.CapacityPerSession.ToString();
                txtRate.Text = ((int)item.RatePerTested).ToString();
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
                RatePerTested = Int32.Parse(txtRate.Text) ,
                Active = chkActive.Checked
            };



            var app = new CenterService().Add(item);

            lblStatus.ShowMessage(app);
            if (app.IsDone)
            {
                txtCenterName.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtCapacity.Text = string.Empty;
                txtRate.Text = string.Empty;

                RegisterScript();
            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
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
                item.LocationId = Int32.Parse(listLocation.SelectedValue);
                item.CapacityPerSession = Int32.Parse(txtCapacity.Text);
                item.RatePerTested = Int32.Parse(txtRate.Text);
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