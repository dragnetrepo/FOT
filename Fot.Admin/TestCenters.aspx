<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestCenters.aspx.cs" Inherits="Fot.Admin.TestCenters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">

        $(document).ready(function () {

            $("#bttnAddCenter").click(function () {

                AddCenter();
                return false;
            });


            $("#bttnAddCenter").hide();

        });

        function AddCenter() {
            var oWnd = radopen("Dialogs/AddOrEditCenter.aspx", "RadWindow1");
            oWnd.center();

        }

   

        function UpdateCenter(ID) {
            var oWnd = radopen("Dialogs/AddOrEditCenter.aspx?id=" + ID, "RadWindow1");
            oWnd.center();

        }


        function refreshGrid() {

            var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

             radMgr.ajaxRequest("Rebind");
         }


     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Test Centers</h1>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 0px;">
        <button id="bttnAddCenter">Add Center</button>
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" Skin="Windows7" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="CenterDataSource" GridLines="None">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="CenterDataSource" DataKeyNames="CenterId">
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
                    <telerik:GridBoundColumn DataField="LocationName" DataType="System.String" FilterControlAltText="Filter LocationName column" HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CapacityPerSession" DataType="System.Int32" FilterControlAltText="Filter CapacityPerSession column" HeaderText="Capacity" SortExpression="CapacityPerSession" UniqueName="CapacityPerSession">
                    </telerik:GridBoundColumn>
                    <telerik:GridCheckBoxColumn DataField="Active" DataType="System.Boolean" FilterControlAltText="Filter Active column" HeaderText="Active?" SortExpression="Active" UniqueName="Active">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                    </telerik:GridCheckBoxColumn>
                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                        UniqueName="TemplateColumn" Visible="False">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
                        <ItemTemplate>
                            <a href="#" onclick='UpdateCenter(<%# Eval("CenterId") %>); return false;'>Edit / Update</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this location?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" Visible="False">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                    </telerik:GridButtonColumn>
                    <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="CenterId" DataNavigateUrlFormatString="CenterStaff.aspx?id={0}" FilterControlAltText="Filter column1 column" Text="Center Staff" UniqueName="column1">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
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

        <asp:ObjectDataSource ID="CenterDataSource" runat="server" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetCenters" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.CenterService" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="CenterId" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="400px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
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
