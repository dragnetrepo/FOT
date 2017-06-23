﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;

namespace Fot.Admin.Client
{
   [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
   [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Schedule)]
    public partial class TestCenters : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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