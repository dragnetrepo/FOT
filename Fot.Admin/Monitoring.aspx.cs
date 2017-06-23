using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Schedule)]
    public partial class Monitoring : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                txtDate.SelectedDate = DateTime.Today;
                lblCurrentDate.Text = txtDate.SelectedDate.Value.ToString("dd-MMM-yyyy");

                var data = new TestScheduleService().GetTodaysSchedule(txtDate.SelectedDate.Value);

                var totalScheduled = data.Sum(x => x.TotalScheduled);
                var totalTested = data.Sum(x => x.TotalTested);

                lblTotalScheduled.Text = "<strong>Total Scheduled:</strong> " + totalScheduled.ToString("#,##0");
                lblTotalTested.Text = "<strong>Total Tested:</strong> " + totalTested.ToString("#,##0");

            }
        }



        public List<DailyScheduleViewModel> GetSchedules(DateTime date)
        {
            return new TestScheduleService().GetTodaysSchedule(date);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
            lblCurrentDate.Text = txtDate.SelectedDate.Value.ToString("dd-MMM-yyyy");

            var data = new TestScheduleService().GetTodaysSchedule(txtDate.SelectedDate.Value);

            var totalScheduled = data.Sum(x => x.TotalScheduled);
            var totalTested = data.Sum(x => x.TotalTested);

            lblTotalScheduled.Text = "<strong>Total Scheduled:</strong> " + totalScheduled.ToString("#,##0");
            lblTotalTested.Text = "<strong>Total Tested:</strong> " + totalTested.ToString("#,##0");

        
        } 
    }
}