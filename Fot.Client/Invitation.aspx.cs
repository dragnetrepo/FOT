using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Client.Infrastructure;
using Fot.Client.Models;
using Fot.Client.Services;
using WebSupergoo.ABCpdf8;

namespace Fot.Client
{
    public partial class Invitation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;

                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    var hide = Request.QueryString["hide"];
                    if (hide == "1")
                    {
                        LoadSchedule2(id);
                    }
                    else
                    {
                        LoadSchedule(id);
                    }
                    
                }
                else
                {
                    Response.Redirect("Details.aspx");
                }

            }
        }

        private void LoadSchedule2(int id)
        {
           

            var schedule = new CandidateService().GetCandidateInvitation(id);

            if (schedule != null )
            {
                lblFullName.Text = schedule.Fullname;
                lblUsername.Text = schedule.Username;
                lblPassword.Text = schedule.Password;
                lblLocation.Text = schedule.Location;
                lblCenterName.Text = schedule.CenterName;
                lblCenterAddress.Text = schedule.Address;
                lblSessionDateTime.Text = string.Format("{0} @ {1}", schedule.TestDate?.ToString("dd-MMM-yyyy"), schedule.TimeText);

                lblInstructions.Text = schedule.Instructions;

                divResponse.Visible = !schedule.HasResponded;

                if (string.IsNullOrEmpty(schedule.PhotoFileName))
                {
                    divInvitation.Visible = false;
                }
                else
                {
                    divPhoto.Visible = false;

                    divPDF.Visible = true;

                    img.ImageUrl = "~/photos/" + schedule.PhotoFileName;

                    divInvitation.Visible = true;

                    if (schedule.LogoPlacement.HasValue)
                    {

                        if (schedule.LogoPlacement == 1)
                        {
                            imgLeft.Visible = true;

                            imgLeft.ImageUrl = "~/photos/" + "dragnet_logo.png";

                            ImgRight.Visible = false;
                        }
                        else if (schedule.LogoPlacement == 2)
                        {
                            imgLeft.Visible = true;
                            imgLeft.ImageUrl = "~/photos/" + "dragnet_logo.png";

                            ImgRight.Visible = true;

                            ImgRight.ImageUrl = "~/photos/" + schedule.LogoFileName;

                        }
                        else if (schedule.LogoPlacement == 3)
                        {
                            imgLeft.Visible = true;

                            imgLeft.ImageUrl = "~/photos/" + schedule.LogoFileName;


                            ImgRight.Visible = false;

                        }
                    }
                    else
                    {
                        imgLeft.ImageUrl = "~/photos/" + "dragnet_logo.png";
                    }


                    var flag = Request.QueryString["hide"];

                    if (flag == "1")
                    {
                        divPDF.Visible = false;
                    }
                }



            }
            else
            {
                Response.Redirect("Details.aspx");
            }
      
        }

        private void LoadSchedule(int id)
        {
            if (Session["USERID"] != null)
            {
                int userid = Int32.Parse(Session["USERID"].ToString());

                hidId.Value = userid.ToString();

                var schedule = new CandidateService().GetCandidateInvitation(id);

                if (schedule != null && schedule.CandidateId == userid)
                {
                    lblFullName.Text = schedule.Fullname;
                    lblUsername.Text = schedule.Username;
                    lblPassword.Text = schedule.Password;
                    lblLocation.Text = schedule.Location;
                    lblCenterName.Text = schedule.CenterName;
                    lblCenterAddress.Text = schedule.Address;
                    lblSessionDateTime.Text = string.Format("{0} @ {1}", schedule.TestDate?.ToString("dd-MMM-yyyy"),schedule.TimeText);

                    lblInstructions.Text = schedule.Instructions;

                    divResponse.Visible = !schedule.HasResponded;

                    if (string.IsNullOrEmpty(schedule.PhotoFileName))
                    {
                        divInvitation.Visible = false;
                    }
                    else
                    {
                        divPhoto.Visible = false;

                        divPDF.Visible = true;

                        img.ImageUrl = "~/photos/" + schedule.PhotoFileName;

                        divInvitation.Visible = true;

                        if (schedule.LogoPlacement.HasValue)
                        {

                            if (schedule.LogoPlacement == 1)
                            {
                                imgLeft.Visible = true;

                                imgLeft.ImageUrl = "~/photos/" + "dragnet_logo.png";

                                ImgRight.Visible = false;
                            }
                            else if (schedule.LogoPlacement == 2)
                            {
                                imgLeft.Visible = true;
                                imgLeft.ImageUrl = "~/photos/" + "dragnet_logo.png";

                                ImgRight.Visible = true;

                                ImgRight.ImageUrl = "~/photos/" + schedule.LogoFileName;

                            }
                            else if (schedule.LogoPlacement == 3)
                            {
                                imgLeft.Visible = true;

                                imgLeft.ImageUrl = "~/photos/" + schedule.LogoFileName;


                                ImgRight.Visible = false;

                            }
                        }
                        else
                        {
                            imgLeft.ImageUrl = "~/photos/" + "dragnet_logo.png";
                        }


                        var flag = Request.QueryString["hide"];

                        if (flag == "1")
                        {
                            divPDF.Visible = false;
                        }
                    }

           

                }
                else
                {
                    Response.Redirect("Details.aspx");
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void bttnAccept_Click(object sender, EventArgs e)
        {

            int id = int.Parse(Request.QueryString["id"]);

            var ctx = new ServiceBase().Context;

            var entry =
                ctx.CampaignEntries.Where(x => x.EntryId == id)
                    .Include(x => x.CandidateScheduleResponse)
                    .FirstOrDefault();

            if (entry != null)
            {
                if (entry.CandidateScheduleResponse == null)
                {
                    entry.CandidateScheduleResponse = new CandidateScheduleResponse {AcceptSchedule = true, RejectReason = string.Empty, DateResponded = DateTime.Today};

                    ctx.SaveChanges();

                    LoadSchedule(id);
                }
                
            }

        }

        protected void bttnReject_Click(object sender, EventArgs e)
        {
            int id = int.Parse(Request.QueryString["id"]);

            var ctx = new ServiceBase().Context;

            var entry =
                ctx.CampaignEntries.Where(x => x.EntryId == id)
                    .Include(x => x.CandidateScheduleResponse)
                    .FirstOrDefault();

            if (entry != null)
            {
                if (entry.CandidateScheduleResponse == null)
                {

                    var reason = txtReason.Text.Length > 200 ? txtReason.Text.Substring(0, 200) : txtReason.Text;

                    entry.CandidateScheduleResponse = new CandidateScheduleResponse { AcceptSchedule = false, RejectReason = reason, DateResponded = DateTime.Today };

                    ctx.SaveChanges();

                    LoadSchedule(id);
                }

            }

        }

        protected void bttnPDF_Click(object sender, EventArgs e)
        {
            DoPDFDownload();
        }



        private void DoPDFDownload()
        {
            int id = Int32.Parse(Request.QueryString["id"]);

            var documents = new Doc();
            documents.Rect.Inset(15, 15);



            // documents.HtmlOptions.Engine = EngineType.Gecko;


            var memory = new MemoryStream();

            documents.Page = documents.AddPage();

            var uri = HttpContext.Current.Request.Url;
            var host = uri.GetLeftPart(UriPartial.Authority) + HostingEnvironment.ApplicationVirtualPath;

            var url = host + "/Invitation.aspx";


            int theID;
            theID = documents.AddImageUrl(url + "?id=" + id + "&hide=1");



            while (true)
            {

                if (!documents.Chainable(theID))
                    break;
                documents.Page = documents.AddPage();
                theID = documents.AddImageToChain(theID);
            }



            for (int i = 1; i <= documents.PageCount; i++)
            {
                documents.PageNumber = i;
                documents.Flatten();
            }


            documents.Save(memory);

            memory.Position = 0;

            Response.Clear();

            Response.Buffer = true;


            Response.ContentType = "application/pdf";


            Response.AddHeader("content-disposition", "attachment;filename=Test_Invitation_" + DateTime.Today.ToString("dd-MMM-yyyy") + ".pdf");


            Response.AddHeader("content-length", memory.Length.ToString());

            Response.BinaryWrite(memory.ToArray());

            documents.Dispose();

            memory.Dispose();

            Response.Flush();



        }

       
    
    }
}