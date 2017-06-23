<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Scheduling.aspx.cs" Inherits="Fot.Admin.Scheduling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
       
 
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Schedule Candidates</h1>
    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Campaign:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblCampaignName" runat="server"></asp:Literal>
        </div>
        <div style="float: right">
            <asp:Button ID="bttnBackToCampaignDetails" runat="server" Text="Return To Campaign Details" OnClick="bttnBackToCampaignDetails_Click" CausesValidation="False" />
           
        </div>
    </div>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>
     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" height="200px" width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
           
    <div style="padding: 0px; border: 1px solid #f0f0f0">
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Skin="Windows7" CausesValidation="False" SelectedIndex="0">
            <Tabs>
                <telerik:RadTab runat="server" Text="Schedule Candidates" Selected="True">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Unschedule Candidates">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" Width="100%" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server">
                <div style="padding: 10px;">


                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 150px;">Candidate Stats</td>
                            <td style="font-weight: 700">
                                <asp:Literal ID="lblStats" runat="server" Text="Total = 34, Scheduled = 14, Unscheduled = 20"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">Location</td>
                            <td>
                                <div style="float: left; width: 150px;">
                                    <asp:DropDownList ID="listLocations" runat="server" AutoPostBack="True" DataSourceID="LocationDataSource" DataTextField="LocationName" DataValueField="LocationId" OnSelectedIndexChanged="listLocations_SelectedIndexChanged" OnDataBound="listLocations_DataBound">
                                       
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="listLocations" CssClass="Formerror" ErrorMessage="RequiredFieldValidator" ValidationGroup="top">*</asp:RequiredFieldValidator>
                                </div>
                                <div style="float: left; width: 150px; padding-top: 5px;">Candidates in Location</div>
                                <div style="float: left; font-weight: 700; padding-top: 5px;">
                                    <asp:Literal ID="lblTotalInLocation" runat="server"></asp:Literal>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">Center</td>
                            <td>
                                <asp:DropDownList ID="listCenters" runat="server" AutoPostBack="True" DataSourceID="CenterDataSource" DataTextField="CenterName" DataValueField="CenterId">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="listCenters" CssClass="Formerror" ErrorMessage="RequiredFieldValidator" ValidationGroup="top">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">Session</td>
                            <td>
                                <asp:DropDownList ID="listSessions" runat="server" DataSourceID="SessionDataSource" DataTextField="DisplayText" DataValueField="SessionId">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="listSessions" CssClass="Formerror" ErrorMessage="RequiredFieldValidator" ValidationGroup="top">*</asp:RequiredFieldValidator>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
               
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="bttnSchedule" runat="server" Text="Schedule To Capacity" OnClick="bttnSchedule_Click" ValidationGroup="top" />
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                            </td>
                        </tr>
                    </table>


                </div>

                <div style="padding: 10px;">

                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CampaignCandidateDataSource" ItemType="Fot.Admin.Models.CampaignCandidateViewModel" OnItemCommand="RadGrid1_ItemCommand" PageSize="15">
                        <ValidationSettings CommandsToValidate="PerformInsert,Update,Schedule" />
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
                                <telerik:GridBoundColumn DataField="MobileNo" FilterControlAltText="Filter MobileNo column" HeaderText="Mobile No" SortExpression="MobileNo" UniqueName="MobileNo">
                                    <HeaderStyle Width="120px" />
                                    <ItemStyle Width="120px" />
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn CommandName="Schedule" FilterControlAltText="Filter column1 column" Text="Schedule" UniqueName="column1">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                </telerik:GridButtonColumn>

                            </Columns>

                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                            </EditFormSettings>
                        </MasterTableView>

                        <HeaderStyle Font-Bold="True" />

                        <PagerStyle Mode="Slider" />

                        <FilterMenu EnableImageSprites="False"></FilterMenu>
                    </telerik:RadGrid>
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView2" runat="server">

               

                    <div style="padding: 10px;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 150px;">Candidate Stats</td>
                                <td style="font-weight: 700">
                                    <asp:Literal ID="lblUnscheduleStats" runat="server" Text="Total = 34, Scheduled = 14, Unscheduled = 20"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">Location</td>
                                <td>
                                    <div style="float: left; width: 150px;">
                                        <asp:DropDownList ID="listUnscheduleLocation" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="listUnscheduleLocation_SelectedIndexChanged" DataSourceID="UnscheduleLocationDataSource" DataTextField="LocationName" DataValueField="LocationId" OnDataBound="listUnscheduleLocation_DataBound">
                                            
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="listUnscheduleLocation" CssClass="Formerror" ErrorMessage="RequiredFieldValidator" ValidationGroup="other">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div style="float: left; width: 150px; padding-top: 5px;">Candidates in Location</div>
                                    <div style="float: left; font-weight: 700; padding-top: 5px;">
                                        <asp:Literal ID="lblUnscheduleCandidatesInLocation" runat="server"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">Center</td>
                                <td>
                                    <asp:DropDownList ID="listUnscheduleCenter" runat="server" AutoPostBack="True" DataSourceID="UnscheduleCenterDataSource" DataTextField="CenterName" DataValueField="CenterId">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="listUnscheduleCenter" CssClass="Formerror" ErrorMessage="RequiredFieldValidator" ValidationGroup="other">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">Session</td>
                                <td>
                                    <asp:DropDownList ID="listUnscheduleSession" runat="server" DataSourceID="UnscheduleSessionDataSource" DataTextField="DisplayTextForUnscheduled" DataValueField="SessionId">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="listUnscheduleSession" CssClass="Formerror" ErrorMessage="RequiredFieldValidator" ValidationGroup="other">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">&nbsp;</td>
                                <td>
                                    <asp:Button ID="bttnUnschedule" runat="server" Text="Unschedule All From Session" ValidationGroup="other" OnClick="bttnUnschedule_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>

           
                <div style="padding: 10px;">
                    <telerik:RadGrid ID="RadGrid2" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="UnscheduledCampaignCandidateDataSource" OnItemCommand="RadGrid2_ItemCommand">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                        <MasterTableView DataSourceID="UnscheduledCampaignCandidateDataSource" DataKeyNames="EntryId">
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
                                <telerik:GridBoundColumn DataField="Username" FilterControlAltText="Filter Username column" HeaderText="Username" SortExpression="Username" UniqueName="Username">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column" HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column" HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter column column" HeaderText="Location" UniqueName="column">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MobileNo" FilterControlAltText="Filter MobileNo column" HeaderText="Mobile No" SortExpression="MobileNo" UniqueName="MobileNo">
                                    <HeaderStyle Width="120px" />
                                    <ItemStyle Width="120px" />
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn CommandName="Unschedule" FilterControlAltText="Filter column1 column" Text="Unschedule" UniqueName="column1">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                        <HeaderStyle Font-Bold="True" />
                        <PagerStyle Mode="Slider" />
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </telerik:RadGrid>
                </div>

            </telerik:RadPageView>
        </telerik:RadMultiPage>
        
    </div>
     </telerik:RadAjaxPanel>


    <div>
        <asp:ObjectDataSource ID="CampaignCandidateDataSource" runat="server" SelectMethod="GetCampaignCandidatesUnscheduled" TypeName="Fot.Admin.Services.CampaignEntryService" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="CountUnscheduled" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="EntryId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CampaignId" PropertyName="Value" Type="Int32" />
                <asp:ControlParameter ControlID="listLocations" Name="LocationId" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="UnscheduledCampaignCandidateDataSource" runat="server" SelectMethod="GetCampaignCandidatesScheduled" TypeName="Fot.Admin.Services.CampaignEntryService" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="CountScheduled" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="EntryId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CampaignId" PropertyName="Value" Type="Int32" />
                <asp:ControlParameter ControlID="listUnscheduleLocation" Name="LocationId" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="LocationDataSource" runat="server" SelectMethod="GetLocationsWithForCandidatesInCampaign" TypeName="Fot.Admin.Services.LocationService">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CampaignId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="UnscheduleLocationDataSource" runat="server" SelectMethod="GetLocationsWithForScheduledCandidatesInCampaign" TypeName="Fot.Admin.Services.LocationService">
              <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CampaignId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="CenterDataSource" runat="server" SelectMethod="GetCentersInLocation" TypeName="Fot.Admin.Services.CenterService">
            <SelectParameters>
                <asp:ControlParameter ControlID="listLocations" Name="LocationId" PropertyName="SelectedValue" Type="Int32"  DefaultValue="0"/>
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="UnscheduleCenterDataSource" runat="server" SelectMethod="GetCentersInLocation" TypeName="Fot.Admin.Services.CenterService">
            <SelectParameters>
                <asp:ControlParameter ControlID="listUnscheduleLocation" Name="LocationId" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="SessionDataSource" runat="server" SelectMethod="GetAvailableSessions" TypeName="Fot.Admin.Services.TestSessionService">
            <SelectParameters>
                <asp:ControlParameter ControlID="listCenters" Name="CenterId" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="UnscheduleSessionDataSource" runat="server" SelectMethod="GetAvailableSessionsForUnscheduleView" TypeName="Fot.Admin.Services.TestSessionService">
            <SelectParameters>
                <asp:ControlParameter ControlID="listUnscheduleCenter" Name="CenterId" PropertyName="SelectedValue" Type="Int32"  DefaultValue="0"/>
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField ID="hidId" runat="server" />

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
        </telerik:RadAjaxLoadingPanel>

        <telerik:RadWindowManager
            ID="RadWindowManager1" runat="server" Height="450px" Skin="Windows7"
            Width="650px" Modal="True" VisibleStatusbar="False"
            Behaviors="Close, Move, Reload" DestroyOnClose="True">
        </telerik:RadWindowManager>

    </div>
</asp:Content>
