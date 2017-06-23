using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Fot.Admin.Infrastructure
{
    /// <summary>
    /// Summary description for Html2ImageBinary
    /// </summary>
    public class Html2ImageBinary
    {

        private string html;
        private byte[] image_bytes;

        public Html2ImageBinary(string html)
        {
            this.html = html;
            this.image_bytes = new byte[0];
            //
            // TODO: Add constructor logic here
            //
        }

        public  byte[] GetImage()
        {

            while (image_bytes.Length < 10)
            {

                Thread m_thread = new Thread(new ThreadStart(GetImageFromHtmlString));
                m_thread.SetApartmentState(ApartmentState.STA);
                m_thread.Start();
                m_thread.Join();

            }

            return image_bytes;


        }


        public void GetImageFromHtmlString()
        {

            WebBrowser wb = new WebBrowser();
            //wb.AllowWebBrowserDrop = false;
            // wb.Url = new Uri("http://localhost/faceoftesting/");


            wb.ScrollBarsEnabled = false;


            wb.DocumentText = this.html;

            wb.ScriptErrorsSuppressed = true;

            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);

            while (wb.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }

            wb.Dispose();
         

        }

        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            WebBrowser wb = (WebBrowser)sender;

            int width = -1;
            int height = -1;

            // Set the size of the WebBrowser control
            wb.Width = width;
            wb.Height = height;

            if (width == -1)
            {
                // Take Screenshot of the web pages full width
                wb.Width = wb.Document.Body.ScrollRectangle.Width;
            }

            if (height == -1)
            {
                // Take Screenshot of the web pages full height
                wb.Height = wb.Document.Body.ScrollRectangle.Height;
            }

            // Get a Bitmap representation of the webpage as it's rendered in the WebBrowser control
            Bitmap bitmap = new Bitmap(wb.Width, wb.Height);
            wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
       

            MemoryStream ms = new MemoryStream();

            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

      

            image_bytes = ms.ToArray();
        }
    }
}
