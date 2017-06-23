using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fot.Client.Controllers
{
    public class TestController : Controller
    {

        public ActionResult Index()
        {
            
            return Redirect("~/Test.aspx");
        }



        public ActionResult Scar()
        {

            return Content("na me oh.");
        }

    }
}
