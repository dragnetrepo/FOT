using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    public partial class ManageCandidates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }

   
    }
}