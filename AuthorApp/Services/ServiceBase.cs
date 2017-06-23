
using System;
using AuthorApp.Models;
using System.Data.Entity;


namespace AuthorApp.Services
{
    public class ServiceBase : IDisposable
    {
        public FotAuthorContext Context;
       

        public ServiceBase()
        {
            Context = new FotAuthorContext();

          
        }

        public void Dispose()
        {
            Context.Dispose();
        }

       
    }
}