using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Services;

namespace Fot.Admin
{
    public partial class TestDayCapture : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttnShowDetails_Click(object sender, EventArgs e)
        {
            ShowDetails();
        }

        private void ShowDetails()
        {
            if (txtDate.SelectedDate.HasValue == false)
            {
               lblStatus.ShowMessage(new AppMessage{IsDone = true, Message = "No date selected.", Status = MessageStatus.Error}); 
                return;
            }

            var centerId = Int32.Parse(listTestCenter.SelectedValue);

            var date = txtDate.SelectedDate.Value;



            var context = new ServiceBase().Context;

            var items = context.TestDayPhotoes.Where(x => x.TestDate == date && x.AdminId.HasValue && x.AdminUser.CenterId == centerId)
                .Select(x => new TestDayViewModel
                {
                    EntryId = x.EntryId,
                    Name = x.AdminUser.Firstname + " " + x.AdminUser.Lastname,
                    PreFileName = x.PreTestPhotoFileName,
                    PostFileName = x.PostTestPhotoFileName,
                    PreTestCapturedBy = x.PreTestCaptureAdmin.Username,
                    PostTestCapturedBy = x.PostTestCaptureAdmin.Username

                }).ToList();

            var items2 = context.TestDayPhotoes.Where(x => x.TestDate == date && x.CenterUserId.HasValue && x.CenterUser.CenterId == centerId)
                .Select(x => new TestDayViewModel
                {
                    EntryId = x.EntryId,
                    Name = x.CenterUser.Firstname + " " + x.CenterUser.Lastname,
                    PreFileName = x.PreTestPhotoFileName,
                    PostFileName = x.PostTestPhotoFileName,
                    PreTestCapturedBy = x.PreTestCaptureAdmin.Username,
                    PostTestCapturedBy = x.PostTestCaptureAdmin.Username

                }).ToList();

            items.AddRange(items2);


            RadGrid1.Visible = true;

            RadGrid1.DataSource = items;

            RadGrid1.DataBind();

        }
    }


    public class TestDayViewModel
    {
        public int EntryId { get; set; }

        public string Name { get; set; }

        public string PreFileName { get; set; }

        public string PostFileName { get; set; }

        public string PreTestCapturedBy { get; set; }

        public string PostTestCapturedBy { get; set; }
    }
}