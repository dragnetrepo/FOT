<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Site.Master" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="Fot.Lan.admin.Status" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <h1>Ongoing Assessments Status</h1>
    
    
    <div style="width: 100%; float: right; margin-top: 30px;">
             <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px;">
        
            <tr>
                <td style="width: 430px; padding-left: 3px;">Search By Username, Firstname, Lastname Or Mobile Number</td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="335px" style="margin-right: 5px;"></asp:TextBox>
                    <asp:Button ID="bttnSearch" runat="server" Text="Search"  style="margin-bottom: 1px" OnClick="bttnSearch_Click"  />
                </td>
               
            </tr>
        
        </table>
    </div>
    
    <div style="width: 100%; float: right; margin-top: 10px;">
        
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="StatusDataSource" PageSize="20">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="StatusDataSource">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="Username" FilterControlAltText="Filter Username column" HeaderText="Username" SortExpression="Username" UniqueName="Username">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Firstname" FilterControlAltText="Filter Firstname column" HeaderText="Firstname" SortExpression="Firstname" UniqueName="Firstname">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Lastname" FilterControlAltText="Filter Lastname column" HeaderText="Lastname" SortExpression="Lastname" UniqueName="Lastname">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="TimeStarted" FilterControlAltText="Filter TimeStarted column" HeaderText="Date Started" SortExpression="TimeStarted" UniqueName="TimeStarted1" DataType="System.DateTime"  DataFormatString="{0:dd-MMM-yyyy}">
        <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
             </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="TimeStarted" FilterControlAltText="Filter TimeStarted column" HeaderText="Time Started" SortExpression="TimeStarted" UniqueName="TimeStarted2" DataType="System.DateTime"  DataFormatString="{0:hh:mm tt}">
        <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
             </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="SaveCount" FilterControlAltText="Filter SaveCount column" HeaderText="Save Count" UniqueName="SaveCount" DataType="System.Int32" SortExpression="SaveCount">
            <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LastSaveStr" FilterControlAltText="Filter LastSaveStr column" HeaderText="Last Saved" SortExpression="LastSaveStr" UniqueName="LastSaveStr" ReadOnly="True">
         <HeaderStyle Width="250px" />
            <ItemStyle Width="250px" />
        </telerik:GridBoundColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

            <HeaderStyle Font-Bold="True" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
        </div>
    <asp:ObjectDataSource ID="StatusDataSource" runat="server" SelectMethod="GetStatusList" TypeName="Fot.Lan.admin.Status" SelectCountMethod="CountStatus" EnablePaging="True" MaximumRowsParameterName="maxRows" StartRowIndexParameterName="startRow">
                 <SelectParameters>
                <asp:ControlParameter ControlID="txtSearch" Name="searchTerm" PropertyName="Text" Type="String" />
            </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
