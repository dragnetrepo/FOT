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
    public partial class AddCampaignSession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForAdd(id);
                }
                else
                {
                    form1.Visible = false;
                }

            }
        }

        private void LoadForAdd(int id)
        {
            var item = new CampaignService().GetCampaign(id);

            if (item != null)
            {
                hidId.Value = id.ToString();
            }
            else
            {
                form1.Visible = false;
            }
        }

        protected void listLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            listCenters.DataBind();

        }

        protected void listCenters_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSessions.DataBind();
        }

        protected void bttnAddSession_Click(object sender, EventArgs e)
        {
            AddSession();
        }

        private void AddSession()
        {
            var entry = new CampaignSession
                {
                    CampaignId = Int32.Parse(hidId.Value),
                    SessionId = Int32.Parse(listSessions.SelectedValue)
                };

            var app = new CampaignSessionService().Add(entry);

            lblStatus.ShowMessage(app);

            if (app.IsDone)
            {
                RegisterScript();
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
    }
}