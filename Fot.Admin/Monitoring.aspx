<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Monitoring.aspx.cs" Inherits="Fot.Admin.Monitoring" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .boxDiv
        {
            width: 400px;
            height: 195px;
            border: 1px solid #E0E0E0;
            background-color: #fff;
            margin: 10px;
            padding: 5px;
            float: left;
        }

            .boxDiv:hover
            {
                border: 1px solid #E0E0E0;
                background-color: #edf6fd;
                cursor: pointer;
                color: black;
            }

        .locationHeader
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            font-weight: bold;
            color: #666;
            text-transform: uppercase;
        }

        .itemText
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 13px;
            font-weight: normal;
            color: #666;
            text-transform: uppercase;
            float: left;
            width: 200px;
        }

        .itemValue
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: bold;
            color: #666;
            text-transform: uppercase;
            float: left;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Monitoring - Overview by Date
        (<asp:Literal ID="lblCurrentDate" runat="server"></asp:Literal>)
    </h1>


    <div style="padding: 0; margin-top: 30px;">
        <div style="float: left; padding: 5px;">Date</div><div style="float: left; padding: 5px;">
            <telerik:RadDatePicker ID="txtDate" runat="server" Culture="en-US" HiddenInputTitleAttibute="Visually hidden input created for functionality purposes." Skin="Windows7" WrapperTableSummary="Table holding date picker control for selection of dates.">
<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" Skin="Windows7"></Calendar>

<DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy" LabelWidth="40%"></DateInput>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
            </telerik:RadDatePicker>
        </div>
        <div>
            <asp:Button ID="Button1" runat="server" Text="View" style="margin-bottom: 3px;" OnClick="Button1_Click"/>
        </div>
    </div>
    
    <div style="padding: 0; margin-top: 20px;">
        <telerik:RadGrid ID="RadGrid1" runat="server"  AutoGenerateColumns="False" CellSpacing="0" GridLines="None" DataSourceID="ObjectDataSource1" Skin="Windows7">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="ObjectDataSource1">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="CenterId" DataType="System.Int32" Display="False" FilterControlAltText="Filter CenterId column" HeaderText="CenterId" SortExpression="CenterId" UniqueName="CenterId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="CenterName" FilterControlAltText="Filter CenterName column" HeaderText="Center" SortExpression="CenterName" UniqueName="CenterName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column" HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="TotalScheduled" DataType="System.Int32" FilterControlAltText="Filter TotalScheduled column" HeaderText="Total Scheduled" SortExpression="TotalScheduled" UniqueName="TotalScheduled" DataFormatString="{0:#,##0}">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="TotalTested" DataType="System.Int32" FilterControlAltText="Filter TotalTested column" HeaderText="Total Tested" SortExpression="TotalTested" UniqueName="TotalTested" DataFormatString="{0:#,##0}">
        </telerik:GridBoundColumn>
        <telerik:GridCheckBoxColumn DataField="DownloadedSchedule" DataType="System.Boolean" FilterControlAltText="Filter DownloadedSchedule column" HeaderText="Downloaded Schedule?" SortExpression="DownloadedSchedule" UniqueName="DownloadedSchedule">
        </telerik:GridCheckBoxColumn>
        <telerik:GridCheckBoxColumn DataField="TriggeredEndOfDay" DataType="System.Boolean" FilterControlAltText="Filter TriggeredEndOfDay column" HeaderText="Triggered EndOfDay?" SortExpression="TriggeredEndOfDay" UniqueName="TriggeredEndOfDay">
        </telerik:GridCheckBoxColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

            <HeaderStyle Font-Bold="True" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetSchedules" TypeName="Fot.Admin.Monitoring">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDate" Name="date" PropertyName="SelectedDate" Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    
    
    
    <div class="row">
        <br/><br/>
        <asp:Literal ID="lblTotalScheduled" runat="server"></asp:Literal> <br/>

        <asp:Literal ID="lblTotalTested" runat="server"></asp:Literal>
    </div>
</asp:Content>
