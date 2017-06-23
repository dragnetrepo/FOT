using Context;
using System.Data.Entity;
using System.EnterpriseServices.Internal;
using Fot.Client.Models;
using StructureMap;


namespace Fot.Client.Services
{
    public class ServiceBase
    {
        public LanContext Context;
       

        public ServiceBase()
        {
            Context = ObjectFactory.GetInstance<LanContext>();

            // Context = ContextManager.AsSingleton<LanContext>();


        }

   

       
    }
}