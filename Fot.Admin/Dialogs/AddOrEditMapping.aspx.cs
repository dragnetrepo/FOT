using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Services;

namespace Fot.Admin.Dialogs
{
    public partial class AddOrEditMapping : System.Web.UI.Page
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

        private void LoadForUpdate(int id)
        {
            var item = new LocationService().GetLocation(id);

            if(item != null)
            {
                hidId.Value = id.ToString();

                listLocation.SelectedValue = item.LocationId.ToString();
                listMapTo.SelectedValue = item.MappedToLocation.ToString();

                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                listLocation.Enabled = false;

            }
            else
            {
                form1.Visible = false;
            }
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            if (listLocation.SelectedIndex < 0 || listMapTo.SelectedIndex < 0) return;
            
            AddMapping();
        }

        private void AddMapping()
        {
            var app = new LocationService().MapLocation(Int32.Parse(listLocation.SelectedValue),
                                                        Int32.Parse(listMapTo.SelectedValue));

            lblStatus.ShowMessage(app);

            if(app.IsDone)
            {
                listLocation.DataBind();
                listMapTo.DataBind();

                RegisterScript();
            }

        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (listLocation.SelectedIndex < 0 || listMapTo.SelectedIndex < 0) return;

           AddMapping();
        }

      
    }
}