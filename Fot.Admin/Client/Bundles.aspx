<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="Bundles.aspx.cs" Inherits="Fot.Admin.Client.Bundles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Assessment Bundles</h1>
 
    <div style="padding:0px; padding-bottom: 0px; margin-top: 20px;">
        <asp:Button ID="bttnAdd" runat="server" Text="Add Assessment Bundle" OnClick="bttnAdd_Click" />

    </div>
       <div style=" padding:0px;">
        

           <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AllowAutomaticDeletes="True" AutoGenerateColumns="False" DataSourceID="BundleDataSource">
               <ClientSettings>
                   <Selecting AllowRowSelect="True" />
               </ClientSettings>
<MasterTableView DataSourceID="BundleDataSource" DataKeyNames="BundleId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="BundleId" DataType="System.Int32" Display="False" FilterControlAltText="Filter BundleId column" HeaderText="BundleId" SortExpression="BundleId" UniqueName="BundleId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Assessment Bundle" SortExpression="Name" UniqueName="Name">
        </telerik:GridBoundColumn>
        <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="BundleId" DataNavigateUrlFormatString="AddOrEditBundle.aspx?id={0}" FilterControlAltText="Filter column column" Text="Edit / Update" UniqueName="column">
            <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
        </telerik:GridHyperLinkColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this assessment bundle?" FilterControlAltText="Filter column1 column" Text="Delete" UniqueName="column1">
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
        

           <asp:ObjectDataSource ID="BundleDataSource" runat="server" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetBundles" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.AssessmentBundleService" EnablePaging="True">
               <DeleteParameters>
                   <asp:Parameter Name="BundleId" Type="Int32" />
               </DeleteParameters>
           </asp:ObjectDataSource>
        

    </div>
</asp:Content>
