<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QuestionSummary.aspx.cs" Inherits="Fot.Admin.QuestionSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        table.mytable, .mytable th, .mytable td {
            border-collapse: collapse;
            border: 1px solid #ccc;
            padding: 5px;
        }
    </style>
    
    

    
        <script type="text/javascript">


             function ShowQuestion(ID) {
                 var oWnd = radopen("ShowQuestion.aspx?id=" + ID, "RadWindow1");
                 oWnd.center();

             }

      


     </script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 style="margin-top: 10px;">Questions Summary -
        <asp:Literal ID="lblSummary" runat="server"></asp:Literal></h1>

    <div style="float: left; width: 100%; margin-top: 20px;">
        <asp:Button ID="bttnDownload" runat="server" Text="Download" Style="margin-bottom: 1px; float: right;" OnClick="bttnDownload_Click" />
    </div>


    <%
        var entries = GetDetails();
        var ctr = 1;
    %>

    <div style="float: left; width: 100%; margin-top: 30px;">
        <table style="width: 100%; margin-bottom: 40px;" class="mytable">
            <thead>
                <tr>
                    <th style="width: 50px; text-align: left;">S/N</th>
                    <th style="width: 90px; text-align: left;">Question ID</th>
                    <th style="width: 70px; text-align: right;">Total</th>
                    <th style="width: 70px; text-align: right;">Right</th>
                    <th style="width: 70px; text-align: right;">Wrong</th>
                    <th style="width: 50px; text-align: right;">Options</th>
                    <th style="width: 90px; text-align: left;">Level</th>
                    <th style="text-align: left;">Group</th>
                    <th style="text-align: left;">Topic</th>
                    <th style="width: 90px; text-align: left;">Type</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (var item in entries)
                    { %>

                <tr>
                    <td><%= ctr %></td>
                    <td><a href="javascript:void(0)" style="text-decoration: underline" onclick="ShowQuestion('<%= item.QuestionId %>')"><%= item.QuestionId %></a></td>
                    <td style="text-align: right;"><%= item.TotalServed %></td>
                    <td style="text-align: right;"><%= item.TotalRight %></td>
                    <td style="text-align: right;"><%= item.TotalWrong %></td>
                    <td style="text-align: right;"><%= item.OptionCount %></td>
                    <td><%= item.Level %></td>
                    <td><%= item.Group %></td>
                    <td><%= item.Topic %></td>
                    <td><%= item.AnswerType %></td>
                </tr>

                   <% ctr++; %>

                <% } %>
            </tbody>
        </table>
    </div>

        <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="450px" Skin="Windows7"
        Width="950px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
</asp:Content>
