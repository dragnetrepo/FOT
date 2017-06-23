<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="CampaignCandidates.aspx.cs" Inherits="Fot.Admin.Client.CampaignCandidates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style type="text/css">
        
        .FloatRight {
            float: right;
        }

        input.myButton {
            margin-bottom: 1px;
            margin-left: 10px;
        }

    </style>
    
        
    <script type="text/javascript">

        $(document).ready(function () {

            $("#bttnAddCandidate").click(function () {

                AddCandidates();
                return false;
            });
            
            $("#bttnUploadCandidates").click(function () {

                BulkUpload();
                return false;
            });



        });

        function AddCandidates() {
            var oWnd = radopen("Dialogs/AddCandidate.aspx?id=<%= hidId.Value %>", "RadWindow1");
            oWnd.center();

        }
        
        function BulkUpload() {
            var oWnd = radopen("Dialogs/CandidateUpload.aspx?id=<%= hidId.Value %>", "RadWindow1");
            oWnd.set_width(700);
            oWnd.set_height(350);
            oWnd.center();

        }




        function refreshGrid() {

            var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

            radMgr.ajaxRequest("Rebind");
        }


     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Manage Candidates In Campaign</h1>
    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
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

    <div style="padding: 10px;">
        <button id="bttnAddCandidate">Add Candidate</button> <asp:Button id="bttnUploadCandidates" CssClass="FloatRight" Text="Candidate Bulk Upload" runat="server" OnClick="bttnUploadCandidates_Click"/>
        
          </div>
        
        <div style="padding: 10px;">
            
                <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Skin="Windows7" Width="100%" SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab runat="server" Text="Candidate Search" Selected="True">
                        </telerik:RadTab>
                        <telerik:RadTab runat="server" Text="Candidate Download">
                        </telerik:RadTab>
                        <telerik:RadTab runat="server" Text="Bulk Location Change">
                        </telerik:RadTab>
                    </Tabs>
             </telerik:RadTabStrip>
            

                <telerik:RadMultiPage ID="RadMultiPage1" Runat="server" Width="100%" SelectedIndex="0">
                    <telerik:RadPageView ID="RadPageView1" runat="server">
                         <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px; margin-top: 20px;">
            <tr>
                <td style="width: 50px; padding-left: 3px;">&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="300px"></asp:TextBox>
                    <asp:Button ID="bttnSearch" runat="server" Text="Search" CssClass="myButton" OnClick="bttnSearch_Click" />
                </td>
               
            </tr>
        
            <tr>
                <td style="width: 50px; padding-left: 3px;">&nbsp;</td>
                <td class="quiet">
                    <em>Search using candidate Username, First name, Last name or Mobile Number</em></td>
               
            </tr>
        
        
         
        
        </table>
                    </telerik:RadPageView>
                      <telerik:RadPageView ID="RadPageView2" runat="server">
                                        <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px; margin-top: 20px;">
        
        
            <tr>
                <td style="width: 200px; padding-left: 3px;">
                    <asp:DropDownList ID="listDownloadType" runat="server">
                        <asp:ListItem Value="1">All Candidates</asp:ListItem>
                        <asp:ListItem Value="2">Scheduled Candidates</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="quiet">
                    <asp:Button ID="bttnDownload" runat="server" CssClass="myButton" Text="Download" OnClick="bttnDownload_Click" />
                </td>
               
            </tr>
        
         
        
                                            <tr>
                                                <td style="width: 200px; padding-left: 3px;">&nbsp;</td>
                                                <td class="quiet">&nbsp;</td>
                                            </tr>
        
         
        
        </table>
                    </telerik:RadPageView>
                      <telerik:RadPageView ID="RadPageView3" runat="server">
                                                      <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px; margin-top: 20px;">
        
        
            <tr>
                <td style="width: 200px; padding-left: 3px;">
                    <asp:DropDownList ID="listLocation" runat="server" DataSourceID="LocationDatasource" DataTextField="LocationName" DataValueField="LocationId">

                    </asp:DropDownList>
                </td>
                <td class="quiet">
                    <asp:Button ID="bttnChangeLocation" runat="server" CssClass="myButton" Text="Change Location" OnClick="bttnChangeLocation_Click" />
                </td>
               
            </tr>
        
         
        
                                            <tr>
                                                <td style="width: 200px; padding-left: 3px;">&nbsp;</td>
                                                <td class="quiet"><em>Location data of all unscheduled candidates will be changed to selected location.</em></td>
                                            </tr>
        
         
        
        </table>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            

      
        </div>
        <div style="padding: 10px;">

        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CampaignCandidateDataSource">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="CampaignCandidateDataSource" DataKeyNames="EntryId">
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
        <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column" HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column" HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter column column" HeaderText="Location" UniqueName="column">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column" HeaderText="Email" SortExpression="Email" UniqueName="Email">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="MobileNo" FilterControlAltText="Filter MobileNo column" HeaderText="Mobile No" SortExpression="MobileNo" UniqueName="MobileNo">
            <HeaderStyle Width="120px" />
            <ItemStyle Width="120px" />
        </telerik:GridBoundColumn>
        <telerik:GridCheckBoxColumn DataField="Scheduled" DataType="System.Boolean" FilterControlAltText="Filter Scheduled column" HeaderText="Scheduled ?" SortExpression="Scheduled" UniqueName="Scheduled">
            <HeaderStyle Width="90px" />
            <ItemStyle Width="90px" />
        </telerik:GridCheckBoxColumn>
        <telerik:GridCheckBoxColumn DataField="Tested" DataType="System.Boolean" FilterControlAltText="Filter Tested column" HeaderText="Tested ?" SortExpression="Tested" UniqueName="Tested">
            <HeaderStyle Width="70px" />
            <ItemStyle Width="70px" />
        </telerik:GridCheckBoxColumn>
               <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Remove candidate from this campaign?" FilterControlAltText="Filter column1 column" Text="Remove" UniqueName="column1">
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
        
        <asp:ObjectDataSource ID="CampaignCandidateDataSource" runat="server" SelectMethod="GetCampaignCandidates" TypeName="Fot.Admin.Services.CampaignEntryService" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="EntryId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CampaignId" PropertyName="Value" Type="Int32" />
                <asp:ControlParameter ControlID="txtSearch" Name="searchTerm" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource> 

        <asp:HiddenField ID="hidId" runat="server" />

          <asp:ObjectDataSource ID="LocationDataSource" runat="server" SelectMethod="GetPossibleParentLocations" TypeName="Fot.Admin.Services.LocationService"></asp:ObjectDataSource>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="480px" Skin="Windows7"
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

    </div>
</asp:Content>
