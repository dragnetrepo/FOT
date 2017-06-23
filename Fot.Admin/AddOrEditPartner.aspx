<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOrEditPartner.aspx.cs" Inherits="Fot.Admin.AddOrEditPartner" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">        

        $(document).ready(function () {
            

            $("#bttnAddDeposit").click(function () {

                AddDeposit();
                return false;
            });
            

            $("#chkSelfManaged").change(function () {

                ShowHideCosts();
            });

            ShowHideCosts();
            
        });
        

        function ShowHideCosts() {
            
            var x = $("#chkSelfManaged:checked").val();

           
            if (x == undefined) {

                $("#trPublic").hide();
                $("#trPrivate").hide();
            }
            else {
  
                $("#trPublic").show();
                $("#trPrivate").show();

            }

        }
        


        function AddDeposit() {
            var oWnd = radopen("Dialogs/AddDeposit.aspx?id=<%= hidId.Value %>", "RadWindow1");
            oWnd.center();

        }



        function refreshGrid() {

            var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                 radMgr.ajaxRequest("Rebind");
             }

    </script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1><asp:Literal runat="server" ID="lblHeader">Add Partner</asp:Literal></h1>
    
        <div style="padding: 10px; text-align: center; height: 30px;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>

        <div style="padding: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 200px;">Partner Name</td>
                    <td>
                        <asp:TextBox ID="txtPartnerName" runat="server" Width="400px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPartnerName" CssClass="Formerror" ErrorMessage="Partner name is required">Partner name is required</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Partner Option</td>
                    <td>
                        <asp:CheckBox ID="chkSelfManaged" runat="server" Text="Partner Account Is Managed By Partner Admins" ClientIDMode="Static" />
                    </td>
                </tr>
                <tr id="trPublic" runat="server" ClientIDMode="Static">
                    <td>Cost Per Test (Public center)</td>
                    <td>
                        <asp:TextBox ID="txtTestCostPublic" runat="server" Width="60px"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTestCostPublic" CssClass="Formerror" ErrorMessage="Specify a valid value for cost per test" Operator="DataTypeCheck" Type="Double">Specify a valid value for cost per test</asp:CompareValidator>
                    </td>
                </tr>
                <tr id="trPrivate" runat="server" ClientIDMode="Static" >
                    <td>Cost Per Test (Private center)</td>
                    <td>
                        <asp:TextBox ID="txtTestCostPrivate" runat="server" Width="60px"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtTestCostPrivate" CssClass="Formerror" ErrorMessage="Specify a valid value for cost per test" Operator="DataTypeCheck" Type="Double">Specify a valid value for cost per test</asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add Partner" OnClick="bttnAdd_Click" />
                        <asp:Button ID="bttnUpdate" runat="server" Text="Update" Visible="False" OnClick="bttnUpdate_Click" />
                        <asp:HiddenField ID="hidId" runat="server" />
                    </td>
                </tr>
                </table>
            <hr/>
        </div>
    
    <div id="divManagedPartnerArea" runat="server" Visible="False">
           <div style="padding: 0px; margin-top: 50px;">
        <fieldset style="width: 100%; border: 1px solid #ccc; "><legend style="font-size: 18px; font-weight: bold; padding-left: 10px;">Partner Wallet / Deposit Entries</legend>
            
            <div style="padding: 10px;">
                
                <table style="width: 100%;" cellpadding="4">
                    <tr>
                        <td style="width: 150px;">Wallet Balance</td>
                        <td style="width: 270px;">
                            <asp:Label ID="lblWalletBalance" runat="server" style="font-weight: 700"></asp:Label>
                        </td>
                        <td style="text-align: left">&nbsp;</td>
                    </tr>
                
                    <tr>
                        <td style="width: 150px;">&nbsp;</td>
                        <td style="width: 270px;">&nbsp;</td>
                        <td style="text-align: left">&nbsp;</td>
                    </tr>
                
                    <tr>
                        <td style="width: 150px;"><button ID="bttnAddDeposit" style="margin-left: 5px;margin-bottom: 3px;">Add Deposit</button></td>
                        <td style="width: 270px;">&nbsp;</td>
                        <td style="text-align: left">&nbsp;</td>
                    </tr>
                
                </table>
                
            </div>
            <div style="padding: 10px;">
                
                <telerik:RadGrid ID="GridDeposits" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" AllowAutomaticDeletes="True" PageSize="5" AllowPaging="True" DataSourceID="PartnerWalletDataSource" OnItemDeleted="GridDeposits_ItemDeleted">
                    <ClientSettings>
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
<MasterTableView DataSourceID="PartnerWalletDataSource" DataKeyNames="EntryId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="EntryId" DataType="System.Int32" FilterControlAltText="Filter EntryId column" HeaderText="EntryId" SortExpression="EntryId" UniqueName="EntryId" Display="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Amount" FilterControlAltText="Filter Amount column" HeaderText="Amount" SortExpression="Amount" UniqueName="Amount" DataType="System.Decimal" DataFormatString="{0:#,##0.00}">
            <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Reference" FilterControlAltText="Filter Reference column" HeaderText="Payment Reference" SortExpression="Reference" UniqueName="Reference">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="EntryDate" DataType="System.DateTime" FilterControlAltText="Filter EntryDate column" HeaderText="Date" SortExpression="EntryDate" UniqueName="EntryDate" DataFormatString="{0:dd-MMM-yyyy}">
            <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="EntryAdmin" FilterControlAltText="Filter EntryAdmin column" HeaderText="Entry Admin" SortExpression="EntryAdmin" UniqueName="EntryAdmin">
              <HeaderStyle Width="150px" />
            <ItemStyle Width="150px" />
        </telerik:GridBoundColumn>
                <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this deposit entry?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" >
            <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

                    <HeaderStyle Font-Bold="True" />

                    <PagerStyle CssClass="validate-skip" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
                </telerik:RadGrid>
                
            </div>
            
            

        </fieldset>

    </div>
        
           <div style="padding: 0px; margin-top: 50px;">
        <fieldset style="width: 100%; border: 1px solid #ccc; "><legend style="font-size: 18px; font-weight: bold; padding-left: 10px;">Assigned Assessments</legend>
            
            <div style="padding: 10px;">
                
                <table style="width: 100%;" cellpadding="4">
                    <tr>
                        <td style="width: 150px;">Assessments</td>
                        <td style="width: 270px;">
                            <asp:DropDownList ID="listAssessments" runat="server" DataSourceID="AssessmentsDataSource" DataTextField="Name" DataValueField="AssessmentId">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left"><asp:Button ID="bttnAddAssessment" runat="server" Text="Add Assessment" style="margin-left: 5px;margin-bottom: 3px;" OnClick="bttnAddAssessment_Click"/></td>
                    </tr>
                
                </table>
                
            </div>
            <div style="padding: 10px;">
                
    
                
                <telerik:RadGrid ID="GridAssessments" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" AllowAutomaticDeletes="True" PageSize="5" AllowPaging="True" DataSourceID="PartnerAssignedAssessmentsDataSource" OnItemDeleted="GridAssessments_ItemDeleted">
                    <ClientSettings>
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
<MasterTableView DataSourceID="PartnerAssignedAssessmentsDataSource" DataKeyNames="EntryId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="EntryId" DataType="System.Int32" Display="False" FilterControlAltText="Filter EntryId column" HeaderText="EntryId" SortExpression="EntryId" UniqueName="EntryId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Assessment" SortExpression="Name" UniqueName="Name">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="AssessmentType" DataType="System.Int32" FilterControlAltText="Filter AssessmentType column" HeaderText="Type" SortExpression="AssessmentType" UniqueName="AssessmentType">
         <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
             </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Remove this assessment?" FilterControlAltText="Filter column column" Text="Remove" UniqueName="column" >
            <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

                    <HeaderStyle Font-Bold="True" />

                    <PagerStyle CssClass="validate-skip" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
                </telerik:RadGrid>
                
            </div>
            
            

        </fieldset>

    </div>
        
                   <div style="padding: 0px; margin-top: 50px; margin-bottom: 50px">
        <fieldset style="width: 100%; border: 1px solid #ccc; "><legend style="font-size: 18px; font-weight: bold; padding-left: 10px;">Assigned Centers</legend>
            
            <div style="padding: 10px;">
                
                <table style="width: 100%;" cellpadding="4">
                    <tr>
                        <td style="width: 150px;">Centers</td>
                        <td style="width: 270px;">
                            <asp:DropDownList ID="listCenters" runat="server" DataSourceID="CentersDataSource" DataTextField="CenterName" DataValueField="CenterId">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left"><asp:Button ID="bttnAddCenter" runat="server" Text="Add Center" style="margin-left: 5px;margin-bottom: 3px;" OnClick="bttnAddCenter_Click"/></td>
                    </tr>
                
                </table>
                
            </div>
            <div style="padding: 10px;">
                
    
                
                <telerik:RadGrid ID="GridCenters" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" AllowAutomaticDeletes="True" PageSize="5" AllowPaging="True" DataSourceID="PartnerAssignedCentersDataSource" OnItemDeleted="GridCenters_ItemDeleted">
                    <ClientSettings>
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
<MasterTableView DataSourceID="PartnerAssignedCentersDataSource" DataKeyNames="CenterId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="CenterId" DataType="System.Int32" Display="False" FilterControlAltText="Filter CenterId column" HeaderText="CenterId" SortExpression="CenterId" UniqueName="CenterId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="CenterName" FilterControlAltText="Filter CenterName column" HeaderText="Center" SortExpression="CenterName" UniqueName="CenterName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column" HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName">
         <HeaderStyle Width="150px" />
            <ItemStyle Width="150px" />
        </telerik:GridBoundColumn>
                          <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Remove this center?" FilterControlAltText="Filter column column" Text="Remove" UniqueName="column" >
            <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

                    <HeaderStyle Font-Bold="True" />

                    <PagerStyle CssClass="validate-skip" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
                </telerik:RadGrid>
                
            </div>
            
            

        </fieldset>

    </div>
<telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="300px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
           <asp:ObjectDataSource ID="PartnerWalletDataSource" runat="server" DeleteMethod="Delete" SelectMethod="GetWalletEntries" TypeName="Fot.Admin.Services.PartnerWalletEntryService" EnablePaging="True" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow">
               <DeleteParameters>
                   <asp:Parameter Name="EntryId" Type="Int32" />
               </DeleteParameters>
               <SelectParameters>
                   <asp:ControlParameter ControlID="hidId" Name="PartnerId" PropertyName="Value" Type="Int32" />
               </SelectParameters>
           </asp:ObjectDataSource>
           <asp:ObjectDataSource ID="AssessmentsDataSource" runat="server" SelectMethod="GetUnassignedAssessments" TypeName="Fot.Admin.Services.PartnerAssignedAssessmentService">
               <SelectParameters>
                   <asp:ControlParameter ControlID="hidId" Name="PartnerId" PropertyName="Value" Type="Int32" />
               </SelectParameters>
           </asp:ObjectDataSource>
           <asp:ObjectDataSource ID="PartnerAssignedAssessmentsDataSource" runat="server" DeleteMethod="DeleteAssessmentFromPartner" SelectMethod="GetAssignedAssessments" TypeName="Fot.Admin.Services.PartnerAssignedAssessmentService" EnablePaging="True" MaximumRowsParameterName="maxRows" StartRowIndexParameterName="startRow" SelectCountMethod="Count">
               <DeleteParameters>
                   <asp:Parameter Name="EntryId" Type="Int32" />
               </DeleteParameters>
               <SelectParameters>
                   <asp:ControlParameter ControlID="hidId" Name="PartnerId" PropertyName="Value" Type="Int32" />
               </SelectParameters>
           </asp:ObjectDataSource>
           <asp:ObjectDataSource ID="PartnerAssignedCentersDataSource" runat="server" DeleteMethod="DeleteCenterFromPartner" SelectMethod="GetAssignedCenters" TypeName="Fot.Admin.Services.PartnerAssignedCenterService" EnablePaging="True" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow">
               <DeleteParameters>
                   <asp:Parameter Name="CenterId" Type="Int32" />
               </DeleteParameters>
               <SelectParameters>
                   <asp:ControlParameter ControlID="hidId" Name="PartnerId" PropertyName="Value" Type="Int32" />
               </SelectParameters>
           </asp:ObjectDataSource>
           <asp:ObjectDataSource ID="CentersDataSource" runat="server" SelectMethod="GetUnassignedCenters" TypeName="Fot.Admin.Services.PartnerAssignedCenterService">
               <SelectParameters>
                   <asp:ControlParameter ControlID="hidId" Name="PartnerId" PropertyName="Value" Type="Int32" />
               </SelectParameters>
           </asp:ObjectDataSource>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                  <telerik:AjaxSetting AjaxControlID="GridDeposits">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblWalletBalance" />
                    <telerik:AjaxUpdatedControl ControlID="GridDeposits" />
                </UpdatedControls>
            </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                      <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="lblWalletBalance" />
                          <telerik:AjaxUpdatedControl ControlID="GridDeposits" />
                      </UpdatedControls>
                  </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </div>
</asp:Content>
