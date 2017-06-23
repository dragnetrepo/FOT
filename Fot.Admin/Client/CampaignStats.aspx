<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="CampaignStats.aspx.cs" Inherits="Fot.Admin.Client.CampaignStats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style type="text/css">
        table.mytable, .mytable th, .mytable td {
            border-collapse: collapse;
            border: 1px solid #ccc;
            padding: 5px;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Campaign Statistics</h1>
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

    <%
        var stats = GetStats();
        var scheduled = stats.Sum(x => x.Scheduled);
        var tested = stats.Sum(x => x.Tested);

    %>

    <h2 style="margin-bottom: 30px; font-size: 17px; color: #666; margin-left: 2px;">Total Scheduled [<%= scheduled.ToString("#,##0") %>] &nbsp;&nbsp;&nbsp;&nbsp; Total Tested [<%= tested.ToString("#,##0") %>]</h2>


    <% foreach (var stat in stats)
       {
    %>
    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;"><%= stat.State %> &nbsp;&nbsp;-&nbsp;&nbsp; Scheduled [<%= stat.Scheduled.ToString("#,##0") %>] &nbsp;&nbsp;&nbsp;&nbsp; Tested [<%= stat.Tested.ToString("#,##0") %>]</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <th style="text-align: left;">Center</th>
            <th style="width: 120px; text-align: right;">Scheduled</th>
            <th style="width: 120px; text-align: right;">Tested</th>
        </tr>
        <% foreach (var center in stat.Stats)
           {
        %>
        <tr>
            <td><%= center.CenterName %></td>
            <td><%= center.Scheduled.ToString("#,##0") %></td>
            <td><%= center.Tested.ToString("#,##0") %></td>
        </tr>
        <% 
           } %>
    </table>

    <% 
       } %>
    <asp:HiddenField ID="hidId" runat="server" />

</asp:Content>
