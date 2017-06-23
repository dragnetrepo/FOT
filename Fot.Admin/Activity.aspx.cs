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
    public partial class Activity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }

        protected void listAdmins_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }

        public List<FixedDataSources> GetLogTypes()
        {
            var ctx = new ServiceBase().Context;

            var items = ctx.AccessLogs.Select(x => x.LogEntryType).Distinct().ToList();

            var list = new List<FixedDataSources>();

           // list.Add(new FixedDataSources { Text = "Any" });

            items.ForEach(x => list.Add(new FixedDataSources {Text = x}));


            return list;
        }

        protected void listLogTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }
    }
}