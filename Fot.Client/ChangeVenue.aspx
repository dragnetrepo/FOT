<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangeVenue.aspx.cs" Inherits="Fot.Client.ChangeVenue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
         <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    

    <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="js/jquery.validationEngine.js" type="text/javascript"></script>
    

    <link href="css/validationEngine.jquery.css" rel="stylesheet" />
    
    

       <script type="text/javascript">


           $(document).ready(function () {

               $("#form1").validationEngine();
           });


    </script>
    
    <style type="text/css">
        
        td {
            padding: 5px;
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Change Schedule (Date / Location / Venue)</h1>
    

    
      <div style="margin-top: 20px;">
        

        <table style="width: 100%;">
            <tr>
                <td style="width: 150px;">Current Schedule</td>
                <td>
                    <asp:Label ID="lblCenter" runat="server" style="font-weight: 700"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">&nbsp;</td>
                <td>
                    <asp:Label ID="lblLocation" runat="server" style="font-weight: 700"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">&nbsp;</td>
                <td>
                    <asp:Label ID="lblDateTime" runat="server" style="font-weight: 700"></asp:Label>
                </td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td >
                    &nbsp;</td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td >
                    &nbsp;</td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td >
                    <h1>Select a new Date / Location / Venue</h1></td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td >
                    <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td >Location</td>
                <td >
                    <asp:DropDownList ID="listLocations" runat="server" style="margin-bottom: 3px;" AutoPostBack="True" DataSourceID="LocationDataSource" DataTextField="LocationName" DataValueField="LocationId" OnSelectedIndexChanged="listLocations_SelectedIndexChanged">
                      
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td >Center</td>
                <td >
                    <asp:DropDownList ID="listCenters" runat="server" style="margin-bottom: 3px;" AutoPostBack="True" DataSourceID="CenterDataSource" DataTextField="CenterName" DataValueField="CenterId" OnSelectedIndexChanged="listCenters_SelectedIndexChanged" >
                     
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td >Date / Time</td>
                <td >
                    <asp:DropDownList ID="listSessions" runat="server" style="margin-bottom: 3px;" CssClass="validate[required]" DataSourceID="SessionDataSource" DataTextField="DisplayText" DataValueField="SessionId">
                      
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td >
                    <asp:Button ID="bttnChangeVenue" runat="server" Text="Change Venue" OnClick="bttnChangeVenue_Click" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:ObjectDataSource ID="LocationDataSource" runat="server" SelectMethod="GetCampaignLocationsWithCenters" TypeName="Fot.Client.Services.LocationService">
                         <SelectParameters>
                            <asp:ControlParameter ControlID="hidCampaignId" Name="CampaignId" PropertyName="Value" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="SessionDataSource" runat="server" SelectMethod="GetCampaignAvailableSessions" TypeName="Fot.Client.Services.TestSessionService">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="listCenters" Name="CenterId" PropertyName="SelectedValue" Type="Int32" />
                             <asp:ControlParameter ControlID="hidCampaignId" Name="CampaignId" PropertyName="Value" Type="Int32" />
                             <asp:ControlParameter ControlID="hidCurrentSessionId" Name="currentSessionId" PropertyName="Value" Type="Int32" />

                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="CenterDataSource" runat="server" SelectMethod="GetCampaignCentersInLocation" TypeName="Fot.Client.Services.CenterService">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="listLocations" Name="LocationId" PropertyName="SelectedValue" Type="Int32" />
                              <asp:ControlParameter ControlID="hidCampaignId" Name="CampaignId" PropertyName="Value" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:HiddenField ID="hidCampaignEntryId" runat="server" />
                    <asp:HiddenField ID="hidCampaignId" runat="server" />
                    <asp:HiddenField ID="hidCurrentSessionId" runat="server" />
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" EnableAJAX="False">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="listLocations">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="listCenters" />
                                    <telerik:AjaxUpdatedControl ControlID="listSessions" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="listCenters">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="listSessions" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                </td>
            </tr>
        </table>
        

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>


    </div>
</asp:Content>
