<%@ Page Title="" Language="C#" MasterPageFile="~/TestCenter/CenterMaster.Master" AutoEventWireup="true" CodeBehind="Schedules.aspx.cs" Inherits="Fot.Admin.TestCenter.TestSessions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Upcoming Schedules By Date</h1>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 0px;">
       
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" Skin="Windows7" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="SessionDataSource" GridLines="None" PageSize="20">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="SessionDataSource">
                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>

                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>

                <Columns>
                    <telerik:GridBoundColumn DataField="Date" DataFormatString="{0:dd-MMM-yyyy}" DataType="System.DateTime" FilterControlAltText="Filter Date column" HeaderText="Date" SortExpression="Date" UniqueName="Date">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TotalScheduled" DataType="System.Int32" FilterControlAltText="Filter TotalScheduled column" HeaderText="Total Scheduled" SortExpression="Total Scheduled" UniqueName="TotalScheduled">
                    </telerik:GridBoundColumn>
                    <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="Date" DataNavigateUrlFormatString="Candidates.aspx?id={0:ddMMyyyy}" FilterControlAltText="Filter column column" Text="View Candidates" UniqueName="column">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
                    </telerik:GridHyperLinkColumn>
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

        <asp:ObjectDataSource ID="SessionDataSource" runat="server" MaximumRowsParameterName="maxRows" SelectCountMethod="CountScheduledCandidatesForCenter" SelectMethod="GetScheduledCandidatesForCenter" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.TestSessionService" EnablePaging="True">

        </asp:ObjectDataSource>

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
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
    </div>
</asp:Content>
