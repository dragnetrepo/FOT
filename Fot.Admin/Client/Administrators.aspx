<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="Administrators.aspx.cs" Inherits="Fot.Admin.Client.Administrators" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
         <script type="text/javascript">


             function refreshGrid() {

                 var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                radMgr.ajaxRequest("Rebind");
            }


     </script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Administrators</h1>
      <div class="contentDiv" style="padding: 10px;" align="center">
        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
    </div>
    

    <div style="padding: 0px;">
        <asp:Button id="bttnAddAdmin" Text="Add Administrator" runat="server" OnClick="bttnAddAdmin_Click"/>
        <telerik:RadGrid runat="server" AllowAutomaticDeletes="True" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="AdminUsersDataSource" ID="AdminGrid">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="AdminUsersDataSource">
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

        <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="AdminId" DataNavigateUrlFormatString="AddOrEditAdmin.aspx?id={0}" FilterControlAltText="Filter column1 column" Text="Edit / Update" UniqueName="column1">
        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
             </telerik:GridHyperLinkColumn>
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
    
    <div>
        
        <asp:ObjectDataSource ID="AdminUsersDataSource" runat="server" DeleteMethod="DeleteAdminUser" SelectMethod="GetPartnerAdminUsers" TypeName="Fot.Admin.Services.AdminUserService" MaximumRowsParameterName="maxRows" SelectCountMethod="CountPartnerAdminUsers" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="AdminId" Type="Int32" />
            </DeleteParameters>
         
            <SelectParameters>
                <asp:ControlParameter ControlID="hidPartnerId" Name="PartnerId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
         
        </asp:ObjectDataSource>
        <asp:HiddenField ID="hidPartnerId" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                  <telerik:AjaxSetting AjaxControlID="AdminGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="AdminGrid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                      <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="AdminGrid" />
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
