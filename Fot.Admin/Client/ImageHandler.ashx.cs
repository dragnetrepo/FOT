using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            int id = Int32.Parse(context.Request.QueryString["id"]);
            string type = context.Request.QueryString["t"];

            byte[] imageBytes = GetImage(type, id);

            context.Response.Buffer = true;
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(imageBytes);
            context.Response.Flush();

        }


        private byte[] GetImage(string type, int id)
        {
            byte[] imageBytes = null;

            switch (type)
            {
                case "b": // assessment bundle description
                    {
                        imageBytes = new AssessmentBundleService().GetDescriptionImage(id);
                        break;
                        
                    }
                case "i": //assessment instruction
                    {
                        imageBytes = new AssessmentService().GetInstructionImage(id);
                        break;

                    }
                case "q": //question content
                    {
                        imageBytes = new AssessmentQuestionService().GetQuestionImage(id);
                        break;

                    }
                case "a": //answer image
                    {
                        imageBytes = new AssessmentAnswerService().GetAnswerImage(id);
                        break;

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