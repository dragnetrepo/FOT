using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;

namespace Fot.Admin.TestCenter
{

    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.CenterAdmin)]
    public partial class TestSessions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}