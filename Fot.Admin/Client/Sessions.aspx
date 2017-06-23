<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="Sessions.aspx.cs" Inherits="Fot.Admin.Client.ManageSessions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
        <script type="text/javascript">

            $(document).ready(function () {

                $("#bttnAddSession").click(function () {

                    AddSession();
                    return false;
                });


            });

            function AddSession() {
                var oWnd = radopen("Dialogs/AddOrEditSession.aspx", "RadWindow1");
                oWnd.center();

            }



            function UpdateSession(ID) {
                var oWnd = radopen("Dialogs/AddOrEditSession.aspx?id=" + ID, "RadWindow1");
                oWnd.center();

            }


            function refreshGrid() {

                var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

            radMgr.ajaxRequest("Rebind");
        }


     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Test Sessions</h1>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 0px;">
        <button id="bttnAddSession">Add Session</button>
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" Skin="Windows7" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="SessionDataSource" GridLines="None">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="SessionDataSource" DataKeyNames="SessionId">
                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>

                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>

                <Columns>
                    <telerik:GridBoundColumn DataField="SessionId" DataType="System.Int32" Display="False" FilterControlAltText="Filter SessionId column" HeaderText="SessionId" SortExpression="SessionId" UniqueName="SessionId">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CenterName" FilterControlAltText="Filter CenterName column" HeaderText="Center" SortExpression="CenterName" UniqueName="CenterName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TestDate" DataType="System.DateTime" FilterControlAltText="Filter TestDate column" HeaderText="Date" SortExpression="TestDate" UniqueName="TestDate" DataFormatString="{0:dd-MMM-yyyy}">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TimeText" FilterControlAltText="Filter TimeText column" HeaderText="Time" SortExpression="TimeText" UniqueName="TimeText">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
                    </telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="Capacity" FilterControlAltText="Filter column1 column" HeaderText="Capacity" UniqueName="column1">
                               <HeaderStyle Width="60px" />
                               <ItemStyle Width="60px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Scheduled" FilterControlAltText="Filter column2 column" HeaderText="Scheduled" UniqueName="column2">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                    </telerik:GridBoundColumn>
                           <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                        UniqueName="TemplateColumn">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
                        <ItemTemplate>
                            <a href="#" onclick='UpdateSession(<%# Eval("SessionId") %>); return false;'>Edit / Update</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this location?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                    </telerik:GridButtonColumn>
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

        <asp:ObjectDataSource ID="SessionDataSource" runat="server" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetSessions" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.PartnerTestSessionService" DeleteMethod="Delete" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="SessionId" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="300px" Skin="Windows7"
        Width="700px" Modal="True" VisibleStatusbar="False"
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
