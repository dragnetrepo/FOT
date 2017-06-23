<%@ Page Title="" Language="C#" MasterPageFile="~/TestCenter/CenterMaster.Master" AutoEventWireup="true" CodeBehind="Candidates.aspx.cs" Inherits="Fot.Admin.TestCenter.ViewCandidates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Scheduled Candidates for
        <asp:Label ID="lblDate" runat="server"></asp:Label>
    </h1>
    
    
    <div style="margin-top: 30px; float: left; width: 100%">
        
        <asp:Button ID="bttnDownload" runat="server" Text="Download List" style="float: left; margin-bottom: 3px;" OnClick="bttnDownload_Click"/>
    </div>
    <div style="margin-top: 10px; float: left; width: 100%">
        
        

        
       
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" Skin="Windows7" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" GridLines="None" DataSourceID="ScheduledDataSource" PageSize="20">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="ScheduledDataSource">
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
                    <telerik:GridBoundColumn DataField="Firstname" FilterControlAltText="Filter Firstname column" HeaderText="Firstname" SortExpression="Firstname" UniqueName="Firstname">
  
                    </telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="Lastname" FilterControlAltText="Filter Lastname column" HeaderText="Lastname" UniqueName="Lastname" SortExpression="Lastname">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SessionTime" FilterControlAltText="Filter SessionTime column" HeaderText="SessionTime" UniqueName="SessionTime" SortExpression="SessionTime">
                    </telerik:GridBoundColumn>
           
                </Columns>

                <EditFormSettings>
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                </EditFormSettings>
            </MasterTableView>

            <HeaderStyle Font-Bold="True" />

            <FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>

        <asp:HiddenField ID="hidId" runat="server" />
        <asp:ObjectDataSource ID="ScheduledDataSource" runat="server" MaximumRowsParameterName="maxRows" SelectCountMethod="CountCandidatesScheduledInCenter" SelectMethod="GetCandidatesScheduledInCenter" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.TestSessionService" EnablePaging="True">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="date" PropertyName="Value" Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
