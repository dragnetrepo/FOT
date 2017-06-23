using System;
using System.Collections.Generic;
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

    public partial class Test : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
           // Utilities.CheckSerial();

            if (!Page.IsPostBack)
            {
               
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


                    var list = candidateService.GetCandidateAssessments(item.CandidateId);

                    if (list.Count > 0)
                    {
                            trLogin.Visible = false;
                            trTakeTest.Visible = true;

                        ListView1.DataSource = list;
                        ListView1.DataBind();


                    }
                    else
                    {
                        lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "You are not currently scheduled for an assessment.", Status = MessageStatus.Error });
                    }



                }
                else
                {
                    lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Invalid username or password", Status = MessageStatus.Error });
                }
            }
            else
            {
                lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Invalid username or password", Status = MessageStatus.Error });
            }
        }


    }
}