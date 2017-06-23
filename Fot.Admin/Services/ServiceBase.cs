using Context;
using Fot.Admin.Models;
using System.Data.Entity;


namespace Fot.Admin.Services
{
    public class ServiceBase
    {
        public FotContext Context;
       

        public ServiceBase()
        {
            Context = ContextManager.AsSingleton<FotContext>();

          
        }

     

       
    }
}