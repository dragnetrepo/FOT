using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Services;

namespace Fot.Admin
{
    public partial class ShowQuestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadQuestion();
        }


        public void LoadQuestion()
        {
            var ctx = new ServiceBase().Context;

            var id = Int32.Parse(Request.QueryString["id"]);

            var item = ctx.AssessmentQuestions.Find(id);

            if (item == null)
            {
                form1.Visible = false;

                return;
            }

            RadBinaryImage1.DataValue = item.QuestionImage;



        }
    }
}