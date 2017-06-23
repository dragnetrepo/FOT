using System;
using System.Data.Entity;
using System.Web;

namespace Context
{
    /// <summary>
    /// Entity Module used to control an Entities DB Context over the lifetime of a request per user.
    /// </summary>
    public class ContextModule : IHttpModule
    {
        private const string DB = ContextManager.DB;


        void context_EndRequest(object sender, EventArgs e)
        {
            Dispose();
        }


        #region IHttpModule Members

        public void Dispose()
        {
            if(HttpContext.Current != null)
            {
                if (HttpContext.Current.Items[DB] != null)
                {
                    var entitiesContext = (DbContext) HttpContext.Current.Items[DB];

                    entitiesContext.Dispose();
                    HttpContext.Current.Items.Remove(DB);

                }

            }
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(context_EndRequest); 
        }

        #endregion

    }
}
