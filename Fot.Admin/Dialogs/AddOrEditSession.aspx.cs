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
    public partial class AddOrEditSession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;

                txtDate.MinDate = DateTime.Today;

                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForUpdate(id);
                }


            }
        }

        private void LoadForUpdate(int id)
        {
            var item = new TestSessionService().GetSession(id);

            if (item != null)
            {
                hidId.Value = id.ToString();

                listCenters.SelectedValue = item.CenterId.ToString();
                txtDate.SelectedDate = item.TestDate;
                listSessionTime.SelectedValue = item.TimeIndex.ToString();


                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                this.Title = "Update Session";

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
            Page.Validate();

            if(Page.IsValid)
            {
                 AddSession();
            }
           
        }

        private void AddSession()
        {
            var item = new TestSession
            {
                CenterId = Int32.Parse(listCenters.SelectedValue),
                TestDate = txtDate.SelectedDate.Value,
                TimeIndex = Int32.Parse(listSessionTime.SelectedValue),
                TimeText = listSessionTime.SelectedItem.Text
              
            };



            var app = new TestSessionService().Add(item);

            lblStatus.ShowMessage(app);
            if (app.IsDone)
            {

                RegisterScript();
            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateSession();
        }

        private void UpdateSession()
        {
            var sessionService = new TestSessionService();

            var item = sessionService.GetSession(Int32.Parse(hidId.Value));

            if (item != null)
            {

                item.CenterId = Int32.Parse(listCenters.SelectedValue);
                item.TestDate = txtDate.SelectedDate.Value;
                item.TimeIndex = Int32.Parse(listSessionTime.SelectedValue);
                item.TimeText = listSessionTime.SelectedItem.Text;

                var app = sessionService.Update(item);

                lblStatus.ShowMessage(app);

                if (app.IsDone)
                {
                    RegisterScript();

                }
            }
        }
    }
}