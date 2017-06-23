using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Lan.Infrastructure;

namespace Fot.Lan
{
    public partial class None : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Utilities.GetMachineID());
        }
    }
}