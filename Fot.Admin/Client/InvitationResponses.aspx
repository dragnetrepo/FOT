<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="InvitationResponses.aspx.cs" Inherits="Fot.Admin.Client.InvitationResponses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Invite Responses</h1>
    
       <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Campaign:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblCampaignName" runat="server"></asp:Literal>
        </div>
        <div style="float: right">
            <asp:Button ID="bttnBackToCampaignDetails" runat="server" Text="Return To Campaign Details" OnClick="bttnBackToCampaignDetails_Click" CssClass="myButton" />
        </div>
    </div>
    
       <div style="padding: 10px; height: 30px;">
          <div style="float: right">
            <asp:Button ID="bttnDownload" runat="server" Text="Download" CssClass="myButton" OnClick="bttnDownload_Click" />
        </div>
    </div>
    
     <asp:HiddenField ID="hidId" runat="server" />
    
    
    
    
    <div style="margin-top: 20px;">
        
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CandidateResponsesDataSource">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="CandidateResponsesDataSource" DataKeyNames="EntryId">
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
        <telerik:GridBoundColumn DataField="Username" FilterControlAltText="Filter Username column" HeaderText="Username" SortExpression="Username" UniqueName="Username">
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="UniqueID" FilterControlAltText="Filter UniqueID column" HeaderText="Unique ID" SortExpression="UniqueID" UniqueName="UniqueID">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column" HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column" HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter column column" HeaderText="Location" UniqueName="column">
        </telerik:GridBoundColumn>
       <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column" HeaderText="Email" SortExpression="Email" UniqueName="Email">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="MobileNo" FilterControlAltText="Filter MobileNo column" HeaderText="Mobile No" SortExpression="MobileNo" UniqueName="MobileNo">
            <HeaderStyle Width="120px" />
            <ItemStyle Width="120px" />
        </telerik:GridBoundColumn>
        <telerik:GridCheckBoxColumn DataField="Accepted" DataType="System.Boolean" FilterControlAltText="Filter Scheduled column" HeaderText="Accepted ?" SortExpression="Accepted" UniqueName="Accepted">
            <HeaderStyle Width="90px" />
            <ItemStyle Width="90px" />
        </telerik:GridCheckBoxColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

            <HeaderStyle Font-Bold="True" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
    </div>
    
           <asp:ObjectDataSource ID="CandidateResponsesDataSource" runat="server" SelectMethod="GetCandidatesResponses" TypeName="Fot.Admin.Services.CampaignEntryService" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="CountCandidatesResponses" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="EntryId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CampaignId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource> 
</asp:Content>
