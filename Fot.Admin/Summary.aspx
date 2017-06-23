<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="Fot.Admin.Summary" %>

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
    <h1 style="margin-top: 10px;">Assessment Summary -
        <asp:Literal ID="lblSummary" runat="server"></asp:Literal></h1>

    <%
        var item = GetDetails();

    %>
    
    <div style="float: left; width: 960px; margin-top: 30px;">
        <a href="QuestionSummary.aspx?id=<%= Request.QueryString["id"] %>" style="float: right;">Questions Summary</a>
    </div>

    <div style="float: left; width: 100%; margin-top: 15px;">
        <table style="width: 960px; margin-bottom: 40px;" class="mytable">
            <tr>
                <th style="width: 200px; text-align: right;">Assessment Name</th>
                <td style="text-align: left;"><%= item.AssessmentName  %></td>

            </tr>
            <tr>
                <th style="width: 200px; text-align: right;">Assessment Developer</th>
                <td style="text-align: left;"><%= item.Developer  %></td>

            </tr>
             <tr>
                <th style="width: 200px; text-align: right;">Assessment Type</th>
                <td style="text-align: left;"><%= item.AssessmentType  %></td>

            </tr>
            <tr>
                <th style="width: 200px; text-align: right;">Year Created</th>
                <td style="text-align: left;"><%= item.YearCreated  %></td>

            </tr>
               <tr>
                <th style="width: 200px; text-align: right;">Deployments</th>
                <td style="text-align: left;"><%= item.Deployments.ToString("#,##0")  %></td>

            </tr>
            

               <tr>
                <th style="width: 200px; text-align: right; vertical-align: top;">Campaigns</th>
                <td style="text-align: left;">
                     <table style="width: 100%;" class="mytable">
                         <thead>
                         <tr>
                             <th>Partner</th>
                             <th> Campaign </th>
                         </tr>
                         </thead>
                         <tbody>
                             <% foreach (var entry in item.Campaigns) { %>
                             <tr>
                                 <td><%= entry.PartnerName %></td>
                                 <td><%= entry.CampaignName %></td>
                             </tr>
                             <% } %>
                         </tbody>
                         </table>
                    

                </td>

            </tr>


        </table>

    </div>
</asp:Content>
