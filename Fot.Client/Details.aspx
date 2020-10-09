<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="Fot.Client.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Schedule Details (Date / Location / Venue)</h1>
    <div style="margin-top: 20px;">

        <asp:ListView ID="ListView1" runat="server" ItemPlaceholderID="ItemPlaceHolder" ItemType="Fot.Client.Models.CandidateScheduleViewModel" DataSourceID="CandidateScheduleDataSource">
            <LayoutTemplate>

                <asp:PlaceHolder ID="ItemPlaceHolder" runat="server"></asp:PlaceHolder>


            </LayoutTemplate>
            <ItemTemplate>
                <div style="margin: auto; padding: 10px; border: 1px solid #f0f0f0; width: 750px; margin-bottom: 10px;">
                    <table width="100%" cellpadding="2">
                        <tr>
                            <td style="width: 120px; font-weight: bold">Center</td>
                            <td style="margin-bottom: 10px;"><%# Item.CenterName %></td>
                            <td style="width: 150px; margin-bottom: 10px;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 120px; font-weight: bold">Address</td>
                            <td style="margin-bottom: 10px;"><%# Item.Address %></td>
                            <td style="width: 150px; margin-bottom: 10px;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 120px; font-weight: bold">Location</td>
                            <td style="margin-bottom: 10px;"><%# Item.Location %></td>
                            <td style="width: 150px; margin-bottom: 10px;">
                                <%# (Item.TestDate.HasValue == false || Item.TestDate == DateTime.Today) ? "&nbsp;" : "<a href='ChangeVenue.aspx?id=" + Item.CampaignEntryId+"' style='font-size: 13px; font-weight: bold; text-decoration: underline;'>Change Schedule</a>" %></td>
                
                        </tr>
                        <tr>
                            <td style="width: 120px; font-weight: bold">Date</td>
                            <td style="margin-bottom: 10px;"><%# Item.TestDate.HasValue? Item.TestDate.Value.ToString("dd-MMM-yyyy") : string.Empty %></td>
                            <td style="width: 150px; margin-bottom: 10px;">&nbsp;</td>
                        </tr>
                        <tr >
                            <td style="width: 120px; font-weight: bold">Time</td>
                            <td style="margin-bottom: 10px;"><%# Item.TimeText %></td>
                            <td style="width: 150px; margin-bottom: 10px;">
                                <%# "<a href='Invitation.aspx?id=" + Item.CampaignEntryId+"' target='_blank' style='font-size: 13px; font-weight: bold; text-decoration: underline;'>Accept / Reject Invitation</a>" %></td>
              
                
                    </table>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>
    
    <div>
        
        <asp:ObjectDataSource ID="CandidateScheduleDataSource" runat="server" SelectMethod="GetCandidateSchedules" TypeName="Fot.Client.Services.CandidateService">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="0" Name="CandidateId" SessionField="USERID" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
