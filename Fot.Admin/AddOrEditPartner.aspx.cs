using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin
{
    public partial class AddOrEditPartner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // Response.Redirect("ManagePartners.aspx");

            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadPartner(id);
                }


            }

            

        }

        private void LoadPartner(int id)
        {
            var service = new PartnerService();

            var item = service.GetPartner(id);

            if (item != null)
            {
                bttnAdd.Visible = false;
                hidId.Value = id.ToString();
                bttnUpdate.Visible = true;

                lblHeader.Text = "Update Partner";

                txtPartnerName.Text = item.PartnerName;
                chkSelfManaged.Checked = item.IsSelfManaged;

                var currentAdmin = new AdminUserService().GetCurrentAdmin();

              

                if (item.IsSelfManaged)
                {
                  

                    txtTestCostPrivate.Text = item.CostPerTestPrivate.Value.ToString();
                    txtTestCostPublic.Text = item.CostPerTestPublic.Value.ToString();

                    if (currentAdmin.IsGlobalAdmin || currentAdmin.HasFinancialsAccess)
                    {
                        divManagedPartnerArea.Visible = true;
                    }
                    

                    lblWalletBalance.Text = item.WalletBalance.ToString("#,##0.00");
                }

            }

        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddPartner();
        }

        private void AddPartner()
        {
            var item = new Partner
            {
                PartnerName = txtPartnerName.Text

            };

            item.IsSelfManaged = chkSelfManaged.Checked;

            if (chkSelfManaged.Checked)
            {
                decimal costPublic = 0.0m;
                decimal costPrivate = 0.0m;


                if (decimal.TryParse(txtTestCostPublic.Text, out costPublic)  &&
                    decimal.TryParse(txtTestCostPrivate.Text, out costPrivate) && costPublic > 0 && costPrivate > 0)
                {

                    item.CostPerTestPublic = costPublic;
                    item.CostPerTestPrivate = costPrivate;
                }

                else
                {
                    lblStatus.ShowMessage(new AppMessage{IsDone = false, Message = "Please specify valid values for both the 'Cost Per Test' fields.", Status = MessageStatus.Error});
                    return;
                    
                }
            }

            var ret = new PartnerService().Add(item);

            lblStatus.ShowMessage(ret);

            if (ret.IsDone)
            {
                if (chkSelfManaged.Checked)
                {
                    LoadPartner((int)ret.Data);
                }
                else
                {
                    Response.Redirect("ManagePartners");
                }


            }
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdatePartner();
        }

        private void UpdatePartner()
        {
            var service = new PartnerService();

            var item = service.GetPartner(Int32.Parse(hidId.Value));

            if (item != null)
            {
                item.PartnerName = txtPartnerName.Text;

                item.IsSelfManaged = chkSelfManaged.Checked;

                if (chkSelfManaged.Checked)
                {
                    decimal costPublic = 0.0m;
                    decimal costPrivate = 0.0m;


                    if (decimal.TryParse(txtTestCostPublic.Text, out costPublic) &&
                        decimal.TryParse(txtTestCostPrivate.Text, out costPrivate))
                    {
                        item.CostPerTestPublic = costPublic;
                        item.CostPerTestPrivate = costPrivate;
                    }

                    else
                    {
                        lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Please specify valid values for both the 'Cost Per Test' fields.", Status = MessageStatus.Error });
                        return;

                    }
                }

            

                var app = service.Update(item);

                if (app.IsDone)
                {
                    if (chkSelfManaged.Checked)
                    {
                        LoadPartner(Int32.Parse(hidId.Value));
                    }
                    else
                    {
                        Response.Redirect("ManagePartners");
                    }
                }


                lblStatus.ShowMessage(app);

            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument.Equals("Rebind"))
            {
                GridDeposits.DataBind();

                var service = new PartnerService();

                var item = service.GetPartner(Int32.Parse(hidId.Value));

                lblWalletBalance.Text = item.WalletBalance.ToString("#,##0.00");

            }
        }

        protected void GridDeposits_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
        {
            var service = new PartnerService();

            var item = service.GetPartner(Int32.Parse(hidId.Value));

            lblWalletBalance.Text = item.WalletBalance.ToString("#,##0.00");
        }

        protected void bttnAddAssessment_Click(object sender, EventArgs e)
        {
            AddAssessment();
        }

        private void AddAssessment()
        {

            if(listAssessments.SelectedIndex < 0) return;

            var service = new PartnerAssignedAssessmentService();


            int assessmentId = Int32.Parse(listAssessments.SelectedValue);
            int partnerId = Int32.Parse(hidId.Value);

               

            var app = service.AssignAssessmentToPartner(assessmentId, partnerId);

            GridAssessments.DataBind();
            listAssessments.DataBind();


        }

        private void AddCenter()
        {

            if (listCenters.SelectedIndex < 0) return;

            var service = new PartnerAssignedCenterService();


            int centerId = Int32.Parse(listCenters.SelectedValue);
            int partnerId = Int32.Parse(hidId.Value);



            var app = service.AssignCenterToPartner(centerId, partnerId);

            GridCenters.DataBind();
            listCenters.DataBind();


        }

        protected void GridAssessments_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
        {
            listAssessments.DataBind();
        }

        protected void bttnAddCenter_Click(object sender, EventArgs e)
        {
            AddCenter();
        }

        protected void GridCenters_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
        {
            listCenters.DataBind();
        }
    }
}