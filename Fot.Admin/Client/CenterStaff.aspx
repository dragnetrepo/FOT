<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CenterStaff.aspx.cs" Inherits="Fot.Admin.Client.CenterStaff" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
             <script type="text/javascript">

             $(document).ready(function () {

                 $("#bttnAddCaptureAdmin").click(function () {

                     AddCaptureAdmin();
                     return false;
                 });
                 
                 $("#bttnAddSupportStaff").click(function () {

                     AddSupportStaff();
                     return false;
                 });



             });

             function AddCaptureAdmin() {
                 var oWnd = radopen("Dialogs/AddCaptureAdmin.aspx?id=<%= hidId.Value %>", "RadWindow1");
                 oWnd.center();

             }
             
             function AddSupportStaff() {
                 var oWnd = radopen("Dialogs/AddSupportStaff.aspx?id=<%= hidId.Value %>", "RadWindow1");
                 oWnd.center();

             }



             function UpdateCaptureAdmin(ID) {
                 var oWnd = radopen("Dialogs/UpdateCaptureAdmin.aspx?id=" + ID, "RadWindow1");
                 oWnd.center();

             }
             

             function UpdateSupportStaff(ID) {
                 var oWnd = radopen("Dialogs/UpdateSupportStaff.aspx?id=" + ID, "RadWindow1");
                 oWnd.center();

             }



             function refreshGrid() {

                 var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                 radMgr.ajaxRequest("Rebind");
             }


     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <h1>Test Center Personnel - <asp:Literal ID="lblCenterName" runat="server"></asp:Literal></h1>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>   <div style="padding: 0;">
        
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Windows7">
            <Tabs>
                <telerik:RadTab runat="server" Selected="True" Text="Photo Capture Administrators">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Support Staff">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">
            <telerik:RadPageView ID="RadPageView1" runat="server">
                
                  <div style="padding: 0px; margin-top: 20px;">
        <button id="bttnAddCaptureAdmin">Add Capture Admin</button>
        <telerik:RadGrid runat="server" AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CaptureAdminDataSource" ID="PartnerGrid">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="CaptureAdminDataSource" DataKeyNames="AdminId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="AdminId" DataType="System.Int32" Display="False" FilterControlAltText="Filter AdminId column" HeaderText="AdminId" SortExpression="AdminId" UniqueName="AdminId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Username" FilterControlAltText="Filter Username column" HeaderText="Username" SortExpression="Username" UniqueName="Username">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Firstname" FilterControlAltText="Filter Firstname column" HeaderText="First Name" SortExpression="Firstname" UniqueName="Firstname">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Lastname" FilterControlAltText="Filter Lastname column" HeaderText="Last Name" SortExpression="Lastname" UniqueName="Lastname">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="MobileNo" FilterControlAltText="Filter MobileNo column" HeaderText="Mobile No" SortExpression="MobileNo" UniqueName="MobileNo">
        </telerik:GridBoundColumn>
        <telerik:GridCheckBoxColumn DataField="Active" DataType="System.Boolean" FilterControlAltText="Filter Active column" HeaderText="Active" SortExpression="Active" UniqueName="Active">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridCheckBoxColumn>
        <telerik:GridBoundColumn DataField="LastLoginDate" DataFormatString="{0:dd-MMM-yyyy}" DataType="System.DateTime" FilterControlAltText="Filter LastLoginDate column" HeaderText="Last Login" SortExpression="LastLoginDate" UniqueName="LastLoginDate">
            <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
        </telerik:GridBoundColumn>
         <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                        UniqueName="TemplateColumn">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
                        <ItemTemplate>
                            <a href="#" onclick='UpdateCaptureAdmin(<%# Eval("AdminId") %>); return false;'>Edit / Update</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this user?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" ConfirmDialogType="RadWindow">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

            <HeaderStyle Font-Bold="True" />

            <PagerStyle Mode="Slider" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
    </div>

            </telerik:RadPageView>
             <telerik:RadPageView ID="RadPageView2" runat="server">
                 
                          <div style="padding: 0px; margin-top: 20px;">
        <button id="bttnAddSupportStaff">Add Support Staff</button>
        <telerik:RadGrid runat="server" AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="SupportStaffDataSource" ID="CenterGrid">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="SupportStaffDataSource" DataKeyNames="UserId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="UserId" DataType="System.Int32" Display="False" FilterControlAltText="Filter UserId column" HeaderText="UserId" SortExpression="UserId" UniqueName="UserId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column" HeaderText="Email" SortExpression="Email" UniqueName="Email">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Firstname" FilterControlAltText="Filter Firstname column" HeaderText="First Name" SortExpression="Firstname" UniqueName="Firstname">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Lastname" FilterControlAltText="Filter Lastname column" HeaderText="Last Name" SortExpression="Lastname" UniqueName="Lastname">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="MobileNo" FilterControlAltText="Filter MobileNo column" HeaderText="Mobile No" SortExpression="MobileNo" UniqueName="MobileNo">
        </telerik:GridBoundColumn>
        <telerik:GridCheckBoxColumn DataField="Active" DataType="System.Boolean" FilterControlAltText="Filter Active column" HeaderText="Active" SortExpression="Active" UniqueName="Active">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridCheckBoxColumn>
         <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                        UniqueName="TemplateColumn">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
                        <ItemTemplate>
                            <a href="#" onclick='UpdateSupportStaff(<%# Eval("UserId") %>); return false;'>Edit / Update</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this user?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" ConfirmDialogType="RadWindow">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

            <HeaderStyle Font-Bold="True" />

            <PagerStyle Mode="Slider" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
    </div>

             </telerik:RadPageView>
        </telerik:RadMultiPage>
    </div>

  
    
    <div>
        
        <asp:ObjectDataSource ID="CaptureAdminDataSource" runat="server" DeleteMethod="DeleteAdminUser" SelectMethod="GetCaptureAdmins" TypeName="Fot.Admin.Services.AdminUserService">
            <DeleteParameters>
                <asp:Parameter Name="AdminId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="CenterId" QueryStringField="id" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <asp:ObjectDataSource ID="SupportStaffDataSource" runat="server" DeleteMethod="DeleteStaff" SelectMethod="GetAllStaff" TypeName="Fot.Admin.Services.SupportStaffService">
            <DeleteParameters>
                <asp:Parameter Name="UserId" Type="Int32" />
            </DeleteParameters>
              <SelectParameters>
                <asp:QueryStringParameter Name="CenterId" QueryStringField="id" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                  <telerik:AjaxSetting AjaxControlID="PartnerGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PartnerGrid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="CenterGrid">
                      <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="CenterGrid" />
                      </UpdatedControls>
                  </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                      <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="PartnerGrid" />
                          <telerik:AjaxUpdatedControl ControlID="CenterGrid" />
                      </UpdatedControls>
                  </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="420px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
        <asp:HiddenField ID="hidId" runat="server" />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

    </div>
    
    


</asp:Content>
