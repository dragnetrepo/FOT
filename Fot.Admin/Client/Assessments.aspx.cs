using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Client
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Author)]
    public partial class Assessments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.AddOrEditAssessment);
        }

        protected void bttnImportAssessment_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.ImportAssessment);
        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }
    }
}