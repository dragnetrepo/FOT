using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Dialogs
{
    public partial class AddDeposit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForAdd(id);
                }
                else
                {
                    form1.Visible = false;
                }

            }
        }

        private void LoadForAdd(int id)
        {
            var item = new PartnerService().GetPartner(id);
            if (item != null)
            {
                hidPartnerId.Value = item.PartnerId.ToString();
            }
            else
            {
                form1.Visible = false;
            }
        }

        private void Add()
        {
            decimal temp;

            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !decimal.TryParse(txtAmount.Text, out temp)) return;
            
            var walletService = new PartnerWalletEntryService();
            var currentAdmin = new AdminUserService().GetAdminUserByName(User.Identity.Name);


            var item = new PartnerWalletEntry
            {
                PartnerId = Int32.Parse(hidPartnerId.Value),
                Amount = decimal.Parse(txtAmount.Text),
                Reference = txtReference.Text,
                EntryDate = DateTime.Today,
                EntryAdmin = currentAdmin.AdminId
            };

            var app = walletService.Add(item);

            lblStatus.ShowMessage(app);

            if (app.IsDone)
            {
                txtAmount.Text = string.Empty;
                txtReference.Text = string.Empty;
                RegisterScript();
            }

        }

        public void RegisterScript()
        {
           

            String csname1 = "PopupScript";
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.

            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                StringBuilder cstext1 = new StringBuilder();
                // cstext1.Append("alert('Hello World!');");
                var fnName = "refreshGrid";
                cstext1.Append("CallFunctionOnParentPage('" + fnName + "');");


                cs.RegisterStartupScript(cstype, csname1, cstext1.ToString(), true);
            }

        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }
    }
}