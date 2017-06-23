using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Context;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using Glimpse.AspNet.Tab;

namespace Fot.Admin.ext
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageByIdHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string id = context.Request.QueryString["id"];
  

            byte[] imageBytes = GetImage(id);

            context.Response.Buffer = true;
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(imageBytes);
            context.Response.Flush();

        }


        private byte[] GetImage(string canonicalId)
        {
            byte[] imageBytes = null;

            var context = ContextManager.AsSingleton<FotContext>();

            var entry = context.CampaignEntries.Where(x => x.Candidate.ClientUniqueID == canonicalId)
                               .Select(x => new {EntryId = x.EntryId, CandidateId = x.Candidate.CandidateId, CampaignId = x.CampaignId})
                               .FirstOrDefault();
            if (entry != null)
            {
                var campaignFolder = UrlMapper.RootPhotosDirectory + entry.CampaignId.ToString();

                var imageUrl = Path.Combine(campaignFolder,
                    string.Format("{0}_{1}.jpg", entry.CandidateId, entry.EntryId));

                if (File.Exists(HttpContext.Current.Server.MapPath(imageUrl)))
                {
                    imageBytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageUrl));
                }
                else
                {
                    var photoUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/ext"), "no_pic_captured.jpg");

                    imageBytes = File.ReadAllBytes(photoUrl);
                }
            }
            else
            {
                var photoUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/ext"), "invalid_candidate_id.jpg");

                imageBytes = File.ReadAllBytes(photoUrl);
            }

            return imageBytes;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}