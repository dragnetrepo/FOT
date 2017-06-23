using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using WebSupergoo.ABCpdf8;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Result)]
    public partial class EssayReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
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
                        Response.Redirect(UrlMapper.ManageCampaigns);
                    }
                }
            }
        }

        private void LoadInitial(int id)
        {
            var item = new AssessmentResultService().GetResultEntry(id);

            if (item != null && item.SelectedEssayId.HasValue)
            {


                var essay = new EssayTopicService().GetTopic(item.SelectedEssayId.Value);


                hidId.Value = item.CampaignEntry.CampaignId.ToString();


                lblEssayTopic.Text = essay.Topic;

                lblEssayContent.Text = item.EssayText;


                lblCandidateName.Text = item.CampaignEntry.Candidate.FirstName + " " + item.CampaignEntry.Candidate.LastName;

                lblAssessmentName.Text = item.Assessment.Name;


            }
            else
            {
                Response.Redirect(UrlMapper.ManageCampaigns);
            }
        }

        protected void bttnBackToResults_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.Results + "?id=" + hidId.Value);
        }

        public void DownloadEssay(int id)
        {
            #region "template"

            var templateHeader = @"<!DOCTYPE html>
<html>
<head>
    <title></title>

    <style type='text/css'>
        html, body {
            font: 14px 'HelveticaNeue', 'Helvetica Neue', Helvetica, Arial, sans-serif;
            color: #444;
        }

        td {
            padding-top: 5px;
            padding-bottom: 5px;
        }
    </style>

</head>


<body>";

    var template = @"<div style='padding: 10px; border: 1px solid #f0f0f0; margin-top: 40px; margin-left: auto; margin-right: auto; width: 1040px;'>
        <table style='width: 1020px;'>
            <tr>
                <td style='width: 200px;'>
                    <strong>Candidate Name</strong>
                </td>

                <td>{0}
                </td>

            </tr>
             <tr>
                <td style='width: 200px;'>
                    <strong>Candidate ID</strong>
                </td>

                <td>{1}
                </td>

            </tr>

            <tr>
                <td>
                    <strong>Assessment Name</strong>
                </td>

                <td>{2}
                </td>

            </tr>

            <tr>
                <td>
                    <strong>Topic</strong>
                </td>

                <td>{3}
                </td>

            </tr>

        </table>

    </div>

    <div style='padding: 10px; border: 1px solid #f0f0f0; margin-top: 40px; margin-left: auto; margin-right: auto;  width: 1040px; min-height: 600px;'>";
      #endregion
	
  var templateFooter = @"</div></body></html>";
      

            var item = new AssessmentResultService().GetResultEntry(id);


            if (item != null && item.SelectedEssayId.HasValue)
            {
                var documents = new Doc();
                documents.Rect.Inset(15, 15);
                documents.HtmlOptions.Engine = EngineType.Gecko;


                var memory = new MemoryStream();

                var candidateName = item.CampaignEntry.Candidate.FirstName + " " + item.CampaignEntry.Candidate.LastName;

                var essay = new EssayTopicService().GetTopic(item.SelectedEssayId.Value);

                var htmlStr = string.Format(template, candidateName, item.CampaignEntry.Candidate.Username, item.Assessment.Name, essay.Topic);

                htmlStr = templateHeader + htmlStr + item.EssayText + templateFooter;

                documents.AddImageHtml(htmlStr);

                documents.Save(memory);

                memory.Position = 0;

                Response.Clear();

                Response.Buffer = true;


                Response.ContentType = "application/pdf";


                Response.AddHeader("content-disposition", "attachment;filename=Essay_Review_" + DateTime.Today.ToString("dd-MMM-yyyy") + ".pdf");


                Response.AddHeader("content-length", memory.Length.ToString());

                Response.BinaryWrite(memory.ToArray());

                documents.Dispose();

                memory.Dispose();

                Response.Flush();
            }


        }



        protected void bttnDownload_Click(object sender, EventArgs e)
        {
            int id = 0;

            if (Int32.TryParse(Request.QueryString["id"], out id))
            {
                DownloadEssay(id);
            }
            else
            {
                Response.Redirect(UrlMapper.PartnerManageCampaigns);
            }
        }
    }
}