using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Lan.Services;

namespace Fot.Lan.Admin
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            int id = Int32.Parse(context.Request.QueryString["id"]);

            byte[] imageBytes = GetImage(id);

            context.Response.Buffer = true;
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(imageBytes);
            context.Response.Flush();

        }


        private byte[] GetImage(int id)
        {
            byte[] imageBytes = null;

            var ctx = new ServiceBase().Context;
            imageBytes = ctx.Candidates.Find(id)?.CandidatePhoto;
                      
                        
      
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