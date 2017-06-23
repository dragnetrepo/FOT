using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.ext
{
    public class MyContext
    {

        public static FotContext GetContext()
        {
            var service = new ServiceBase();

            return service.Context;
        }
    }
}