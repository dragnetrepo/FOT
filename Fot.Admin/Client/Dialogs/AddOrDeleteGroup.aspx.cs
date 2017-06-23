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
    public partial class AddOrDeleteGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
                int aid = 0;

                if (Int32.TryParse(Request.QueryString["aid"], out aid))
                {
                    LoadForAdd(aid);
                }
                else
                {
                    form1.Visible = false;
                }

               
            }
        }

        private void LoadForAdd(int aid)
        {
            var item = new PartnerAssessmentService().GetAssessment(aid);

            if (item != null)
            {
                hidAid.Value = aid.ToString();

            }
            else
            {
                form1.Visible = false;
            }
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddGroup();
        }

        private void AddGroup()
        {
            var item = new QuestionGroup
                {
                    AssessmentId = Int32.Parse(hidAid.Value),
                    GroupName = txtGroupName.Text
                };

            var app = new QuestionGroupService().Add(item);

            if(app.IsDone)
            {
                RadGrid1.DataBind();
                txtGroupName.Text = string.Empty;
                RegisterScript();

            }

            lblStatus.ShowMessage(app);


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
                var fnName = "refreshGroups";
                cstext1.Append("CallFunctionOnParentPage('" + fnName + "');");


                cs.RegisterStartupScript(cstype, csname1, cstext1.ToString(), true);
            }

        }

        protected void RadGrid1_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
        {
            RegisterScript();
        }
    }
}