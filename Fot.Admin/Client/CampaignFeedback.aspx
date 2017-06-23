<%@ Page Title="" Language="C#" MasterPageFile="~/Client/PartnerMaster.Master" AutoEventWireup="true" CodeBehind="CampaignFeedback.aspx.cs" Inherits="Fot.Admin.Client.CampaignFeedback" %>

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
    <h1>Test Feedback</h1>
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
    <asp:HiddenField ID="hidId" runat="server" />
    

    <div style="margin-bottom: 10px; margin-top: 10px; width: 100%; float: left;">
        <div style="float: left;">
        <asp:DropDownList ID="listCenters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="listCenters_SelectedIndexChanged">
            <asp:ListItem Value="0">All Centers</asp:ListItem>
        </asp:DropDownList> 
            </div>
        
        <div style="float: right;">
            <asp:Button ID="bttnDownload" runat="server" Text="Download Comments"  style="margin-bottom: 3px;" OnClick="bttnDownload_Click"/>
        </div>
    </div>


    <%
        var stats = GetStats();

        var responses = stats.Count;
        

    %>

    <h2 style="margin-bottom: 30px; font-size: 17px; color: #666; margin-left: 2px;">Total Responses [<%= responses.ToString("#,##0") %>]</h2>



    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">Directions to the test center in the invite were accurate.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Strongly Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Directions == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Directions == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Neither Agree Nor Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Directions == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Directions == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Strongly Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Directions == 5) %></td>
        </tr>

    </table>




    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">I did not have to wait long hours at the gate before i was attended to.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Strongly Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.WaitTime == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.WaitTime == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Neither Agree Nor Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.WaitTime == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.WaitTime == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Strongly Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.WaitTime == 5) %></td>
        </tr>

    </table>


    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">I felt the testing staff were polite and professional.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Strongly Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Professionalism == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Professionalism == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Neither Agree Nor Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Professionalism == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Professionalism == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Strongly Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Professionalism == 5) %></td>
        </tr>

    </table>



    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">My test commenced at the time stated on the invite.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Strongly Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.StartTime == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.StartTime == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Neither Agree Nor Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.StartTime == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.StartTime == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Strongly Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.StartTime == 5) %></td>
        </tr>

    </table>


    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">I felt that the briefing before the test was accurate and helpful during the test.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Strongly Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Briefing == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Briefing == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Neither Agree Nor Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Briefing == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Briefing == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Strongly Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Briefing == 5) %></td>
        </tr>

    </table>


    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">I felt the online registration process was properly structured.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Strongly Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Registration == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Registration == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Neither Agree Nor Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Registration == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Registration == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Strongly Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Registration == 5) %></td>
        </tr>

    </table>


    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">Overall, I am satisfied with the test process.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Strongly Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Overall == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Agree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Overall == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Neither Agree Nor Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Overall == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Overall == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Strongly Disagree</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.Overall == 5) %></td>
        </tr>

    </table>


    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">Kindly indicate which of the following test process you consider unsatisfactory.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Invigilation</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.UnsatisfactoryArea == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Gate Control</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.UnsatisfactoryArea == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Photo Capture</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.UnsatisfactoryArea == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Ushering</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.UnsatisfactoryArea == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Test Reschedule</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.UnsatisfactoryArea == 5) %></td>
        </tr>

    </table>


    <h2 style="margin-bottom: 3px; font-size: 14px; color: #666; margin-left: 2px;">Kindly indicate which of the following test process you consider satisfactory.</h2>


    <table style="width: 960px; margin-bottom: 40px;" class="mytable">
        <tr>
            <td style="text-align: left;">Invigilation</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.SatisfactoryArea == 1) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Gate Control</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.SatisfactoryArea == 2) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Photo Capture</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.SatisfactoryArea == 3) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Ushering</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.SatisfactoryArea == 4) %></td>
        </tr>
        <tr>
            <td style="text-align: left;">Test Reschedule</td>
            <td style="width: 120px; text-align: right;"><%= stats.Count(x => x.SatisfactoryArea == 5) %></td>
        </tr>

    </table>


</asp:Content>
