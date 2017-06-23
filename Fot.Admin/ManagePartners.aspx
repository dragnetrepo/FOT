<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagePartners.aspx.cs" Inherits="Fot.Admin.ManagePartners" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
       

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Partners</h1>
    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 0px; margin-top: 30px;">
        <asp:Button id="bttnAddPartner" runat="server" Text="Add Partner" OnClick="bttnAddPartner_Click" Visible="False"></asp:Button>
        
        
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="PartnerDataSource" GridLines="None" Skin="Windows7">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="PartnerDataSource" DataKeyNames="PartnerId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="PartnerId" DataType="System.Int32" FilterControlAltText="Filter PartnerId column" HeaderText="PartnerId" SortExpression="PartnerId" UniqueName="PartnerId" Display="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="PartnerName" FilterControlAltText="Filter PartnerName column" HeaderText="Partner" SortExpression="PartnerName" UniqueName="PartnerName">
        </telerik:GridBoundColumn>
   
                    <telerik:GridCheckBoxColumn DataField="IsSelfManaged" DataType="System.Boolean" FilterControlAltText="Filter column2 column" HeaderText="Self Managed ?" UniqueName="column2">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
        </telerik:GridCheckBoxColumn>
   
                    <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="PartnerId" DataNavigateUrlFormatString="AddOrEditPartner.aspx?id={0}" FilterControlAltText="Filter column1 column" Text="Edit / Update" UniqueName="column1" Visible="True">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
        </telerik:GridHyperLinkColumn>
   
                    <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this partner?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" Visible="False">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                    </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

            <HeaderStyle Font-Bold="True" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>

    </div>
    
    <div>
        
        
        <asp:ObjectDataSource ID="PartnerDataSource" runat="server" DeleteMethod="Delete" SelectMethod="GetPartners" TypeName="Fot.Admin.Services.PartnerService" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="PartnerId" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="500px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

    </div>

</asp:Content>
