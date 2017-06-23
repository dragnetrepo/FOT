<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MessageLog.aspx.cs" Inherits="Fot.Admin.MessageLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <h1>Email Log</h1>
    
       
           
    <div style="padding: 0; margin-top: 20px;">
        
              <telerik:RadGrid ID="RadGrid1" runat="server"  AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Windows7" AllowPaging="True" DataSourceID="MessageLogDataSource" PageSize="20">
                  
<MasterTableView DataSourceID="MessageLogDataSource">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="BatchId" DataType="System.Int32" Display="False" FilterControlAltText="Filter BatchId column" HeaderText="BatchId" SortExpression="BatchId" UniqueName="BatchId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="BatchDate" DataFormatString="{0:dd-MMM-yyyy}" DataType="System.DateTime" FilterControlAltText="Filter BatchDate column" HeaderText="Date" SortExpression="BatchDate" UniqueName="BatchDate">
            <HeaderStyle Width="120px" />
            <ItemStyle Width="120px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Subject" FilterControlAltText="Filter Subject column" HeaderText="Subject" SortExpression="Subject" UniqueName="Subject">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Queued" DataFormatString="{0:#,##0}" DataType="System.Int32" FilterControlAltText="Filter Queued column" HeaderText="Queued" SortExpression="Queued" UniqueName="Queued">
            <HeaderStyle Width="150px" />
            <ItemStyle Width="150px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Sent" DataFormatString="{0:#,##0}" DataType="System.Int32" FilterControlAltText="Filter Sent column" HeaderText="Sent" SortExpression="Sent" UniqueName="Sent">
            <HeaderStyle Width="150px" />
            <ItemStyle Width="150px" />
        </telerik:GridBoundColumn>
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
         
        
        
    <asp:ObjectDataSource ID="MessageLogDataSource" runat="server" EnablePaging="True" TypeName="Fot.Admin.MessageLog" SelectMethod="GetMessageLog" SelectCountMethod="GetMessageLogCount" MaximumRowsParameterName="maxRows" StartRowIndexParameterName="startRow"></asp:ObjectDataSource>

    
</asp:Content>
