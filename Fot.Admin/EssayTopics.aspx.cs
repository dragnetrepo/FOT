using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Author)]
    public partial class EssayTopics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    CheckAssessment(id);
                }
                else
                {
                    Response.Redirect(UrlMapper.Assessments);
                }


            }
        }

        private void CheckAssessment(int id)
        {
            var item = new AssessmentService().GetAssessment(id);

            if (item == null)
            {
                Response.Redirect(UrlMapper.Assessments);
            }
            else
            {
                hidId.Value = id.ToString();
                lblAssessmentName.Text = item.Name;

            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if(e.Argument.Equals("Rebind"))
            {
                RadGrid1.DataBind();
            }
        }
    }
}