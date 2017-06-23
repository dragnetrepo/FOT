using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorApp.Infrastructure
{
   public  class Utilities
    {
       public static string FormatHtml(string html)
       {
           var writer = new StringWriter();
           var doc = new HtmlAgilityPack.HtmlDocument();
           doc.LoadHtml(html);

           foreach (var img in doc.DocumentNode.Descendants("img"))
           {
               var fileName = img.Attributes["src"].Value;

               fileName = fileName.Replace(@"file:///", string.Empty);

               fileName = Uri.UnescapeDataString(fileName);


               if (fileName.StartsWith("data:image/")) continue;

               var bytes = File.ReadAllBytes(fileName);

               var base64Str = Convert.ToBase64String(bytes);

               var mimeType = GetMime(Path.GetExtension(fileName));

               img.Attributes["src"].Value = "data:"+ mimeType +";base64," + base64Str;
           }


           doc.Save(writer);

           return writer.ToString();
       }


       private static string GetMime(string ext)
       {

           var str = string.Empty;

           switch (ext)
           {
               case ".jpg":
                   {
                       str = "image/jpeg";
                       break;
                       
                   }
               case ".png":
                   {
                       str = "image/png";
                       break;

                   }
               case ".gif":
                   {
                       str = "image/gif";
                       break;

                   }
               default:
                   {
                       str = "image/jpeg";
                       break;
                       
                   }
           }

           return str;
       }

    }
}
