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
    public partial class AddOrEditLevel : System.Web.UI.Page
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

                else if (Int32.TryParse(Request.QueryString["lid"], out tid))
                {
                    LoadLevel(tid);
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
        }

        private void LoadLevel(int id)
        {
            var item = new QuestionDifficultyLevelService().GetLevel(id);

            if (item != null)
            {
                bttnAdd.Visible = false;
                hidTid.Value = id.ToString();
                bttnUpdate.Visible = true;

                this.Title = "Update Difficulty Level";

                txtDifficultyLevel.Text = item.LevelName;
                listScale.SelectedValue = item.LevelWeight.ToString();

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
            AddLevel();
        }

        private void AddLevel()
        {
            var item = new QuestionDifficultyLevel
            {
                LevelName = txtDifficultyLevel.Text,
                AssessmentId = Int32.Parse(hidAid.Value),
                LevelWeight = Int32.Parse(listScale.SelectedValue)
            };

            var ret = new QuestionDifficultyLevelService().Add(item);

            lblStatus.ShowMessage(ret);

            if (ret.IsDone)
            {
                txtDifficultyLevel.Text = string.Empty;

                RegisterScript();


            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateLevel();
        }

        private void UpdateLevel()
        {
            var service = new QuestionDifficultyLevelService();

            var item = service.GetLevel(Int32.Parse(hidTid.Value));

            if (item != null)
            {
                item.LevelName = txtDifficultyLevel.Text;
                item.LevelWeight = Int32.Parse(listScale.SelectedValue);


                var app = service.Update(item);

                if (app.IsDone)
                {
                    RegisterScript();

                }

                lblStatus.ShowMessage(app);

            }
        }
    }
}