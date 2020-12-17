<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OnlineTests.aspx.cs" Inherits="Fot.Admin.Client.OnlineTests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   	<script src='<%: ResolveClientUrl("~/Scripts/jquery.signalR-2.2.2.min.js") %>' type="text/javascript"></script>
    <script src='<%: ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">

        var currentEntryId = null;
        var campaignId = <%= hidId.Value %>;
        

        var chat = {};

        function DoSubmit(entryId) {
            currentEntryId = entryId;

            radconfirm("Submit this candidates assessment?", SubmitEntry);
        }

        function DoSubmitAll() {

            radconfirm("Submit all assessments?", SubmitAll);
        }

        function SubmitAll(flag) {
            if (flag) {
                $('#loader').show();
                $('#lblSubmitStatus').html('Initializing submit process ...');

                chat.server.submitAll(campaignId).done(function () {
                    $('#loader').hide();
                    $('#lblSubmitStatus').html('');
                    var radMgr = $find("<%=RadAjaxManager1.ClientID %>");
                    radMgr.ajaxRequest("Rebind");
                    console.log('Invocation of SubmitAll succeeded');
                }).fail(function (error) {
                    console.log('Invocation of SubmitAll failed. Error: ' + error);
                });
             }
        }

        function SubmitEntry(flag) {

            if (flag) {
                $('#loader').show();
                $('#lblSubmitStatus').html('Initializing submit process ...');

                chat.server.submitEntry(currentEntryId).done(function () {
                    $('#loader').hide();
                    $('#lblSubmitStatus').html('');
                    var radMgr = $find("<%=RadAjaxManager1.ClientID %>");
                    radMgr.ajaxRequest("Rebind");
                    console.log('Invocation of SubmitEntry succeeded');
                }).fail(function (error) {
                    console.log('Invocation of SubmitEntry failed. Error: ' + error);
                });
            }
        }


        $(function () {
            // Reference the auto-generated proxy for the hub.
             chat = $.connection.processHub;
            // Create a function that the hub can call back to display messages.
            chat.client.sendMessage = function (item) {
                // Add the message to the page.
                $('#lblSubmitStatus').html(item);
            };

            //$.connection.hub.url = "https://testhaven.fot.com.ng/signalr";
            // Start the connection.
            $.connection.hub.start().done(function () {

                console.log('connected successfully.');
                //alert('connected successfully');

            }).fail(function () { console.log('connected failed.'); });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Online Tests</h1>

    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px; margin-bottom: 30px;">
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



    <div style="width: 100%; float: right; margin-top: 30px; margin-bottom: 20px;">
        <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px;">

            <tr>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="400px" Style="margin-right: 5px;" placeholder="Search By Username, Firstname, Lastname Or Mobile Number"></asp:TextBox>
                    <asp:Button ID="bttnSearch" runat="server" Text="Search" Style="margin-bottom: 1px" OnClick="bttnSearch_Click" />
                </td>
                <td>
                    <div style="margin-right: 30px; min-width: 450px; float: left;">
                        <div id="loader" style="display: none; margin-right: 10px; float: left">
                            <svg width="30px" height="30px" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid" class="lds-bricks" style="background: none;">
                                <rect ng-attr-fill="{{config.c1}}" ng-attr-x="{{config.x}}" ng-attr-y="{{config.x}}" ng-attr-width="{{config.w}}" ng-attr-height="{{config.w}}" ng-attr-rx="{{config.radius}}" ng-attr-ry="{{config.radius}}" fill="#93dbe9" x="55" y="55" width="30" height="30" rx="3" ry="3">
                                    <animate attributeName="x" calcMode="linear" values="15;55;55;55;55;15;15;15;15" keyTimes="0;0.083;0.25;0.333;0.5;0.583;0.75;0.833;1" dur="2" begin="-1.8333333333333333s" repeatCount="indefinite"></animate>
                                    <animate attributeName="y" calcMode="linear" values="15;55;55;55;55;15;15;15;15" keyTimes="0;0.083;0.25;0.333;0.5;0.583;0.75;0.833;1" dur="2" begin="-1.3333333333333333s" repeatCount="indefinite"></animate>
                                </rect><rect ng-attr-fill="{{config.c2}}" ng-attr-x="{{config.x}}" ng-attr-y="{{config.x}}" ng-attr-width="{{config.w}}" ng-attr-height="{{config.w}}" ng-attr-rx="{{config.radius}}" ng-attr-ry="{{config.radius}}" fill="#689cc5" x="15" y="15" width="30" height="30" rx="3" ry="3"><animate attributeName="x" calcMode="linear" values="15;55;55;55;55;15;15;15;15" keyTimes="0;0.083;0.25;0.333;0.5;0.583;0.75;0.833;1" dur="2" begin="-1.1666666666666667s" repeatCount="indefinite"></animate>
                                    <animate attributeName="y" calcMode="linear" values="15;55;55;55;55;15;15;15;15" keyTimes="0;0.083;0.25;0.333;0.5;0.583;0.75;0.833;1" dur="2" begin="-0.6666666666666666s" repeatCount="indefinite"></animate>
                                </rect><rect ng-attr-fill="{{config.c3}}" ng-attr-x="{{config.x}}" ng-attr-y="{{config.x}}" ng-attr-width="{{config.w}}" ng-attr-height="{{config.w}}" ng-attr-rx="{{config.radius}}" ng-attr-ry="{{config.radius}}" fill="#5e6fa3" x="15" y="55" width="30" height="30" rx="3" ry="3"><animate attributeName="x" calcMode="linear" values="15;55;55;55;55;15;15;15;15" keyTimes="0;0.083;0.25;0.333;0.5;0.583;0.75;0.833;1" dur="2" begin="-0.5s" repeatCount="indefinite"></animate>
                                    <animate attributeName="y" calcMode="linear" values="15;55;55;55;55;15;15;15;15" keyTimes="0;0.083;0.25;0.333;0.5;0.583;0.75;0.833;1" dur="2" begin="0s" repeatCount="indefinite"></animate>
                                </rect></svg>
                        </div>
                        <div id="lblSubmitStatus" style="margin-top: 5px; font-size: 16px;"></div>
                    </div>

                    <button style="margin-bottom: 1px; float: right;" type="button" onclick="DoSubmitAll(); return false;">Submit All</button>
                </td>
            </tr>

        </table>
    </div>

    <div style="width: 100%; float: right; margin-top: 10px;">

        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="StatusDataSource" PageSize="20">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="StatusDataSource" DataKeyNames="CampaignEntryId">
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
                    <telerik:GridBoundColumn DataField="FirstUpdated" FilterControlAltText="Filter FirstUpdated column" HeaderText="First Updated" SortExpression="FirstUpdated" UniqueName="FirstUpdated" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LastUpdated" FilterControlAltText="Filter LastUpdated column" HeaderText="Last Updated" SortExpression="LastUpdated" UniqueName="LastUpdated" DataType="System.DateTime" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" UniqueName="TemplateColumn" HeaderText="Status">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Convert.ToBoolean(Eval("Online")) ? "Online" : "Offline" %>' ForeColor='<%# Convert.ToBoolean(Eval("Online")) ? System.Drawing.Color.Green : System.Drawing.Color.Gray %>' runat="server" Font-Size="Larger" Font-Bold="true"></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="SaveCount" FilterControlAltText="Filter SaveCount column" HeaderText="Save Count" UniqueName="SaveCount" DataType="System.Int32" SortExpression="SaveCount">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CurrentAssessment" FilterControlAltText="Filter CurrentAssessment column" HeaderText="Current Assessment" SortExpression="CurrentAssessment" UniqueName="CurrentAssessment" ReadOnly="True">
                        <HeaderStyle Width="250px" />
                        <ItemStyle Width="250px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TimeRemainingMinutes" FilterControlAltText="Filter TimeRemainingMinutes column" HeaderText="Time Remaining" UniqueName="TimeRemainingMinutes" DataType="System.Int32" SortExpression="TimeRemainingMinutes">
                        <HeaderStyle Width="150px" />
                        <ItemStyle Width="150px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" UniqueName="TemplateColumn">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
                        <ItemTemplate>
                            <a href="javascript:void(0)" onclick='DoSubmit(<%# Eval("CampaignEntryId") %>)'>Submit</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>

                <EditFormSettings>
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                </EditFormSettings>
            </MasterTableView>

            <HeaderStyle Font-Bold="True" />

            <FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
    </div>
    <asp:ObjectDataSource ID="StatusDataSource" runat="server" SelectMethod="GetEntries" TypeName="Fot.admin.OnlineTests" SelectCountMethod="CountEntries" EnablePaging="True" MaximumRowsParameterName="maxRows" StartRowIndexParameterName="startRow">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="searchTerm" PropertyName="Text" Type="String" />
            <asp:QueryStringParameter Name="campaignId" Type="Int32" QueryStringField="id" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:HiddenField ID="hidId" runat="server" />
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="500px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
       <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                            <AjaxSettings>
                                 <telerik:AjaxSetting AjaxControlID="bttnSearch">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
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
</asp:Content>

