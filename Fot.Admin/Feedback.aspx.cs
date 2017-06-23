using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fot.Admin
{
    public partial class Feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void listFeedbackTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }
    }
}