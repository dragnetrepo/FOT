using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;


namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Author)]
    public partial class Questions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    CheckQuestion(id);
                }
                else
                {
                    Response.Redirect(UrlMapper.Assessments);
                }

               
            }

           
        }

        private void CheckQuestion(int id)
        {
            var item = new AssessmentService().GetAssessment(id);

            if(item == null)
            {
                Response.Redirect(UrlMapper.Assessments);
            }
            else
            {
                hidId.Value = id.ToString();
                lblAssessmentName.Text = item.Name;

            }
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.AddOrEditQuestion + "?aid="+ hidId.Value);
        }

        protected void RadListView1_ItemCommand(object sender, Telerik.Web.UI.RadListViewCommandEventArgs e)
        {
            if(e.CommandName.Equals("Edit"))
            {
                var id = Int32.Parse(e.CommandArgument.ToString());

                Response.Redirect(UrlMapper.AddOrEditQuestion + "?qid=" + id);

            }

            else if (e.CommandName.Equals("Delete"))
            {
                var id = Int32.Parse(e.CommandArgument.ToString());
                var questionService = new AssessmentQuestionService();

                if (questionService.IsValidAfterDelete(id))
                {
                    questionService.Delete(id);
                    RadListView1.DataBind();

                    Response.Redirect(UrlMapper.Questions + "?id=" + Int32.Parse(hidId.Value));
                }
                else
                {
                    var item = new AssessmentService().GetAssessment(Int32.Parse(hidId.Value));

                    if (item.AdvancedOutputOptions)
                    {
                        lblStatus.ShowMessage(new AppMessage
                            {
                                IsDone = false,
                                Message =
                                    "Deleting this question would invalidate a config entry in the <strong>Advanced Retrieval Options</strong> page.",
                                Status = MessageStatus.Error
                            });
                    }
                    else
                    {
                        lblStatus.ShowMessage(new AppMessage
                        {
                            IsDone = false,
                            Message =
                                "Deleting this question would invalidate the <strong>Questions Per Test</strong> value in the <strong>Assessment Edit/Update</strong> page.",
                            Status = MessageStatus.Error
                        });
                    }
                }

               
            }
        }

        protected void RadListView1_ItemDeleting(object sender, Telerik.Web.UI.RadListViewCommandEventArgs e)
        {
           
        }

        protected void listValidity_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadListView1.DataBind();
        }
    }
}