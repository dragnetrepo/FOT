using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.FriendlyUrls;

namespace Fot.Admin.Infrastructure
{
    public class MyFriendlyUrlResolver : WebFormsFriendlyUrlResolver, IFriendlyUrlResolver
    {
        protected override IList<string> GetExtensions(HttpContextBase httpContext)
        {
            if (httpContext.Request.RawUrl.Contains("Telerik.Web.UI.DialogHandler.aspx"))
            {
                return null;
            }
            return base.GetExtensions(httpContext);
        }

        bool IFriendlyUrlResolver.TryConvertToFriendlyUrl(string path, out string friendlyUrl)
        {
            if (path.Contains("Telerik.Web.UI.DialogHandler.aspx"))
            {
                friendlyUrl = path;
                return false;
            }

            return base.TryConvertToFriendlyUrl(path, out friendlyUrl);
        }
    }
}