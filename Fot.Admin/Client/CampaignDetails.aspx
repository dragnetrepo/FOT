<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="CampaignDetails.aspx.cs" Inherits="Fot.Admin.Client.CampaignDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .boxDiv {
            width: 200px;
            height: 30px;
            border: 1px solid #E0E0E0;
            background-color: #fff;
            margin: 20px;
            margin-left: 10px;
            padding: 10px;
            float: left;
            padding-top: 20px;
        }

            .boxDiv:hover {
                border: 1px solid #E0E0E0;
                background-color: #edf6fd;
                cursor: pointer;
                color: black;
            }

        .codeStyle {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 18px;
            font-weight: bold;
            color: #666;
            text-transform: uppercase;
            text-align: center;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {

            $("#bttnAddSession").click(function () {

                AddSession();
                return false;
            });



        });

        function AddSession() {
            var oWnd = radopen("Dialogs/AddCampaignSession.aspx?id=<%= hidId.Value %>", "RadWindow1");
            oWnd.center();

        }





        function refreshGrid() {

            var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

            radMgr.ajaxRequest("Rebind");
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Manage Campaign</h1>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 10px; width: 98%; float: left;">
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Windows7">
            <Tabs>
                <telerik:RadTab runat="server" Selected="True" Text="Campaign Details">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Campaign Instructions">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Campaign Sessions">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Campaign Logos">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" Height="340px" Width="100%" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server">
                <div style="padding: 10px; border: 1px solid #ccc; margin-top: 10px; height: 430px; width: 100%; float: left;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 150px;">Campaign Name</td>
                            <td>
                                <asp:TextBox ID="txtCampaignName" runat="server" Width="400px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCampaignName" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">Assessment Bundle</td>
                            <td>
                                <asp:DropDownList ID="listAssessmentBundles" runat="server" DataSourceID="BundleDataSource" DataTextField="Name" DataValueField="BundleId">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="listAssessmentBundles" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">Campaign Type</td>
                            <td>
                                <strong>
                                    <asp:Label ID="lblCampaignType" runat="server"></asp:Label></strong>
                            </td>
                        </tr>
                        <tr id="trStartDate" runat="server" visible="False">
                            <td style="width: 150px;">Start Date</td>
                            <td>
                                <telerik:RadDatePicker ID="txtStartDate" runat="server" Skin="Windows7" Culture="en-US" HiddenInputTitleAttibute="Visually hidden input created for functionality purposes." WrapperTableSummary="Table holding date picker control for selection of dates.">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" Skin="Windows7"></Calendar>

                                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy" LabelWidth="40%"></DateInput>

                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr id="trEndDate" runat="server" visible="False">
                            <td style="width: 150px;">End Date</td>
                            <td>
                                <telerik:RadDatePicker ID="txtEndDate" runat="server" Skin="Windows7" Culture="en-US" HiddenInputTitleAttibute="Visually hidden input created for functionality purposes." WrapperTableSummary="Table holding date picker control for selection of dates.">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" Skin="Windows7"></Calendar>

                                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy" LabelWidth="40%"></DateInput>

                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">Request Feedback</td>
                            <td>
                                <asp:CheckBox ID="chkFeedback" runat="server" Text="Request Candidate Feedback After Test" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">Results Viewing</td>
                            <td>
                                <asp:CheckBox ID="chkViewResults" runat="server" Text="Candidate Can View Results" />
                            </td>
                        </tr>
                        <tr id="trProctored" runat="server" visible="False">
                            <td style="width: 150px;">Enable Proctoring</td>
                            <td>
                                <asp:CheckBox ID="chkProctoring" runat="server" Text="Enable" />
                            </td>
                        </tr>
                          <tr id="trSeb" runat="server" visible="False">
                            <td style="width: 150px;">Safe Exam Browser</td>
                            <td>
                                <asp:CheckBox ID="chkSeb" runat="server" Text="Require Safe Exam Browser" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">Status</td>
                            <td>
                                <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="bttnUpdate" runat="server" Text="Update" OnClick="bttnUpdate_Click" />
                                <asp:HiddenField ID="hidId" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView2" runat="server">
                <div style="padding: 10px; border: 1px solid #ccc; margin-top: 10px; height: 360px; width: 100%; float: left;">
                    <table style="width: 100%;">
                        <tr>
                            <td>Campaign Instructions</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadEditor ID="editor" runat="server" EditModes="Design" Height="240px" Skin="Windows7" Width="700px">
                                    <Tools>
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorTool Name="FontName" />
                                            <telerik:EditorTool Name="FontSize" />
                                            <telerik:EditorTool Name="ForeColor" />
                                            <telerik:EditorTool Name="Cut" />
                                            <telerik:EditorTool Name="Copy" />
                                            <telerik:EditorTool Name="Bold" />
                                            <telerik:EditorTool Name="Underline" />
                                            <telerik:EditorTool Name="Italic" />
                                            <telerik:EditorTool Name="TableWizard" />
                                            <telerik:EditorTool Name="InsertTable" />
                                            <telerik:EditorTool Name="InsertSymbol" />
                                            <telerik:EditorTool Name="InsertUnorderedList" />
                                            <telerik:EditorTool Name="InsertOrderedList" />
                                            <telerik:EditorTool Name="JustifyCenter" />
                                            <telerik:EditorTool Name="JustifyFull" />
                                            <telerik:EditorTool Name="JustifyLeft" />
                                            <telerik:EditorTool Name="JustifyRight" />
                                            <telerik:EditorTool Name="Superscript" />
                                            <telerik:EditorTool Name="Subscript" />
                                            <telerik:EditorTool Name="Indent" />
                                        </telerik:EditorToolGroup>
                                    </Tools>
                                    <Content>
                                    </Content>
                                </telerik:RadEditor>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="bttnUpdateCampaignInstructions" runat="server" Text="Update" Style="margin-bottom: 3px;" OnClick="bttnUpdateCampaignInstructions_Click" />
                            </td>
                        </tr>
                    </table>

                </div>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView3" runat="server">
                <div style="padding: 10px; border: 1px solid #ccc; margin-top: 10px; height: 320px;">
                    <div>
                        <button id="bttnAddSession">Add Session</button>
                    </div>
                    <div>

                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" PageSize="5" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CampaignSessionDataSource" AllowAutomaticDeletes="True">
                            <ClientSettings>
                                <Selecting AllowRowSelect="True" />
                            </ClientSettings>
                            <MasterTableView DataSourceID="CampaignSessionDataSource" DataKeyNames="EntryId">
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    <HeaderStyle Width="20px" />
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    <HeaderStyle Width="20px" />
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="EntryId" DataType="System.Int32" Display="False" FilterControlAltText="Filter EntryId column" HeaderText="EntryId" SortExpression="EntryId" UniqueName="EntryId">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column" HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CenterName" FilterControlAltText="Filter CenterName column" HeaderText="Center" SortExpression="CenterName" UniqueName="CenterName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TestDate" DataFormatString="{0:dd-MMM-yyyy}" DataType="System.DateTime" FilterControlAltText="Filter TestDate column" HeaderText="Date" SortExpression="TestDate" UniqueName="TestDate">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TestTime" FilterControlAltText="Filter TestTime column" HeaderText="Time" SortExpression="TestTime" UniqueName="TestTime">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn ConfirmDialogType="RadWindow" ConfirmText="Remove this session?" FilterControlAltText="Filter column column" Text="Remove" UniqueName="column" CommandName="Delete">
                                    </telerik:GridButtonColumn>
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                            <HeaderStyle Font-Bold="True" />
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                        </telerik:RadGrid><asp:ObjectDataSource ID="CampaignSessionDataSource" runat="server" TypeName="Fot.Admin.Services.CampaignSessionService" DeleteMethod="Delete" SelectMethod="GetCampaignSessions" EnablePaging="True" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow">
                            <DeleteParameters>
                                <asp:Parameter Name="EntryId" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hidId" Name="campaignId" PropertyName="Value" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Behaviors="Close, Move, Reload" DestroyOnClose="True" Height="350px" Modal="True" Skin="Windows7" VisibleStatusbar="False" Width="650px">
                        </telerik:RadWindowManager>
                        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
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
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView4" runat="server">
                <div style="padding: 10px; border: 1px solid #ccc; margin-top: 10px; height: 320px;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 150px;">Logo Options</td>
                            <td>
                                <asp:DropDownList ID="listOptions" runat="server">
                                    <asp:ListItem Value="1">Dragnet Logo</asp:ListItem>
                                    <asp:ListItem Value="2">Dragnet Logo + Company Logo</asp:ListItem>
                                    <asp:ListItem Value="3">Company Logo</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">&nbsp;</td>
                            <td>
                                <asp:Button ID="bttnUpdateOptions" runat="server" Text="Update Options" Style="margin-bottom: 1px;" OnClick="bttnUpdateOptions_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 150px; vertical-align: top;">Company Logo</td>
                            <td style="height: 100px; vertical-align: top;">
                                <asp:Image ID="imgLogo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">&nbsp;</td>
                            <td>
                                <asp:FileUpload ID="picFile" runat="server" />
                                <asp:Button ID="bttnUpload" runat="server" Text="Upload Image" Style="margin-bottom: 1px; margin-left: 5px;" OnClick="bttnUpload_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </telerik:RadPageView>
        </telerik:RadMultiPage>




    </div>

    <div style="padding: 10px; text-align: center; clear: both; float: left; width: 100%;">

        <div class='boxDiv' onclick="location.href='CampaignCandidates.aspx?id=<%=Request.QueryString["id"] %>';">
            <div class='codeStyle'>Candidates</div>
        </div>

        <asp:Literal ID="linkScheduling" runat="server"></asp:Literal>

        <asp:Literal ID="linkResponses" runat="server"></asp:Literal>


        <div class='boxDiv' onclick="location.href='Messaging.aspx?id=<%=Request.QueryString["id"] %>';">
            <div class='codeStyle'>Messaging</div>
        </div>

        <div class='boxDiv' id="boxResult" runat="server">
            <div class='codeStyle'>Results</div>
        </div>
    </div>

    <div style="padding: 10px; text-align: center; width: 100%; float: left; clear: both">

        <div id="divCampaignReports" runat="server">
            <div class='boxDiv' onclick="location.href='Reports.aspx?id=<%=Request.QueryString["id"] %>';">
                <div class='codeStyle'>Campaign Reports</div>
            </div>
        </div>

        <div class='boxDiv' onclick="location.href='CampaignStats.aspx?id=<%=Request.QueryString["id"] %>';">
            <div class='codeStyle'>Campaign Stats</div>
        </div>

        <div class='boxDiv' onclick="location.href='CampaignFeedback.aspx?id=<%=Request.QueryString["id"] %>';">
            <div class='codeStyle'>Test Feedback</div>
        </div>
    </div>

    <div style="margin-top: 20px; float: left; margin-left: 20px; width: 100%" id="divReset" runat="server" visible="False">

        <div style="border: 1px solid #ccc; background-color: #f0f0f0; width: 500px; height: 130px; padding: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="height: 80px; vertical-align: top;"><strong>
                        <asp:Literal ID="lblCount" runat="server"></asp:Literal></strong> Candidates previously scheduled to take the assessment either did not show up for the assessment or their scores are yet to be synchronized. If you are sure that these are no-shows, click &quot;Reset Untested&quot; to put them back into the pool of unscheduled candidates.</td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Button ID="bttnReset" runat="server" Text="Reset Untested" Style="margin-bottom: 3px;" OnClick="bttnReset_Click" />
                    </td>
                </tr>
            </table>

        </div>

    </div>

    <div>



        <asp:ObjectDataSource ID="BundleDataSource" runat="server" SelectMethod="GetBundles" TypeName="Fot.Admin.Services.AssessmentBundleService">
            <SelectParameters>
                <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>


        <asp:ObjectDataSource ID="PartnerDataSource" runat="server" SelectMethod="GetNonSelfManagedPartners" TypeName="Fot.Admin.Services.PartnerService"></asp:ObjectDataSource>

    </div>
</asp:Content>
