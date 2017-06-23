<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestDayCapture.aspx.cs" Inherits="Fot.Admin.TestDayCapture" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <h1>Test Day Personnel Photo Capture</h1>
    
    
       <div class="contentDiv" style="padding: 10px; height: 50px;" align="center">
        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
    </div>

    <div style="padding: 0; margin-top: 30px;">
        <div style="float: left; padding: 5px;">Date</div><div style="float: left; padding: 5px;">
            <telerik:RadDatePicker ID="txtDate" runat="server" Culture="en-US" HiddenInputTitleAttibute="Visually hidden input created for functionality purposes." Skin="Windows7" WrapperTableSummary="Table holding date picker control for selection of dates.">
<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" Skin="Windows7"></Calendar>

<DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy" LabelWidth="40%"></DateInput>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
            </telerik:RadDatePicker>
        </div>
        <div style="float: left; padding: 5px; margin-left: 20px;">Test Center</div>
        <div style="float: left; margin-left: 10px; margin-right: 20px;">
            <asp:DropDownList ID="listTestCenter" runat="server" DataSourceID="CenterDataSource" DataTextField="CenterName" DataValueField="CenterId"></asp:DropDownList>
        </div>
        <div>
            <asp:Button ID="bttnShowDetails" runat="server" Text="Show Details" style="margin-bottom: 3px;" OnClick="bttnShowDetails_Click"/>
        </div>
    </div>
    
    
    
    
    <div style="padding: 0; margin-top: 20px;">
        
              <telerik:RadGrid ID="RadGrid1" runat="server"  AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Windows7" Visible="False">

                  <ClientSettings>
                      <Selecting AllowRowSelect="True" />
                  </ClientSettings>

<MasterTableView>
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="EntryId" Display="False" FilterControlAltText="Filter column column" UniqueName="column">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter column1 column" HeaderText="Personnel Name" UniqueName="column1">
            <ItemStyle VerticalAlign="Top" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="PreTestCapturedBy" FilterControlAltText="Filter column4 column" HeaderText="Pre-Test Captured By" UniqueName="column4">
            <ItemStyle VerticalAlign="Top" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="PostTestCapturedBy" FilterControlAltText="Filter column5 column" HeaderText="Post-Test Captured By" UniqueName="column5">
            <ItemStyle VerticalAlign="Top" />
        </telerik:GridBoundColumn>
        <telerik:GridImageColumn DataImageUrlFields="PreFileName" DataImageUrlFormatString="~/staffphotos/{0}" FilterControlAltText="Filter column2 column" HeaderText="Pre-Test Photo" ImageHeight="" ImageWidth="320px" UniqueName="column2">
            <ItemStyle Width="320px" />
        </telerik:GridImageColumn>
        <telerik:GridImageColumn DataImageUrlFields="PostFileName" DataImageUrlFormatString="~/staffphotos/{0}" FilterControlAltText="Filter column3 column" HeaderText="Post-Test Photo" ImageHeight="" ImageWidth="320px" UniqueName="column3">
            <ItemStyle Width="320px" />
        </telerik:GridImageColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

                  <HeaderStyle Font-Bold="True" />

<FilterMenu EnableImageSprites="False"></FilterMenu>

        </telerik:RadGrid>
  </div>
    
    
    <asp:ObjectDataSource ID="CenterDataSource" runat="server" MaximumRowsParameterName="" SelectMethod="GetCenters" StartRowIndexParameterName="" TypeName="Fot.Admin.Services.CenterService">
            <SelectParameters>
                <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>


</asp:Content>
