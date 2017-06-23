<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OtherAdmins.aspx.cs" Inherits="Fot.Admin.OtherAdmins" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
         <script type="text/javascript">

             $(document).ready(function () {

                 $("#bttnAddPartnerAdmin").click(function () {

                     AddPartnerAdmin();
                     return false;
                 });
                 
                 $("#bttnAddCenterAdmin").click(function () {

                     AddCenterAdmin();
                     return false;
                 });


                 $("#bttnAddPartnerAdmin").hide();
                 $("#bttnAddCenterAdmin").hide();

             });

             function AddPartnerAdmin() {
                 var oWnd = radopen("Dialogs/AddOrEditPartnerAdmin.aspx", "RadWindow1");
                 oWnd.center();

             }
             
             function AddCenterAdmin() {
                 var oWnd = radopen("Dialogs/AddOrEditCenterAdmin.aspx", "RadWindow1");
                 oWnd.center();

             }



             function UpdatePartnerAdmin(ID) {
                 var oWnd = radopen("Dialogs/AddOrEditPartnerAdmin.aspx?id=" + ID, "RadWindow1");
                 oWnd.center();

             }
             

             function UpdateCenterAdmin(ID) {
                 var oWnd = radopen("Dialogs/AddOrEditCenterAdmin.aspx?id=" + ID, "RadWindow1");
                 oWnd.center();

             }



             function refreshGrid() {

                 var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                 radMgr.ajaxRequest("Rebind");
             }


     </script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Partner/Center Administrators</h1>
      <div class="contentDiv" style="padding: 10px;" align="center">
        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
    </div>
    
    <div style="padding: 0;">
        
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Windows7">
            <Tabs>
                <telerik:RadTab runat="server" Selected="True" Text="Partner Administrators">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Center Administrators">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">
            <telerik:RadPageView ID="RadPageView1" runat="server">
                
                  <div style="padding: 0px; margin-top: 20px;">
        <button id="bttnAddPartnerAdmin">Add Partner Admin</button>
        <telerik:RadGrid runat="server" AllowAutomaticDeletes="True" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="PartnerDataSource" ID="PartnerGrid">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="PartnerDataSource" DataKeyNames="AdminId">
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
        <telerik:GridBoundColumn DataField="PartnerName" FilterControlAltText="Filter column1 column" HeaderText="Partner" UniqueName="column1">
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
                        UniqueName="TemplateColumn" Visible="False">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
                        <ItemTemplate>
                            <a href="#" onclick='UpdatePartnerAdmin(<%# Eval("AdminId") %>); return false;'>Edit / Update</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this user?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" ConfirmDialogType="RadWindow" Visible="False">
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
        <button id="bttnAddCenterAdmin">Add Center Admin</button>
        <telerik:RadGrid runat="server" AllowAutomaticDeletes="True" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CenterDataSource" ID="CenterGrid">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="CenterDataSource" DataKeyNames="AdminId">
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
        <telerik:GridBoundColumn DataField="CenterName" FilterControlAltText="Filter column1 column" HeaderText="Center" UniqueName="column1">
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
                        UniqueName="TemplateColumn" Visible="False">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
                        <ItemTemplate>
                            <a href="#" onclick='UpdateCenterAdmin(<%# Eval("AdminId") %>); return false;'>Edit / Update</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this user?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" ConfirmDialogType="RadWindow" Visible="False">
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
        
        <asp:ObjectDataSource ID="PartnerDataSource" runat="server" DeleteMethod="DeleteAdminUser" SelectMethod="GetPartnerAdmins" TypeName="Fot.Admin.Services.AdminUserService" MaximumRowsParameterName="maxRows" StartRowIndexParameterName="startRow" SelectCountMethod="CountPartnerAdmins" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="AdminId" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        
        <asp:ObjectDataSource ID="CenterDataSource" runat="server" DeleteMethod="DeleteAdminUser" SelectMethod="GetCenterAdmins" TypeName="Fot.Admin.Services.AdminUserService" MaximumRowsParameterName="maxRows" SelectCountMethod="CountCenterAdmins" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="AdminId" Type="Int32" />
            </DeleteParameters>
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
        ID="RadWindowManager1" runat="server" Height="500px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

    </div>
</asp:Content>
