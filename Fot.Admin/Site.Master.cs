using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;


namespace Fot.Admin
{

    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // Utilities.CheckSerial();
            
         
            ProcessNavigation();
             
        }


        public void ProcessNavigation()
        {

            var admin = new AdminUserService().GetCurrentAdmin();

            if(admin.IsGlobalAdmin) return;

            AdminPanelBar.Items[0].Visible = admin.CanAuthor;

            AdminPanelBar.Items[1].Visible = admin.CanSchedule;

            AdminPanelBar.Items[2].Visible = admin.CanSchedule;

            AdminPanelBar.Items[3].Visible = admin.HasUsersAccess;

            AdminPanelBar.Items[4].Items[0].Visible = admin.CanSchedule;

            AdminPanelBar.Items[4].Items[1].Visible = false;

            AdminPanelBar.Items[3].Items[4].Visible = admin.IsGlobalAdmin;

            AdminPanelBar.Items[3].Items[1].Visible = (admin.HasCenterUsersAccess || admin.HasPartnerUsersAccess);
          
             
            
            if ((admin.HasCenterUsersAccess || admin.HasPartnerUsersAccess) && !admin.HasUsersAccess)
            {
                AdminPanelBar.Items[3].Visible = true;
                AdminPanelBar.Items[3].Items[1].Visible = true;
                AdminPanelBar.Items[3].Items[0].Visible = false;
                AdminPanelBar.Items[3].Items[2].Visible = false;
                AdminPanelBar.Items[3].Items[3].Visible = false;
                AdminPanelBar.Items[3].Items[4].Visible = false;

                AdminPanelBar.Items[3].ChildGroupHeight = new Unit(40);
            }


            int count = 0;

            if (AdminPanelBar.Items[3].Items[0].Visible) count++;
            if (AdminPanelBar.Items[3].Items[1].Visible) count++;
            if (AdminPanelBar.Items[3].Items[2].Visible) count++;
            if (AdminPanelBar.Items[3].Items[3].Visible) count++;
            if (AdminPanelBar.Items[3].Items[4].Visible) count++;


            AdminPanelBar.Items[3].ChildGroupHeight = new Unit((20 * count) + 20);

        }

       
    
      
    }
}