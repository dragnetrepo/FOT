using System.Data.Entity;
using System.Web;
using Fot.Admin.Models;

namespace Context
{
    public static class ContextManager
    {
        internal const string DB = "MY_DB_CONTEXT";

        /// <summary>
        /// Get an instance that lives for the life time of the request per user and automatically disposes.
        /// </summary>
        /// <returns>Model</returns>  
        public static T AsSingleton<T>() where T : DbContext, new()
        {
            HttpContext.Current.Items[DB] = (T)HttpContext.Current.Items[DB] ?? new T();
            return (T)HttpContext.Current.Items[DB];
        }

   
    }
}
