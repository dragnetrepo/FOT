using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fot.Lan;
using Fot.Client.Infrastructure;
using Fot.Client.Models;
using Fot.Client.Services;
using Telerik.Web.UI;

namespace Fot.Client
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // Utilities.CheckSerial();

            if (!Page.IsPostBack)
            {
                ProcessLogout();

                

            }
        }


      

        private void ProcessLogout()
        {
            if (Session["STATUS"] != null)
            {
                var app = (AppMessage) Session["STATUS"];

                lblStatus.ShowMessage(app);

                Session["STATUS"] = null;
            }
        }


        protected void bttnLogin_Click(object sender, EventArgs e)
        {
            DoLogin();
        }

        public void DoLogin()
        {
            var candidateService = new CandidateService();
            var item = candidateService.GetCandidateByUsername(txtUsername.Text);


            if (item != null)
            {
                if (item.Password.Equals(txtPassword.Text))
                {
                    Session["LOGIN"] = true;
                    Session["USERID"] = item.CandidateId;

                    Response.Redirect("Details.aspx");
                }
                else
                {
                    lblStatus.ShowMessage(new AppMessage
                        {
                            IsDone = false,
                            Message = "Invalid username or password",
                            Status = MessageStatus.Error
                        });
                }
            }
            else
            {
                lblStatus.ShowMessage(new AppMessage
                    {
                        IsDone = false,
                        Message = "Invalid username or password",
                        Status = MessageStatus.Error
                    });
            }
        }



      
    }
}