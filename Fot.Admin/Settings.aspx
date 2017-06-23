<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="Fot.Admin.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
         <script type="text/javascript">

             $(document).ready(function () {

                 $("#bttnAddFeedback").click(function () {

                     AddFeedbackSubject();
                     return false;
                 });

                 $("#bttnAddAuthor").click(function () {

                     AddAuthor();
                     return false;
                 });


             });

             function AddFeedbackSubject() {
                 var oWnd = radopen("Dialogs/AddFeedbackSubject.aspx", "RadWindow1");
                 oWnd.center();

             }

       

             function AddAuthor() {
                 var oWnd = radopen("Dialogs/AddAuthor.aspx", "RadWindow1");
                 oWnd.center();

             }




             function refreshGrid() {

                 var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                 radMgr.ajaxRequest("Rebind");
             }


              function refreshGrid2() {

                 var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                 radMgr.ajaxRequest("Rebind2");
             }
     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Settings</h1>
    
      <div class="contentDiv" style="padding: 10px;" align="center">
        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
    </div>
    
    <div style="padding: 0px; margin-top: 20px;">
        <fieldset style="width: 100%; border: 1px solid #ccc; "><legend style="font-size: 18px; font-weight: bold; padding-left: 10px;">Feedback Options</legend>
            
            <div style="margin-top: 10px; margin-bottom: 3px; padding: 10px; padding-bottom: 0;">
                
  <button id="bttnAddFeedback">Add Feedback Subject</button>
                
            </div>
            <div style="padding: 10px;">
                
                <telerik:RadGrid ID="GridFeedback" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="FeedbackTypesDataSource" AllowAutomaticDeletes="True" PageSize="5" AllowPaging="True">
                    <ClientSettings>
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
<MasterTableView DataSourceID="FeedbackTypesDataSource" DataKeyNames="EntryId">
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
        <telerik:GridBoundColumn DataField="FeedbackReason" FilterControlAltText="Filter FeedbackReason column" HeaderText="Feedback Subject" SortExpression="FeedbackReason" UniqueName="FeedbackReason">
        </telerik:GridBoundColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this feedback subject?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" >
            <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

                    <HeaderStyle Font-Bold="True" />

                    <PagerStyle CssClass="validate-skip" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
                </telerik:RadGrid>
                
                <asp:ObjectDataSource ID="FeedbackTypesDataSource" runat="server" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetFeedbackTypes" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.CandidateFeedbackService" DeleteMethod="Delete">
                    <DeleteParameters>
                        <asp:Parameter Name="EntryId" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                
                    <asp:ObjectDataSource ID="AuthorDatasource" runat="server" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetAuthors" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.AuthorService" DeleteMethod="Delete">
                    <DeleteParameters>
                        <asp:Parameter Name="AuthorId" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>


    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="250px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
                
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>

                <telerik:AjaxSetting AjaxControlID="GridFeedback">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="GridFeedback" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                      <telerik:AjaxSetting AjaxControlID="GridAuthor">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="GridAuthor" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="GridFeedback" />
                        <telerik:AjaxUpdatedControl ControlID="GridAuthor" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

            </AjaxSettings>
        </telerik:RadAjaxManager>
                
            </div>
            
            

        </fieldset>

    </div>
    
    <div style="padding: 0px; margin-top: 50px; min-height: 80px;">
        <fieldset style="width: 100%; border: 1px solid #ccc; min-height: 80px;"><legend style="font-size: 18px; font-weight: bold; padding-left: 10px;">Other Options</legend>
            
            <div style="padding: 10px;">
                
                <table style="width:100%;">
                    <tr>
                        <td style="width: 500px;">
                            <asp:CheckBox ID="chkAssessmentReview" runat="server" Text="Enable Partner Assessment Review For Non-Owned Assessments" />&nbsp;</td>
                         <td>
                             <asp:Button ID="bttnUpdate" runat="server" Text="Update" style="margin-bottom: 3px;" OnClick="bttnUpdate_Click"/></td>
                    </tr>
                                   
                </table>
                
                </div>
            </fieldset>
        </div>
    
        
    <div style="padding: 0px; margin-top: 20px;">
        <fieldset style="width: 100%; border: 1px solid #ccc; min-height: 400px;"><legend style="font-size: 18px; font-weight: bold; padding-left: 10px;">Assessment Authors/Developers</legend>
                        <div style="margin-top: 10px; margin-bottom: 3px; padding: 10px; padding-bottom: 0;">
                
  <button id="bttnAddAuthor">Add Author</button>
                
            </div>
            <div style="margin-top: 10px; margin-bottom: 3px; padding: 10px; padding-bottom: 0;">
                
                                <telerik:RadGrid ID="GridAuthor" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="AuthorDatasource" AllowAutomaticDeletes="True" PageSize="5" AllowPaging="True">
                    <ClientSettings>
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
<MasterTableView DataSourceID="AuthorDatasource" DataKeyNames="AuthorId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="AuthorId" DataType="System.Int32" Display="False" FilterControlAltText="Filter AuthorId column" HeaderText="AuthorId" SortExpression="AuthorId" UniqueName="AuthorId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="AuthorName" FilterControlAltText="Filter AuthorName column" HeaderText="Assessment Developer/Author" SortExpression="AuthorName" UniqueName="AuthorName">
        </telerik:GridBoundColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this author?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" >
            <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

                    <HeaderStyle Font-Bold="True" />

                    <PagerStyle CssClass="validate-skip" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
                </telerik:RadGrid>
                
                </div>
            </fieldset>
        
        </div>

</asp:Content>
