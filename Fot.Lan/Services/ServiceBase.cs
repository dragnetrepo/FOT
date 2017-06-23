using System.Data.Entity;
using Fot.Lan.Models;
using StructureMap;


namespace Fot.Lan.Services
{
    public class ServiceBase
    {
        public LanContext Context;
       

        public ServiceBase()
        {
            Context = ObjectFactory.GetInstance<LanContext>();

          



        }

     

       
    }
}