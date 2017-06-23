using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Lan.Models;
using Fot.Lan.Services;

namespace Fot.Lan.admin
{
    public partial class Expunge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            FindPhoto();
        }

        private void FindPhoto()
        {
            var ctx = new ServiceBase().Context;

            var entry =
                ctx.Candidates
                    .FirstOrDefault(x => x.Username == txtSearch.Text && x.AssessmentCompleted == false && x.PhotoCapturedByAdminId.HasValue);


            if (entry == null)
            {
                divPhoto.Visible = false;

                lblStatus.ShowMessage(new AppMessage
                {
                    IsDone = false,
                    Message = "No candidate photo with the specified Username has been captured.",
                    Status = MessageStatus.Error
                });
            }

            else
            {
                lblStatus.Text = string.Empty;

                divPhoto.Visible = true;

                img.ImageUrl = "ImageHandler.ashx?id=" + entry.CandidateEntryId;

                hidId.Value = entry.CandidateEntryId.ToString();

            }

        }

        protected void bttnDelete_Click(object sender, EventArgs e)
        {
            RemovePhoto();
        }

        private void RemovePhoto()
        {
            var ctx = new ServiceBase().Context;

            int id = Int32.Parse(hidId.Value);

            var entry =
                ctx.Candidates
                    .FirstOrDefault(x => x.CandidateEntryId == id && x.AssessmentCompleted == false && x.PhotoCapturedByAdminId.HasValue);

            if (entry != null)
            {
                entry.PhotoCapturedByAdminId = null;
                entry.CandidatePhoto = null;

                if (Session["USERID"] != null)
                {

                    var log = new PhotoLog();
                    log.AdminUserId = Int32.Parse(Session["USERID"].ToString());
                    log.CandidateId = entry.CandidateId;
                    log.ExpungeDate = DateTime.Now;

                    ctx.PhotoLogs.Add(log);
                }

                ctx.SaveChanges();

                lblStatus.ShowMessage(new AppMessage
                {
                    IsDone = false,
                    Message = "Candidate photo has been deleted successfully.",
                    Status = MessageStatus.Success
                });

                divPhoto.Visible = false;
            }

        }
    }
}