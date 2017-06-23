using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Client.Models;
using Fot.Client.Services;

namespace Fot.Client
{
    public partial class PhotoUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Page.IsPostBack)
            {
                LoadSchedule();
            }
        }


        private void LoadSchedule()
        {
            if (Session["USERID"] != null)
            {
                int userid = Int32.Parse(Session["USERID"].ToString());



                var context = new CandidateService().Context;

                var candidate = context.Candidates.Find(userid);



                if (candidate != null)
                {



                    if (!string.IsNullOrEmpty(candidate.PhotoFileName))
                    {

                        divPhotoUploaded.Visible = true;

                        img.ImageUrl = "~/photos/" + candidate.PhotoFileName;

                       

                    }


                }

            }
        
        }


        protected void bttnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile && UploadIsValid())
            {
                UploadFile();
            }
        }

        private void UploadFile()
        {


            var fileName = System.Guid.NewGuid().ToString().Replace("-", string.Empty) + ".jpg";
            var path = Server.MapPath("~/photos");
            var fullPath = Path.Combine(path, fileName);

            FileUpload1.SaveAs(fullPath);

            var service = new ServiceBase();
            int candidateId = Int32.Parse(Session["USERID"].ToString());


            var item = service.Context.Candidates.FirstOrDefault(x => x.CandidateId == candidateId);



            item.PhotoFileName = fileName;


            service.Context.SaveChanges();

            lblStatus.ShowMessage(new AppMessage
            {
                IsDone = true,
                Message = "Photo uploaded successfully.",
                Status = MessageStatus.Success
            });


            LoadSchedule();

        }


        public bool UploadIsValid()
        {
            var file = FileUpload1.PostedFile;
            var app = new AppMessage();

            if (file == null || file.ContentLength < 2)
            {
                app = new AppMessage()
                {
                    IsDone = false,
                    Message = "No image file was selected.",
                    Status = MessageStatus.Error
                };

                lblStatus.ShowMessage(app);

                return false;
            }
            var fileExt = Path.GetExtension(file.FileName.ToLower());
            if (!fileExt.Equals(".jpg") && !fileExt.Equals(".jpeg"))
            {
                app = new AppMessage()
                {
                    IsDone = false,
                    Message = "Only Jpeg files (.jpg, .jpeg) are allowed.",
                    Status = MessageStatus.Error
                };

                lblStatus.ShowMessage(app);

                return false;
            }

            if (file.ContentLength > 204800)
            {
                app = new AppMessage()
                {
                    IsDone = false,
                    Message = "File should not exceed 200kb in size.",
                    Status = MessageStatus.Error
                };

                lblStatus.ShowMessage(app);

                return false;
            }



            var img = System.Drawing.Image.FromStream(file.InputStream);

            if (img.Width != 450 && img.Height != 450)
            {
                app = new AppMessage()
                {
                    IsDone = false,
                    Message = "Image width and height should be 450px by 450px.",
                    Status = MessageStatus.Error
                };

                lblStatus.ShowMessage(app);

                return false;
            }



            return true;
        }
    }
}