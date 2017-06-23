using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Context;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.ext
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            int id = Int32.Parse(context.Request.QueryString["id"]);
            string email = context.Request.QueryString["email"];

            byte[] imageBytes = GetImage(email, id);

            context.Response.Buffer = true;
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(imageBytes);
            context.Response.Flush();

        }


        private byte[] GetImage(string email, int campaignId)
        {
            byte[] imageBytes = null;

            var context = ContextManager.AsSingleton<FotContext>();

            var entry = context.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Candidate.Email.Equals(email))
                               .Select(x => new {EntryId = x.EntryId, CandidateId = x.Candidate.CandidateId})
                               .FirstOrDefault();
            if (entry != null)
            {
                var campaignFolder = UrlMapper.RootPhotosDirectory + campaignId.ToString();

                var imageUrl = Path.Combine(campaignFolder, string.Format("{0}_{1}.jpg", entry.CandidateId, entry.EntryId));

                if (File.Exists(HttpContext.Current.Server.MapPath(imageUrl)))
                {
                    imageBytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageUrl));
                }
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