<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Activity.aspx.cs" Inherits="Fot.Admin.Activity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
            <h1>Activity Logs</h1>
    
    <div style="padding: 0; margin-top: 30px;">
        <asp:DropDownList ID="listAdmins" runat="server" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="AdminDatasource" DataTextField="Username" DataValueField="AdminId" OnSelectedIndexChanged="listAdmins_SelectedIndexChanged">
            <asp:ListItem Value="0">All Administrators</asp:ListItem>
        </asp:DropDownList>

        <asp:DropDownList ID="listLogTypes" runat="server" AppendDataBoundItems="True" DataSourceID="LogTypeDatasource" DataTextField="Text" DataValueField="Text" OnSelectedIndexChanged="listLogTypes_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem Value="0">All Activity Types</asp:ListItem>
        </asp:DropDownList>

    </div>
    
      <div style="padding: 0; padding-top: 10px;">
        

          <telerik:RadGrid ID="RadGrid1" runat="server" Skin="Windows7" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="ActivityDataSource" GridLines="None" PageSize="20">
              <ClientSettings>
                  <Selecting AllowRowSelect="True" />
              </ClientSettings>
<MasterTableView DataSourceID="ActivityDataSource">
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
        <telerik:GridBoundColumn DataField="LogEntryType" FilterControlAltText="Filter LogEntryType column" HeaderText="Activity Type" SortExpression="LogEntryType" UniqueName="LogEntryType">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LogEntryDetails" FilterControlAltText="Filter LogEntryDetails column" HeaderText="Log Details" SortExpression="LogEntryDetails" UniqueName="LogEntryDetails">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LogDate" DataType="System.DateTime" FilterControlAltText="Filter LogDate column" HeaderText="Log Date" SortExpression="LogDate" UniqueName="LogDate" DataFormatString="{0:dd-MMM-yyyy @ hh:mm:ss tt}">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="IpAddress" FilterControlAltText="Filter IpAddress column" HeaderText="IP Address" SortExpression="IpAddress" UniqueName="IpAddress">
        </telerik:GridBoundColumn>
 
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

              <HeaderStyle Font-Bold="True" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
          </telerik:RadGrid>
          <asp:ObjectDataSource ID="ActivityDataSource" runat="server" TypeName="Fot.Admin.Services.AccessLogService" MaximumRowsParameterName="maxRows" SelectCountMethod="CountLogs" SelectMethod="GetLogs" StartRowIndexParameterName="startRow" EnablePaging="True">
              <SelectParameters>
                  <asp:ControlParameter ControlID="listAdmins" Name="AdminId" PropertyName="SelectedValue" Type="Int32" />
                  <asp:ControlParameter ControlID="listLogTypes" Name="LogType" PropertyName="SelectedValue" Type="String" />
              </SelectParameters>
          </asp:ObjectDataSource>

          <asp:ObjectDataSource ID="AdminDatasource" runat="server" TypeName="Fot.Admin.Services.AdminUserService" SelectMethod="GetAllAdmins"></asp:ObjectDataSource>
          <asp:ObjectDataSource ID="LogTypeDatasource" runat="server" TypeName="Fot.Admin.Activity" SelectMethod="GetLogTypes"></asp:ObjectDataSource>
    </div>

</asp:Content>
