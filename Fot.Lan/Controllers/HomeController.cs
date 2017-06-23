using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fot.Lan.Models;
using Fot.Lan.Services;

namespace Fot.Lan.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            
            return Redirect("~/Default.aspx");
        }



 
    }
}
