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
    public partial class AddOrEditTopic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int tid = 0;
                int aid = 0;

                if (Int32.TryParse(Request.QueryString["aid"], out aid))
                {
                    LoadForAdd(aid);
                }

                else if (Int32.TryParse(Request.QueryString["tid"], out tid))
                {
                    LoadTopic(tid);
                }
            }
        }

        private void LoadForAdd(int aid)
        {
            var item = new AssessmentService().GetAssessment(aid);

            if(item != null)
            {
                hidAid.Value = aid.ToString();

            }
        }

        private void LoadTopic(int id)
        {
            var item = new AssessmentTopicService().GetTopic(id);

            if(item != null)
            {
                bttnAdd.Visible = false;
                hidTid.Value = id.ToString();
                bttnUpdate.Visible = true;

                this.Title = "Update Assessment Topic";

                txtTopic.Text = item.Topic;

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
            if(string.IsNullOrWhiteSpace(txtTopic.Text)) return;
            AddTopic();
        }

        private void AddTopic()
        {
            var item = new AssessmentTopic
                {
                    Topic = txtTopic.Text,
                    AssessmentId = Int32.Parse(hidAid.Value)
                };

            var ret = new AssessmentTopicService().Add(item);

            lblStatus.ShowMessage(ret);

            if (ret.IsDone)
            {
                txtTopic.Text = string.Empty;

                RegisterScript();
              

            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTopic.Text)) return;
            UpdateTopic();
        }

        private void UpdateTopic()
        {
            var service = new AssessmentTopicService();

            var item = service.GetTopic(Int32.Parse(hidTid.Value));

            if(item != null)
            {
                item.Topic = txtTopic.Text;

                var app = service.Update(item);

                if(app.IsDone)
                {
                    RegisterScript();
                    
                }

                lblStatus.ShowMessage(app);

            }
        }
    }
}