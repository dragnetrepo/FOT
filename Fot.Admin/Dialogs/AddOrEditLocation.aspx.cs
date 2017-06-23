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
    public partial class AddOrEditLocation : System.Web.UI.Page
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
            var item = new LocationService().GetLocation(id);

            if(item != null)
            {
                hidId.Value = id.ToString();
                txtLocation.Text = item.LocationName;
                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                this.Title = "Update Location";

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
            if(string.IsNullOrWhiteSpace(txtLocation.Text)) return;
            AddLocation();
        }

        private void AddLocation()
        {
            var item = new Location
                {
                    LocationName = txtLocation.Text

                };

            var app = new LocationService().Add(item);

            lblStatus.ShowMessage(app);
            if(app.IsDone)
            {
                txtLocation.Text = string.Empty;
                RegisterScript();
            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text)) return;
            UploadLocation();
        }

        private void UploadLocation()
        {
            var locationService = new LocationService();

            var item = locationService.GetLocation(Int32.Parse(hidId.Value));

            if(item != null)
            {
                item.LocationName = txtLocation.Text;

                var app = locationService.Update(item);

                lblStatus.ShowMessage(app);

                if(app.IsDone)
                {
                    RegisterScript();

                }
            }
        }
    }
}