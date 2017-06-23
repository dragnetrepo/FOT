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
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.CenterUsers)]
    public partial class CenterStaff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadInitial(id);
                }
                else
                {
                    Response.Redirect("TestCenters.aspx");
                }
            }

        }

        private void LoadInitial(int id)
        {
            var item = new CenterService().GetCenter(id);

            if (item != null)
            {
                lblCenterName.Text = item.CenterName;

                hidId.Value = id.ToString();

            }
            else
            {
                Response.Redirect("TestCenters.aspx");
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument.Equals("Rebind"))
            {
                PartnerGrid.DataBind();
                CenterGrid.DataBind();
            }
        }
    }
}